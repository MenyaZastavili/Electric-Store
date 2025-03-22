using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.ApplicationData
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsSelected {  get; set; } = false;

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value > StockQuantity ? StockQuantity : value;
            }
        }
    }
}
