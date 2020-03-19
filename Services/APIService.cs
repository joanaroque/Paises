namespace Paises.Services
{
    using Newtonsoft.Json;
    using Paises.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    public class APIService
    {
        //devolver uma Response, vai buscar as taxas
        public async Task<Response> GetCountries(string urlBase, string controller)
        {
            //Sempre try
            try
            {
                //carrega paises, API, para fazer ligação externa
                var country = new HttpClient();

                country.BaseAddress = new Uri(urlBase);

                var response = await country.GetAsync(controller);

                //este objeto fica a espera da resposta que vem de cima
                //o seu conteudo lido como uma string
                var result = await response.Content.ReadAsStringAsync();


                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,

                    };
                }
                //se chegou aqui, correu bem
                //recebo o json, converte e pomo lo numa lista de dados do tipo Rate
                var countries = JsonConvert.DeserializeObject<RootObject>(result);

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
    }
}
