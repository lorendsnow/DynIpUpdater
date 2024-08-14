namespace DynIpUpdater
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AddrFetcher fetcher = new();

            IAddress address = await fetcher.FetchAddressAsync();

            Console.WriteLine(address);
        }
    }
}
