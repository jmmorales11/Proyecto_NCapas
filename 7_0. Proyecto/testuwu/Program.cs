using ProyectoProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testuwu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Instancia del proxy
            var proxy = new Proxy();

            // Prueba: Filtrar productos por categoría
            Console.WriteLine("Probando FilterProductsByCategoryID...");
            int categoryId = 1; // Cambia el ID según lo necesites
            var products = proxy.FilterProductsByCategoryID(categoryId);

            // Verifica los resultados
            if (products != null && products.Count > 0)
            {
                Console.WriteLine($"Se encontraron {products.Count} productos en la categoría {categoryId}:");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductID}, Nombre: {product.ProductName}, Precio: {product.UnitPrice}, Stock: {product.UnitsInStock}");
                }
            }
            else
            {
                Console.WriteLine("No se encontraron productos o la respuesta fue nula.");
            }

            // Mantén la consola abierta
            Console.WriteLine("Pruebas completadas. Presiona cualquier tecla para salir.");
            Console.ReadKey();
        }
    }
}
