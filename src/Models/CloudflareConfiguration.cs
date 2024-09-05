namespace DynIpUpdater
{
    /// <summary>
    /// Represents a configuration for Cloudflare records. Supports multiple zones, each with
    /// multiple records.
    /// </summary>
    public class CloudflareConfiguration
    {
        /// <summary>
        /// Interval in minutes to wait between IP checks.
        /// </summary>
        public int Interval { get; init; } = 5;

        /// <summary>
        /// Set the logging output level.
        /// </summary>
        /// <remarks>
        /// 0 = No logging, 1 = Error, 2 = Warning, 3 = Info, 4 = Info plus include Microsoft and
        /// System logging up to Info.
        /// </remarks>
        public int Verbosity { get; init; } = 3;

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
