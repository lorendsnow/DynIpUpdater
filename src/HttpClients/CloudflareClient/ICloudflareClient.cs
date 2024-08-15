namespace DynIpUpdater
{
    public interface ICloudflareClient
    {
        public IHttpClientFactory Factory { get; }
    }
}
