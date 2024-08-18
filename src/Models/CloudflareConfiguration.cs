namespace DynIpUpdater
{
    public class CloudflareConfiguration
    {
        public required List<ZoneConfiguration> Zones { get; set; }

        public override string ToString()
        {
            return "CloudflareConfiguration "
                + "{ "
                + $"Zones: [{string.Join(", ", Zones)}] "
                + "}";
        }
    }
}
