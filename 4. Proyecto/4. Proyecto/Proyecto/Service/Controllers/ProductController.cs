using BLL;
using Entities;
using SLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Service.Controllers
{
    [RoutePrefix("product")] 
    public class ProductController : ApiController, IProduct
    {
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        [Route("create-product")] 
        public Product CreateProduct(Product newProduct)
        {
            var BL = new Products();
            var NewProduct = BL.Create(newProduct);
            return NewProduct;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("delete-product/{id}")]
        public bool DeleteProduct(int id)
        {
            var BL = new Products();
            var Result = BL.Delete(id);
            return Result;
        }

        //obtiene todos los productos de la categoria seleccionada
        [HttpGet]
        [Authorize(Roles = "Admin, Viewer")]
        [Route("filter-product/{id}")]
        public List<Product> FilterProductsByCategoryID(int id)
        {
            var BL = new Products();
            var Result = BL.FilterByCategoryID(id);
            return Result;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Viewer")]
        [Route("filter")]
        public List<Product> GetProducts()
        {
            var BL = new Products();
            var result = BL.GetAllProduct();
            return result;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Viewer")]
        [Route("retrieve/{id}")]
        public Product RetrieveProductByID(int id)
        {
            var BL = new Products();
            var Result = BL.RetrieveByID(id);
            return Result;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        [Route("update-product")]
        public bool UpdateProduct(Product productToUpdate)
        {
            var BL = new Products();
            var Result = BL.Update(productToUpdate);
            return Result;
        }
    }
}
