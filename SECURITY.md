# Security Policy

## Supported Versions

AgentFramework Toolkit ships coordinated releases for all NuGet packages. Only the most recent preview/release published from the `main` branch receives security updates. If you're running an older version, plan to upgrade before requesting a fix.

## Reporting a Vulnerability

1. **Do not** open a public GitHub issue for security problems.
2. Submit a private report via the GitHub security advisory form: https://github.com/rwjdk/AgentFrameworkToolkit/security/advisories/new
3. Include:
   - A clear description of the issue and why it is a vulnerability.
   - Steps to reproduce (including sample code, environment details, and configuration values where possible).
   - The impact/severity you believe the issue has.
   - Any suggested mitigations.

If you need encrypted communication, mention it in the report and a maintainer will respond with PGP details.

## Response Process

1. You’ll receive an acknowledgment within 3 business days.
2. The maintainers will investigate, reproduce, and classify the report.
3. A coordinated fix will be developed and tested using the same pipeline defined in `.github/workflows/Build.yml`.
4. A patched release will be published and the advisory will be updated before public disclosure.

## Coordinated Disclosure

We follow responsible disclosure practices:

- You will be credited (if desired) once the fix is released.
- Please do not publicly disclose the vulnerability before the maintainers complete remediation or give you permission.

## Hardening Guidelines

- Keep dependencies up to date via `Directory.Packages.props`.
- Run the full `dotnet build`/`dotnet test` workflow locally before submitting PRs.
- Rotate API keys used in `development/Secrets` regularly and avoid committing secrets to the repository.

Thank you for helping keep AgentFramework Toolkit secure!***
