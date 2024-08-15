using DynIpUpdater;

namespace Tests
{
    public class IpifyClientFactoryTests
    {
        [Fact]
        public void CreateIpifyClient_ReturnsHttpClient()
        {
            var expected = new HttpClient() { BaseAddress = new Uri("https://api.ipify.org") };

            var actual = IpifyClientFactory.CreateClient();

            Assert.Equal(expected.BaseAddress, actual.BaseAddress);
        }
    }
}
