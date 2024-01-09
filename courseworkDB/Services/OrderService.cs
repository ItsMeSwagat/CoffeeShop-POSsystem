using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace courseworkDB.Services
{
    public class OrderService
    {
        public class Order
        {
            public Guid OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public string CustomerUsername { get; set; }
            public List<CoffeeService.CartItem> Items { get; set; }
            public decimal TotalPrice { get; set; }
        }

        private const string OrderDataFilePath = "D:\\AD coursework\\courseworkDB\\courseworkDB\\Models\\orderData.json";

        public List<Order> Orders { get; private set; } = new List<Order>();

        public void ProcessOrder(List<CoffeeService.CartItem> cartItems, string customerUsername)
        {
            if (cartItems.Any())
            {
                var orderId = Guid.NewGuid();
                var order = new Order
                {
                    OrderId = orderId,
                    OrderDate = DateTime.Now,
                    CustomerUsername = customerUsername,
                    
                    Items = new List<CoffeeService.CartItem>(cartItems),
                    TotalPrice = cartItems.Sum(item => item.TotalPrice)
                };

                Orders.Add(order);

                // You may want to perform additional processing here (e.g., sending order details to the kitchen, updating inventory, etc.)

                SaveData();
            }
            else
            {
                // Handle an empty cart
            }
        }

        public void SaveData()
        {
            string jsonData = JsonSerializer.Serialize(Orders);
            File.WriteAllText(OrderDataFilePath, jsonData);
        }

        public void LoadData()
        {
            if (File.Exists(OrderDataFilePath))
            {
                string jsonData = File.ReadAllText(OrderDataFilePath);
                Orders = JsonSerializer.Deserialize<List<Order>>(jsonData) ?? new List<Order>();
            }
        }
    }
}
