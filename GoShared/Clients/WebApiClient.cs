using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoShared.Clients;

public class WebApiClient
{
    private static HttpClient ApiClient { get; set; } = new HttpClient();

    private static void InitializeClient()
    {
        ApiClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8080/api/")
        };
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static async Task<T> CallWebApi<T, U>(U requestModel, string urlPath) where T : class, new() where U : class
    {
        //if (!requestModel.GetType().IsClass)
        //{
        //    return new T();
        //}

        InitializeClient();

        using HttpResponseMessage responseMessage = await ApiClient.PostAsJsonAsync(urlPath, requestModel);

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<T>() ?? new T();
        }

        return new T();
    }
}
