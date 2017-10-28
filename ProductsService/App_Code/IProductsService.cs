using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Products
{
    // Service contract describing the operations provided by the WCF service
    [ServiceContract]
    public interface IProductsService
    {
        // Get all products
        [OperationContract]
        List<ProductData> ListProducts();
        // Get the details of a single product
        [OperationContract]
        ProductData GetProduct(string productCode);
        // Get the current stock for a product
        [OperationContract]
        int CurrentStock(string productCode);
        // Add stock for a product
        [OperationContract]
        bool AddStock(string productCode, decimal quantity);
    }

    // Data contract describing the details of a product passed to client applications
    [DataContract]
    public class ProductData
    {
        [DataMember]
        public string Name;
        [DataMember]
        public string Code;
        [DataMember]
        public decimal Price;
    }
}