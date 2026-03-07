using CesarBmx.CryptoWatcher.Application.FakeResponses;
using CesarBmx.CryptoWatcher.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.CryptoWatcher.Api.ResponseExamples
{
    public class LogListResponseExample : IExamplesProvider<List<LogResponse>>
    {
        public List<LogResponse> GetExamples()
        {
            return FakeLog.GetFake_List();
        }
    }
}