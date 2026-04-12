using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public sealed class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient()
        {
            _http = new HttpClient { BaseAddress = new Uri("https://grgac.ase-lab.ovh/") };
        }

        public async Task<T> GetAsync<T>(string relativeUrl) // Kristian Katulić
        {
            var res = await _http.GetAsync(relativeUrl);
            res.EnsureSuccessStatusCode();
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
        public async Task<T> PostAsync<T>(string relativeUrl, object body) // Kristian Katulić
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var res = await _http.PostAsync(relativeUrl, content);
            res.EnsureSuccessStatusCode();

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<T> PostJsonAsync<T>(string relativeUrl, object body) // Karlo Kršak
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var res = await _http.PostAsync(relativeUrl, content);
            var responseBody = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                try
                {
                    dynamic err = JsonConvert.DeserializeObject(responseBody);
                    string msg = err?.error != null ? (string)err.error : responseBody;
                    throw new Exception(msg);
                }
                catch
                {
                    throw new Exception(responseBody);
                }
            }

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<TResponse> PostMatchAsync<TRequest, TResponse>(string relativeUrl, TRequest body) // Filip Grgac
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await _http.PostAsync(relativeUrl, content);
            res.EnsureSuccessStatusCode();

            var responseJson = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }
    }
}
