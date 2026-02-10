using System.Net;
using System.Net.Mail;
using JetBrains.Annotations;
using Microsoft.Extensions.AI;

namespace AgentFrameworkToolkit.Tools.Common;

/// <summary>
/// Tools for SMTP email operations
/// </summary>
[PublicAPI]
public static class EmailTools
{
    private static readonly TimeSpan DefaultSendTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Get all email tools with their default settings
    /// </summary>
    /// <param name="options">Optional options</param>
    /// <returns>Tools</returns>
    public static IList<AITool> All(EmailToolsOptions? options = null)
    {
        return [Send(options)];
    }

    /// <summary>
    /// Send email through SMTP
    /// </summary>
    /// <param name="options">Optional options</param>
    /// <param name="toolName">Name of tool</param>
    /// <param name="toolDescription">Description of tool</param>
    /// <returns>Tool</returns>
    public static AITool Send(EmailToolsOptions? options = null, string? toolName = null, string? toolDescription = null)
    {
        return AIFunctionFactory.Create(async (
            string to,
            string subject,
            string body,
            bool isHtml = false,
            string? cc = null,
            string? bcc = null) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject))
                {
                    throw new ArgumentException("Parameter 'subject' must be provided.");
                }

                if (string.IsNullOrWhiteSpace(body))
                {
                    throw new ArgumentException("Parameter 'body' must be provided.");
                }

                EmailToolsOptions optionsToUse = GuardAndGetOptions(options);

                IList<MailAddress> toAddresses = ParseAddresses(to, nameof(to), isRequired: true);
                IList<MailAddress> ccAddresses = ParseAddresses(cc, nameof(cc), isRequired: false);
                IList<MailAddress> bccAddresses = ParseAddresses(bcc, nameof(bcc), isRequired: false);
                List<MailAddress> allRecipients = [.. toAddresses, .. ccAddresses, .. bccAddresses];
                GuardThatRecipientsAreWithinConfinedDomains(allRecipients, optionsToUse);

                using MailMessage mailMessage = new();
                mailMessage.From = CreateFromAddress(optionsToUse);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isHtml;

                AddAddresses(mailMessage.To, toAddresses);
                AddAddresses(mailMessage.CC, ccAddresses);
                AddAddresses(mailMessage.Bcc, bccAddresses);

                using SmtpClient smtpClient = CreateSmtpClient(optionsToUse);
                try
                {
                    await smtpClient.SendMailAsync(mailMessage).WaitAsync(DefaultSendTimeout);
                }
                catch (TimeoutException exception)
                {
                    throw new InvalidOperationException(
                        $"Timed out while sending email to SMTP host '{optionsToUse.Host}:{optionsToUse.Port}'. " +
                        "Verify SMTP host/port, secure connection setting, credentials, and firewall rules.",
                        exception);
                }

