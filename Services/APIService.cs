namespace Countries.Services
{
    using Newtonsoft.Json;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public static class APIService
    {
        public const string url = "http://restcountries.eu/rest/v2/";
        public const string path = "all";

        public const string urlExtra = "https://coronavirus-19-api.herokuapp.com/";
        public const string pathExtra = "countries";
        /// <summary>
        /// Connect to the api and retrieve the data in it
        /// </summary>
        /// <param name="urlBase">Url of the API</param>
        /// <param name="apiPath">Folder where the data is</param>
        /// <returns></returns>
        public static async Task<Response> GetCountries(string urlBase, string apiPath)
        {
            try
            {
                //ProgressReport report = new ProgressReport();

                var client = new HttpClient(); // cliente prepara chamada a um servidor (cliente é alguem que pede algo)

                client.BaseAddress = new Uri(urlBase); // "Qual é a morada que eu vou chamar? (que é sempre um URI)"
                
                Console.WriteLine($"Fetching countries from API: {DateTime.Now}");
                var response = await client.GetAsync(apiPath); // para este clt configurado previamente, vai buscar a info dos clints de forma assincrona

                //le a resposta e converte a de binario para string 
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,

                    };
                }

                var countries = JsonConvert.DeserializeObject<List<Country>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Console.WriteLine($"Deserialized countries from json: {DateTime.Now}");

                //report.Percentage = (countries.Count * 100) / 125;
                //progress.Report(report);

                return new Response
                {
                    IsSuccess = true,
                    Result = countries
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public static async Task<Response> GetCovid19Data(string urlBase, string apiPath)
        {
            try
            {
                //ProgressReport report = new ProgressReport();

                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.GetAsync(apiPath);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,

                    };
                }

                var covid19 = JsonConvert.DeserializeObject<List<Covid19Data>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //report.Percentage = (covid19.Count * 100) / 125;
                //progress.Report(report);

                return new Response
                {
                    IsSuccess = true,
                    Result = covid19
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}

