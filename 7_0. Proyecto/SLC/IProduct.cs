using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface IProduct
    {
        Product CreateProduct(Product newProduct);
        Product RetrieveProductByID(int ID);
        bool UpdateProduct(Product productToUpdate);
        bool DeleteProduct(int ID);
        List<Product> FilterProductsByCategoryID(int ID);
        List<Product> GetProducts();

    }
}
