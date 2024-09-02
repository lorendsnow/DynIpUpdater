namespace DynIpUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string configPath = args.Length > 0 ? args[0] : string.Empty;
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a path to a configuration file.");
                return;
            }

            Uri baseIpifyAddress = new("https://api.ipify.org");
            Uri baseCloudflareAddress = new("https://api.cloudflare.com/client/v4");

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.ParseConfigFile(configPath);

            builder.Services.AddHostedService<UpdateService>();
            builder.Services.AddSingleton(baseCloudflareAddress);
            builder.Services.AddIpifyClient(baseIpifyAddress);
            builder.Services.AddCloudflareClient();

            IHost host = builder.Build();

            host.Run();
        }
    }
}
