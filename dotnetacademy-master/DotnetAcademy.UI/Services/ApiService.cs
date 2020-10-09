using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace DotnetAcademy.UI.Services {
    public static class ApiService<T> where T : class {
        public static async Task<T> GetApi(string path, object authToken = null) {
            T obj = null;

            using (var client = new HttpClient()) {
                HttpResponseMessage result;

                client.BaseAddress = new Uri("https://localhost:44395/api/");

                if (authToken != null) {
                    //Add the authorization header
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + authToken);
                }

                try {
                    result = await client.GetAsync(path);
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                    throw;
                }

                if (result.IsSuccessStatusCode) {
                    var json = await result.Content.ReadAsStringAsync();

                    obj = JsonConvert.DeserializeObject<T>(json);
                }

                return obj;
            }
        }

        public static async Task<string> PostApi(string path, T data, object authToken = null) {
            string json = null;

            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri("https://localhost:44395/api/");

                if (authToken != null) {
                    //Add the authorization header
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + authToken);
                }

                HttpResponseMessage result;
                try {
                    result = await client.PostAsJsonAsync(path, data);
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                    throw;
                }

                if (result.IsSuccessStatusCode) {
                    json = await result.Content.ReadAsStringAsync();
                }

                return json;
            }
        }

        public static async Task<T> AuthenticateAsync(string userName, string password, string path) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri("https://localhost:44395/api/");
                var result = await client.PostAsync(path, new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("userName", userName),
                        new KeyValuePair<string, string>("password", password)
                    }));

                string json = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode) {
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return null;
            }
        }

    }
}