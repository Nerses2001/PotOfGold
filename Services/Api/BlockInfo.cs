using Newtonsoft.Json;
using PotOfGold.Services.Api.Interfases;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace PotOfGold.Services.Api
{
    internal class BlockInfo:IGetRequests
    {
        private readonly HttpClient _client ;

        public BlockInfo()
        {
            _client  = new HttpClient();   
        }

        public async Task<T> Get<T>(string url)
        {

            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            else
            {
                throw new Exception($"Request failed with status: {response.StatusCode}.");
            }
        }
    }
}
