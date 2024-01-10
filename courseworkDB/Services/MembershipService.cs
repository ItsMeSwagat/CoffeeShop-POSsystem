using System.Text.Json;



namespace courseworkDB.Services
{
    public class MembershipService
    {
        private CoffeeService _coffeeService;

        public MembershipService(CoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }
        public class Membership
        {
            public string CustomerUsername { get; set; }
            public int PurchaseCount { get; set; }
            public bool IsRegularMember { get; set; }
            public DateTime LastPurchaseDate { get; set; }
        }

        private const string MembershipDataFilePath = "D:\\AD coursework\\courseworkDB\\courseworkDB\\Models\\membershipData.json";

        public List<Membership> Memberships { get; private set; } = new List<Membership>();

        public void CreateMembership(string customerUsername)
        {
            var existingMembership = Memberships.FirstOrDefault(m => m.CustomerUsername == customerUsername);

            if (existingMembership != null)
            {

                existingMembership.PurchaseCount++;
                existingMembership.LastPurchaseDate = DateTime.Now;

                if (existingMembership.PurchaseCount % 10 == 0)
                {
                    // Customer is eligible for a complimentary coffee after every 10 purchases

                    RedeemComplimentaryCoffee();
                }

                if (IsRegularMember(existingMembership))
                {
                    // Customer is a regular member, provide a flat 10% discount for the entire month

                    ApplyRegularMemberDiscount();
                }
            }
            else
            {
                // Create a new membership
                var newMembership = new Membership
                {
                    CustomerUsername = customerUsername,
                    PurchaseCount = 1,
                    LastPurchaseDate = DateTime.Now
                };

                Memberships.Add(newMembership);
            }

            SaveData();
        }

        public bool IsRegularMember(Membership membership)
        {
            // Check if the customer is a regular member based on the defined criteria
            // For simplicity, let's assume a customer is a regular member if they make daily purchases for a full month (excluding weekends)
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;

            if (membership.LastPurchaseDate >= startDate && membership.LastPurchaseDate <= endDate)
            {
                // Check if the customer made purchases every day (excluding weekends)
                var currentDate = startDate;
                while (currentDate <= endDate)
                {
                    if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (Memberships.Any(m => m.CustomerUsername == membership.CustomerUsername && m.LastPurchaseDate.Date == currentDate.Date))
                        {
                            currentDate = currentDate.AddDays(1);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        currentDate = currentDate.AddDays(1);
                    }
                }

                return true;
            }

            return false;
        }

        public void ApplyRegularMemberDiscount()
        {
            
            foreach (var cartItem in _coffeeService.Cart)
            {
                cartItem.TotalPrice = decimal.Round(cartItem.TotalPrice * 0.9m, 2); // Apply a 10% discount

            }

            
            _coffeeService.GrandTotal = _coffeeService.Cart.Sum(cartItem => cartItem.TotalPrice);

            _coffeeService.SaveData();


        }

        public void RedeemComplimentaryCoffee()
        {
            var complimentaryCoffee = _coffeeService.CoffeeTypes.FirstOrDefault(c => c.Name == "Black Coffee");
            
            if (complimentaryCoffee != null)
            {
                // Add the complimentary coffee to the cart
                _coffeeService.AddToCart(complimentaryCoffee, new List<CoffeeService.AddIn>(), 1);

                // Reset the TotalPrice of the complimentary coffee in the cart to 0
                var cartItem = _coffeeService.Cart.FirstOrDefault(item => item.CoffeeType.Name == "Black Coffee");
                if (cartItem != null)
                {
                    cartItem.TotalPrice = 0;
                    _coffeeService.GrandTotal = _coffeeService.Cart.Sum(cartItem => cartItem.TotalPrice);
                }
            }
            _coffeeService.SaveData();
        }

        public void SaveData()
        {
            string jsonData = JsonSerializer.Serialize(Memberships);
            File.WriteAllText(MembershipDataFilePath, jsonData);
        }

        public void LoadData()
        {
            if (File.Exists(MembershipDataFilePath))
            {
                string jsonData = File.ReadAllText(MembershipDataFilePath);
                Memberships = JsonSerializer.Deserialize<List<Membership>>(jsonData) ?? new List<Membership>();
            }
        }
    }
}
