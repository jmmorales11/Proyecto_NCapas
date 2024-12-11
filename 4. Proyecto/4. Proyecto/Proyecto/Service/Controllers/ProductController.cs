using BLL;
using Entities;
using SLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Service.WebApiConfig;

namespace Service.Controllers
{
    [RoutePrefix("product")] 
    public class ProductController : ApiController, IProduct
    {
        [HttpPost]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin", "Editor")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("create-product")] 
        public Product CreateProduct(Product newProduct)
        {
            var BL = new Products();
            var NewProduct = BL.Create(newProduct);
            return NewProduct;
        }

        [HttpGet]
<<<<<<< HEAD
=======

        [RoleAuthorize("Admin")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("delete-product/{id}")]
        public bool DeleteProduct(int id)
        {
            var BL = new Products();
            var Result = BL.Delete(id);
            return Result;
        }

        //obtiene todos los productos de la categoria seleccionada
        [HttpGet]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin", "Viewer")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("filter-product/{id}")]
        [AllowAnonymous]
        public List<Product> FilterProductsByCategoryID(int id)
        {
            var BL = new Products();
            var Result = BL.FilterByCategoryID(id);
            return Result;
        }

        [HttpGet]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin", "Viewer")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("filter")]
        [AllowAnonymous]
        public List<Product> GetProducts()
        {
            var BL = new Products();
            var result = BL.GetAllProduct();
            return result;
        }

        [HttpGet]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin", "Viewer")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("retrieve/{id}")]
        public Product RetrieveProductByID(int id)
        {
            var BL = new Products();
            var Result = BL.RetrieveByID(id);
            return Result;
        }

        [HttpPost]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin", "Editor")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("update-product")]
        public bool UpdateProduct(Product productToUpdate)
        {
            var BL = new Products();
            var Result = BL.Update(productToUpdate);
            return Result;
        }
    }
}
