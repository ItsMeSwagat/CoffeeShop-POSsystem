using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace courseworkDB.Services
{
    public class MembershipService
    {
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
                // Update existing membership
                existingMembership.PurchaseCount++;
                existingMembership.LastPurchaseDate = DateTime.Now;

                if (existingMembership.PurchaseCount % 10 == 0)
                {
                    // Customer is eligible for a complimentary coffee after every 10 purchases
                    // Implement logic to redeem a complimentary coffee
                    RedeemComplimentaryCoffee(existingMembership.CustomerUsername);
                }

                if (IsRegularMember(existingMembership))
                {
                    // Customer is a regular member, provide a flat 10% discount for the entire month
                    // Implement logic to apply the discount (e.g., update the cart)
                    ApplyRegularMemberDiscount(existingMembership.CustomerUsername);
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

        private void ApplyRegularMemberDiscount(string customerUsername)
        {
            // Implement logic to apply a flat 10% discount for the entire month
            // You can update the cart or perform any other actions as needed
            var coffeeService = new CoffeeService(); // You might want to inject this instead of creating a new instance
            foreach (var cartItem in coffeeService.Cart)
            {
                cartItem.GrandTotal = decimal.Round(cartItem.TotalPrice * 0.9m, 2); // Apply a 10% discount
                
            }
            coffeeService.SaveData();
        }

        private void RedeemComplimentaryCoffee(string customerUsername)
        {
            // logic to redeem a complimentary coffee after every 10 purchases
            var coffeeService = new CoffeeService(); // You might want to inject this instead of creating a new instance
            var complimentaryCoffee = coffeeService.CoffeeTypes.FirstOrDefault(c => c.Name == "Complimentary Coffee");

            if (complimentaryCoffee != null)
            {
                // Add the complimentary coffee to the cart
                coffeeService.AddToCart(complimentaryCoffee, new List<CoffeeService.AddIn>(), 1);
            }
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
