using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataLibrary.Models;

namespace DataLibrary.Logic
{
    public static class FreshserviceClient
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<List<AssetModel>> GetAssetsFromFreshserviceAsync(string apiKey)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:X")));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("https://yourdomain.freshservice.com/api/v2/assets");
            response.EnsureSuccessStatusCode();

            string responseData = await response.Content.ReadAsStringAsync();
            var assets = JsonConvert.DeserializeObject<List<AssetModel>>(responseData);

            return assets;
        }

        public static async Task UpdateAssetInFreshserviceAsync(string apiKey, AssetModel asset)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:X")));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(asset);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"https://yourdomain.freshservice.com/api/v2/assets/{asset.Id}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