                return $"Email sent. From: '{optionsToUse.FromAddress}'. To: {toAddresses.Count}. Cc: {ccAddresses.Count}. Bcc: {bccAddresses.Count}. Subject: '{subject}'.";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }, toolName ?? "send_email", toolDescription ?? "Send an email via SMTP");
    }

    private static EmailToolsOptions GuardAndGetOptions(EmailToolsOptions? options)
    {
        EmailToolsOptions optionsToUse = options ?? throw new InvalidOperationException("EmailToolsOptions must be provided.");

        if (string.IsNullOrWhiteSpace(optionsToUse.Host))
        {
            throw new InvalidOperationException("EmailToolsOptions.Host must be provided.");
        }

        if (optionsToUse.Port <= 0 || optionsToUse.Port > 65535)
        {
            throw new InvalidOperationException("EmailToolsOptions.Port must be between 1 and 65535.");
        }

        if (string.IsNullOrWhiteSpace(optionsToUse.FromAddress))
        {
            throw new InvalidOperationException("EmailToolsOptions.FromAddress must be provided.");
        }

        bool hasUsername = !string.IsNullOrWhiteSpace(optionsToUse.Username);
        bool hasPassword = !string.IsNullOrWhiteSpace(optionsToUse.Password);
        if (hasUsername ^ hasPassword)
        {
            throw new InvalidOperationException("EmailToolsOptions.Username and EmailToolsOptions.Password must both be provided together.");
        }

        return optionsToUse;
    }

    private static void GuardThatRecipientsAreWithinConfinedDomains(IEnumerable<MailAddress> recipients, EmailToolsOptions options)
    {
        if (options.ConfineSendingToTheseDomains == null)
        {
            return;
        }

        HashSet<string> allowedDomains = [];
        foreach (string domain in options.ConfineSendingToTheseDomains)
        {
            string normalizedDomain = NormalizeHostOrDomain(domain);
            if (!string.IsNullOrWhiteSpace(normalizedDomain))
            {
                allowedDomains.Add(normalizedDomain);
            }
        }

        if (allowedDomains.Count == 0)
        {
            throw new InvalidOperationException("EmailToolsOptions.ConfineSendingToTheseDomains must contain at least one valid domain.");
        }

        foreach (MailAddress recipient in recipients)
        {
            string recipientDomain = NormalizeHostOrDomain(recipient.Host);
            if (!allowedDomains.Contains(recipientDomain))
            {
                throw new InvalidOperationException($"Recipient '{recipient.Address}' is not within the allowed recipient domains.");
            }
        }
    }

    private static IList<MailAddress> ParseAddresses(string? addresses, string parameterName, bool isRequired)
    {
        if (string.IsNullOrWhiteSpace(addresses))
        {
            if (isRequired)
            {
                throw new ArgumentException($"Parameter '{parameterName}' must be provided.");
            }

            return [];
        }

        string[] rawAddresses = addresses.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (rawAddresses.Length == 0)
        {
            if (isRequired)
            {
                throw new ArgumentException($"Parameter '{parameterName}' must contain at least one email address.");
            }

            return [];
        }

        List<MailAddress> result = [];
        foreach (string rawAddress in rawAddresses)
        {
            result.Add(new MailAddress(rawAddress));
        }

        return result;
    }

    private static void AddAddresses(MailAddressCollection target, IEnumerable<MailAddress> source)
    {
        foreach (MailAddress address in source)
        {
            target.Add(address);
        }
    }

    private static MailAddress CreateFromAddress(EmailToolsOptions options)
    {
        return string.IsNullOrWhiteSpace(options.FromDisplayName)
            ? new MailAddress(options.FromAddress!)
            : new MailAddress(options.FromAddress!, options.FromDisplayName);
    }

    private static SmtpClient CreateSmtpClient(EmailToolsOptions options)
    {
        SmtpClient smtpClient = new(options.Host, options.Port)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = options.UseSecureConnection,
            UseDefaultCredentials = false
        };

        if (!string.IsNullOrWhiteSpace(options.Username))
        {
            smtpClient.Credentials = new NetworkCredential(options.Username, options.Password);
        }

        return smtpClient;
    }

    private static string NormalizeHostOrDomain(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        if (Uri.TryCreate(value, UriKind.Absolute, out Uri? uri))
        {
            return uri.Host.Trim().TrimEnd('.');
        }

        if (Uri.TryCreate($"smtp://{value}", UriKind.Absolute, out Uri? smtpUri))
        {
            return smtpUri.Host.Trim().TrimEnd('.');
        }

        return value.Trim().TrimEnd('.');
    }
}

/// <summary>
/// Options for email tools
/// </summary>
[PublicAPI]
public class EmailToolsOptions
{
    /// <summary>
    /// SMTP host (Required)
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    /// SMTP port (Default: 587)
    /// </summary>
    public int Port { get; set; } = 587;

    /// <summary>
    /// Use a secure SMTP transport (typically TLS via STARTTLS/SMTPS) (Default: true)
    /// </summary>
    public bool UseSecureConnection { get; set; } = true;

    /// <summary>
    /// Username for SMTP AUTH (Optional)
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for SMTP AUTH (Optional, requires Username)
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Sender email address (Required)
    /// </summary>
    public required string FromAddress { get; set; }

    /// <summary>
    /// Sender display name (Optional)
    /// </summary>
    public string? FromDisplayName { get; set; }

    /// <summary>
    /// Restrict recipients to these email domains (Optional)
    /// </summary>
    public IList<string>? ConfineSendingToTheseDomains { get; set; }
}
