using Entities;
using Newtonsoft.Json;
using SLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoProxyService
{
    public class Proxy : IProduct
    {
        string BaseAddress = "https://localhost:44396";

        private async Task<T> SendPost<T, PostData>(string requestURI, PostData data)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;
                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var JSONData = JsonConvert.SerializeObject(data);
                    var Response = await Client.PostAsync(requestURI, new StringContent(JSONData, Encoding.UTF8, "application/json"));

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<T>(ResultWebAPI);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en SendPost: {ex.Message}");
                }
            }
            return Result;
        }

        private async Task<T> SendGet<T>(string requestURI)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;
                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ResultJSON = await Client.GetStringAsync(requestURI);
                    Result = JsonConvert.DeserializeObject<T>(ResultJSON);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en SendGet: {ex.Message}");
                }
            }
            return Result;
        }



        //Crear producto
        public Product CreateProduct(Product newProduct)
        {
            return Task.Run(async () => await SendPost<Product, Product>("/product/create-product", newProduct)).Result;

        }

        public Product RetrieveProductByID(int ID)
        {
            return Task.Run(async () => await SendGet<Product>($"/product/retrieve/{ID}")).Result;
        }

        public bool UpdateProduct(Product productToUpdate)
        {
            return Task.Run(async () => await SendPost<bool, Product>("/product/update-product", productToUpdate)).Result;
        }

        public bool DeleteProduct(int ID)
        {
            return Task.Run(async () => await SendPost<bool, int>("/product/delete-product", ID)).Result;
        }

        public List<Product> FilterProductsByCategoryID(int ID)
        {
            return Task.Run(async () => await SendGet<List<Product>>($"/product/filter-product/{ID}")).Result;
        }

        public List<Product> GetProducts()
        {
            return Task.Run(async () => await SendGet<List<Product>>("/product/filter")).Result;
        }
    }
}
