
namespace W.Net.Sockets
{
    /// <summary>
    /// Must be implemented by clients to be used by SecureServer
    /// </summary>
    public interface ISecureSocket : ISocket { }
    ///// <summary>
    ///// Must be implemented by clients to be used by SecureServer
    ///// </summary>
    //public interface ISecureSocket2<TDataType> : ISocket2<TDataType> { }
    /// <summary>
    /// Must be implemented by clients to be used by SecureServer
    /// </summary>
    public interface ISecureSocket<TClientType, TDataType> : IMessageSocket<TClientType, TDataType> { }
}