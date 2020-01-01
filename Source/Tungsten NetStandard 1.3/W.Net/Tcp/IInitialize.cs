namespace W.Net
{
    public static partial class Tcp
    {
        public interface IInitialize
        {
            bool Initialize(params object[] args);
        }
    }
}
