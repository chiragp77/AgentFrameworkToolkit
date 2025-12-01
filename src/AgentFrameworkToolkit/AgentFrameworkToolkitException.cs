namespace AgentFrameworkToolkit;

/// <summary>
/// An AgentFrameworkToolkit Exception 
/// </summary>
/// <param name="message">Error Message</param>
/// <param name="innerException">Inner Exception</param>
public class AgentFrameworkToolkitException(string message, Exception? innerException = null) : Exception(message, innerException);