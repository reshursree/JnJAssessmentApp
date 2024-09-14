using JNJAssessmentSearchApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;

namespace JNJAssessmentSearchApp.Controllers
{
    public class JNJController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IHttpClientFactory _httpClientFactory;

        public JNJController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var model = new SearchModel();
            return View("JNJSearch");
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();
                string apiUrl;

                // If the type is "name", use the search endpoint. Otherwise, treat it as an ID.
                if (model.Type.ToLower() == "name")
                {
                    apiUrl = $"{_configuration["SwapiSearchURL"]}/?search={model.Term}";
                }
                else
                {
                    apiUrl = $"{_configuration["SwapiSearchURL"]}/{model.Term}/";
                }

                try
                {
                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var searchResult = JsonConvert.DeserializeObject<dynamic>(content);

                        if (searchResult != null)
                        {
                            var people = new List<PersonModel>();
                            if (searchResult.results != null)
                            {
                                people = JsonConvert.DeserializeObject<List<PersonModel>>(searchResult.results.ToString());
                            }
                            else
                            {
                                people.Add(JsonConvert.DeserializeObject<PersonModel>(searchResult.ToString()));
                            }

                            if (people.Any())
                            {
                                model.Persons = people;
                                return View("JNJSearch", model);
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "No Results Found.");
                                return View("JNJSearch", model);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"API Request Failed. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred during the API call: {ex.Message}");
                }
            }

            return View("JNJSearch", model);
        }
    }
}