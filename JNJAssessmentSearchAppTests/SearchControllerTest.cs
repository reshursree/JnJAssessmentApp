using JNJAssessmentSearchApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace JNJAssessmentSearchApp
{
    public class SearchControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SearchControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Search_WithValidInput_ReturnsViewWithResults()
        {
            var searchTerm = "R4";
            var searchType = "name";

            var url = $"/JNJ/Search?Type={searchType}&Term={searchTerm}";

            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.NotNull(responseBody);
            Assert.Contains("JNJSearch", responseBody);
        }
    }
}