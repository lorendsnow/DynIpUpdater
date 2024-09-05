namespace DynIpUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting DynIpUpdater");

            Uri baseIpifyAddress = new("https://api.ipify.org/");
            Uri baseCloudflareAddress = new("https://api.cloudflare.com/client/v4/");

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            Console.WriteLine("Loading configuration file...");
            CloudflareConfiguration config = LoadConfiguration.ParseConfigFile(args);

            builder.Services.AddHostedService<UpdateService>();
            builder.Services.AddSingleton(baseCloudflareAddress);
            builder.Services.AddIpifyClient(baseIpifyAddress);
            builder.Services.AddCloudflareClient();
            builder.Services.AddSingleton<IAddrFetcher, IpifyFetcher>();
            builder.Services.AddSingleton<ICloudflareClient, CloudflareClient>();
            builder.Services.AddSingleton(config);

            builder.ConfigureLogging(config.Verbosity);

            IHost host = builder.Build();

            host.Run();
        }
    }
}
