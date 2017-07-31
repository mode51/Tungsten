#if NETSTANDARD1_3 || NET45 || NETCOREAPP1_0

namespace W.Encryption
{
    /// <summary>
    /// The public and private RSA keys
    /// </summary>
    public class PublicPrivateKeyPair
    {
        /// <summary>
        /// The private RSA key.  Should never be shared.
        /// </summary>
        public System.Security.Cryptography.RSAParameters PrivateKey { get; set; }
        /// <summary>
        /// the public RSA key. Should be shared.
        /// </summary>
        public System.Security.Cryptography.RSAParameters PublicKey { get; set; }
    }
}
#endif
