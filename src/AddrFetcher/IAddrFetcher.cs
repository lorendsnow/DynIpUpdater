namespace DynIpUpdater
{
    /// <summary>
    /// Generic interface for fetching an IP address.
    /// </summary>
    public interface IAddrFetcher
    {
        Task<IAddress> FetchAddressAsync();
    }
}
