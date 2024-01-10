using System.Text.Json;


namespace courseworkDB.Services
{
    public class OrderService
    {
        private readonly CoffeeService coffeeService;

        public OrderService(CoffeeService coffeeService)
        {
            this.coffeeService = coffeeService ?? throw new ArgumentNullException(nameof(coffeeService));
        }

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



                SaveData();
            }
            else
            {

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
