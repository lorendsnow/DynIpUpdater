namespace DynIpUpdater
{
    public class UpdateService(ILogger<UpdateService> logger, IAddrFetcher addrFetcher)
        : BackgroundService
    {
        private readonly ILogger<UpdateService> _logger = logger;
        private readonly IAddrFetcher _addrFetcher = addrFetcher;

        public IAddress? CurrentAddress { get; set; }

        public bool AddressSet
        {
            get => CurrentAddress is not null;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!AddressSet)
                {
                    CurrentAddress = await _addrFetcher.FetchAddressAsync();
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Ip Address is {address}", CurrentAddress.Address);
                    }
                }
                else
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation(
                            "Ip Address is already set to {address}",
                            CurrentAddress?.Address
                        );
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
