using CesarBmx.CryptoWatcher.Application.Requests;


namespace CesarBmx.CryptoWatcher.Application.FakeRequests
{
    public static class FakeDisableWatcher
    {
        public static DisableWatcherRequest GetFake_1()
        {
            return new DisableWatcherRequest
            {
                WatcherId = 1,
                UserId = "cesarbmx"
            };
        }       
    }
}
