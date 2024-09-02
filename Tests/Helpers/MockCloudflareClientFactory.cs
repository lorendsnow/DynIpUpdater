namespace Tests.Helpers
{
    public class MockCloudflareClientFactory(MockHttpMessageHandler handler) : IHttpClientFactory
    {
        private readonly MockHttpMessageHandler _handler = handler;

        public HttpClient CreateClient(string name)
        {
            HttpClient client = new(_handler);
            return client;
        }

        public HttpClient CreateClient()
        {
            return CreateClient("MockedClient");
        }
    }
}
