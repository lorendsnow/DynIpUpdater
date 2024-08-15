namespace DynIpUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddHostedService<UpdateService>();
            builder.Services.AddIpifyClient();

            IHost host = builder.Build();

            host.Run();
        }
    }
}
