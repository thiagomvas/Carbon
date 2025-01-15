namespace Carbon.Core;


public interface ICliWriter
{
    void WriteSuccess(string message, params object[]? args);
    void WriteError(string message, params object[]? args);
    void WriteInfo(string message, params object[]? args);
    void WriteDebug(string message, params object[]? args);
}