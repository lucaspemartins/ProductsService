using ProductsEntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace Products
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da classe "Service" no arquivo de código, svc e configuração ao mesmo tempo.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ProductsService : IProductsService, IProductsServiceV2
    {
        public bool AddStock(string productCode, decimal quantity)
        {
            try
            {
                // Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel())
                {
                    if (!ProductExists(productCode, database))
                        return false;
                    else
                    {
                        // Find the ProductID for the specified product
                        int productID = (from p in database.Products
                                         where String.Compare(p.Code, productCode) == 0
                                         select p.Id).First();
                        // Find the Stock object that matches the parameters passed
                        // in to the operation
                        Stock stock = database.Stocks.First(pi => pi.Id == productID);
                        stock.Quantity = quantity;
                        stock.LastUpdate = DateTime.Now;
                        database.Stocks.Add(stock);
                        // Save the change back to the database
                        database.SaveChanges();
                    }
                }
            }
            catch
            {
                // If an exception occurs, return false to indicate failure
                return false;
            }
            // Return true to indicate success
            return true;
        }

        public int CurrentStock(string productCode)
        {
            int quantityTotal = 0;
            try
            {
                // Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel())
                {
                    if (ProductExists(productCode, database))
                    {
                        // Calculate the sum of all quantities for the specified product
                        quantityTotal = (from s in database.Stocks
                                         join p in database.Products
                                         on s.Product.Id equals p.Id
                                         where String.Compare(p.Code, productCode) == 0
                                         select (int)s.Quantity).Sum();
                    }
                }
            }
            catch
            {
                // Ignore exceptions in this implementation
            }
            // Return the stock level
            return quantityTotal;
        }

        public ProductData GetProduct(string productCode)
        {
            ProductData productData = null;
            try
            {// Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel())
                {
                    if (ProductExists(productCode, database))
                    {
                        // Find the first product that matches the specified product code
                        Product matchingProduct = database.Products.First(
                        p => String.Compare(p.Code, productCode) == 0);
                        productData = new ProductData()
                        {
                            Name = matchingProduct.Name,
                            Code = matchingProduct.Code,
                            Price = matchingProduct.Price
                        };
                    }
                }
            }
            catch
            {
                // Ignore exceptions in this implementation
            }
            // Return the product
            return productData;
        }

        public List<ProductData> ListProducts()
        {
            return ListMatchingProducts("");
        }

        public List<ProductData> ListMatchingProducts(string match)
        {
            // Create a list of products
            List<ProductData> productsList = new List<ProductData>();
            try
            {
                // Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel())
                {
                    // Fetch the products in the database
                    List<Product> products = (from product in database.Products
                                              where product.Name.Contains(match)
                                              select product).ToList();
                    foreach (Product product in products)
                    {
                        ProductData productData = new ProductData()
                        {
                            Name = product.Name,
                            Code = product.Code,
                            Price = product.Price
                        };
                        productsList.Add(productData);
                    }
                }
            }
            catch
            {
                // Ignore exceptions in this implementation
            }
            // Return the list of products
            return productsList;
        }

        public bool ProductExists(string productCode, ProductsModel database)
        {
            // Check to see whether the specified product exists in the database
            int numProducts = (from p in database.Products
                               where string.Equals(p.Code, productCode)
                               select p).Count();
            return numProducts > 0;
        }
    }
}

