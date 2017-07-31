namespace W.Net
{
    /// <summary>
    /// Extends SecureServer by supporting generics
    /// </summary>
    /// <typeparam name="TType">The type to transmit and receive</typeparam>
    public class GenericServer<TType> : W.Net.Sockets.SecureServer<GenericClient<TType>>
    {
    }
}