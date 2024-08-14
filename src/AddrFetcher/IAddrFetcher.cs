namespace DynIpUpdater
{
    public interface IAddrFetcher
    {
        Task<IAddress> FetchAddressAsync();
    }
}
