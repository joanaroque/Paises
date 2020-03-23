﻿namespace Countries.Services
{
    using Newtonsoft.Json;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    public class APIService
    {
        //devolver uma Response, vai buscar as taxas
        public async Task<Response> GetCountries(string urlBase, string apiPath)
        {
            //Sempre try
            try
            {
                var client = new HttpClient(); // cliente prepara chamada a um servidor (cliente é alguem que pede algo)

                client.BaseAddress = new Uri(urlBase); // "Qual é a morada que eu vou chamar? (que é sempre um URI)"

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

                var countries = JsonConvert.DeserializeObject<List<RootObject>>(result, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

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
