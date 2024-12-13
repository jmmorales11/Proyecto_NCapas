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
    public class Proxy : IProduct, ICategory, IUser, ILogin
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



        //Product
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
            return Task.Run(async () => await SendGet<bool>($"/product/delete-product/{ID}")).Result;
        }

        public List<Product> FilterProductsByCategoryID(int ID)
        {
            return Task.Run(async () => await SendGet<List<Product>>($"/product/filter-product/{ID}")).Result;
        }

        public List<Product> GetProducts()
        {
            return Task.Run(async () => await SendGet<List<Product>>("/product/filter")).Result;
        }



        // Categorías
        public Category CreateCategory(Category newCategory)
        {
            return Task.Run(async () => await SendPost<Category, Category>("/category/create-category", newCategory)).Result;
        }

        public Category RetrieveCategoryByID(int ID)
        {
            return Task.Run(async () => await SendGet<Category>($"/category/retrieve-category/{ID}")).Result;
        }

        public bool UpdateCategory(Category CategoryToUpdate)
        {
            return Task.Run(async () => await SendPost<bool, Category>("/category/update-category", CategoryToUpdate)).Result;
        }

        public bool DeleteCategory(int ID)
        {
            return Task.Run(async () => await SendGet<bool>($"/category/delete-category/{ID}")).Result;
        }

        public List<Category> GetCategory()
        {
            return Task.Run(async () => await SendGet<List<Category>>("/category/filter-category")).Result;
        }



        // Usuarios
        public User CreateUser(User newUser)
        {
            return Task.Run(async () => await SendPost<User, User>("/user/create-user", newUser)).Result;
        }

        public User RetrieveUserByID(int ID)
        {
            return Task.Run(async () => await SendGet<User>($"/user/retrieve-user/{ID}")).Result;
        }

        public bool UpdateUser(User UserToUpdate)
        {
            return Task.Run(async () => await SendPost<bool, User>("/user/update-user", UserToUpdate)).Result;
        }

        public bool DeleteUser(int ID)
        {
            return Task.Run(async () => await SendGet<bool>($"/user/delete-user/{ID}")).Result;
        }

        public List<User> GetUsers()
        {
            return Task.Run(async () => await SendGet<List<User>>("/user/filter-user")).Result;
        }

        //Login
        public class LoginResponse
        {
            public string Token { get; set; }
            public string Email { get; set; }

            public string Role { get; set; }
            public string Message { get; set; }
        }

        // Login
        // Método para realizar login
        public async Task<LoginResponse> AuthenticateAsync(string email, string password)
        {
            var loginRequest = new
            {
                Email = email,
                Password = password
            };

            var response = await SendPost<LoginResponse, object>("/user/login", loginRequest);
            return response;
        }

        public LoginResponse Authenticate(string email, string password)
        {
            LoginResponse result = null;
            Task.Run(async () => result = await AuthenticateAsync(email, password)).Wait();
            return result;
        }

        // Método para verificar el código de autenticación
        public async Task<LoginResponse> VerifyCode(string email, string code)
        {
            var verifyRequest = new
            {
                Email = email,
                Code = code
            };

            // Llamada al endpoint /user/verify-code
            return await SendPost<LoginResponse, object>("/user/verify-code", verifyRequest);
        }

        User ILogin.Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            // Escapar el email para asegurarse de que caracteres especiales sean manejados correctamente
            string encodedEmail = Uri.EscapeDataString(email);

            // Construir la URL con el parámetro de consulta
            string requestUri = $"/user/filter-user-email?email={encodedEmail}";

            // Hacer la solicitud GET
            return Task.Run(async () => await SendGet<User>(requestUri)).Result;
        }
    }
}
