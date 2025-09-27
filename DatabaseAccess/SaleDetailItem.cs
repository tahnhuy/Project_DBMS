using System;
using System.Collections.Generic;

namespace Sale_Management.DatabaseAccess
{
    public class SaleDetailItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }

        public SaleDetailItem(int productId, int quantity, decimal salePrice, string productName = "", string unit = "")
        {
            ProductId = productId;
            Quantity = quantity;
            SalePrice = salePrice;
            ProductName = productName;
            Unit = unit;
        }
    }
}
