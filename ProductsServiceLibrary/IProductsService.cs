﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Security;

namespace Products
{
    // Service contract describing the operations provided by the WCF service
    [ServiceContract(Namespace = "http://localhost/productsService/01", Name = "IProductsService")]
    public interface IProductsService
    {
        // Get all products
        [OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        List<ProductData> ListProducts();

        // Get the details of a single product
        [OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        ProductData GetProduct(string productCode);

        // Get the current stock for a product
        [OperationContract(ProtectionLevel = ProtectionLevel.Sign)]
        int CurrentStock(string productCode);

        // Add stock for a product
        [OperationContract(ProtectionLevel = ProtectionLevel.Sign)]
        bool AddStock(string productCode, decimal quantity);
    }

    // Service contract describing the operations provided by the WCF service
    [ServiceContract(Namespace = "http://localhost/productsService/02", Name = "IProductsService")]
    public interface IProductsServiceV2
    {
        // Get the product numbers of matching products
        [OperationContract]
        List<ProductData> ListMatchingProducts(string match);

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
        [DataMember(Order = 0)]
        public string Name;
        [DataMember(Order = 1)]
        public string Code;
        [DataMember(Order = 2)]
        public decimal Price;
    }
}