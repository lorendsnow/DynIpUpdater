namespace DynIpUpdater
{
    /// <summary>
    /// Represents a configuration for Cloudflare records. Supports multiple zones, each with
    /// multiple records.
    /// </summary>
    public class CloudflareConfiguration
    {
        /// <summary>
        /// A list of <see cref="ZoneConfiguration"/> objects.
        /// </summary>
        public required List<ZoneConfiguration> Zones { get; set; }

        /// <summary>
        /// Returns a string representation of the configuration.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "CloudflareConfiguration "
                + "{ "
                + $"Zones: [{string.Join(", ", Zones)}] "
                + "}";
        }
    }
}
