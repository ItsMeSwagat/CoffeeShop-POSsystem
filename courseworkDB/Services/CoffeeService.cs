using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static MudBlazor.CategoryTypes;

namespace courseworkDB.Services
{
    /* public class CoffeeService
     {
         public decimal GrandTotal { get; private set; } = 0;
         public class CoffeeType
         {
             public string Name { get; set; }
             public decimal Price { get; set; }
             public string ImageUrl { get; set; }
             public int Quantity { get; set; }

             public CoffeeType(string name, decimal price, string imageUrl, int quantity)
             {
                 Name = name;
                 Price = price;
                 ImageUrl = imageUrl;
                 Quantity = quantity;
             }
         }

         public class AddIn
         {
             public string Name { get; set; }

             public decimal Price { get; set; }

             public bool Selected { get; set; }

             public AddIn(string name, decimal price, bool selected)
             {
                 Name = name;
                 Price = price;
                 Selected = selected;
             }

         }


         public List<CoffeeType> CoffeeTypes { get; private set; }
         public List<AddIn> AddIns { get; private set; }
         public List< (CoffeeType,List<AddIn>, int, decimal, decimal, Guid)> Cart { get; private set; } = new List<(CoffeeType, List<AddIn>, int, decimal, decimal, Guid)>();

         public CoffeeService()
         {
             // Initialize coffee types
             CoffeeTypes = new List<CoffeeType>
             {
                 new CoffeeType("Black Coffee", 150, "Images/black.jpg",0),
                 new CoffeeType("Americano Coffee", 550, "Images/americano.jpg",0),
                 new CoffeeType("Cappuccino Coffee", 550, "Images/cappuccino.jpg", 0),
                 new CoffeeType("Cappuccino Coffee", 550, "Images/cappuccino.jpg", 0),
                 // Add more coffee types as needed
             };

             // Initialize add-ins
             AddIns = new List<AddIn>
             {
                 new AddIn("Cinnamon", 50, false),
                 new AddIn("Honey", 100, false),
                 new AddIn("Ginger", 60, false),
                 // Add more add-ins as needed
             };
         }




         public void AddToCart(CoffeeType coffeeType, List<AddIn> selectedAddIns, int quantity)
         {
             var CartId = Guid.NewGuid();
             var totalAddInsPrice = selectedAddIns.Sum(addIn => addIn.Price);

             var totalCoffeePrice = coffeeType.Price * quantity;
             var totalPrice = totalCoffeePrice + totalAddInsPrice;
             GrandTotal += totalPrice; 

             Cart.Add((coffeeType, selectedAddIns, coffeeType.Quantity, totalPrice, GrandTotal, CartId));
         }

         public void RemoveItemFromCart(Guid CartId)
         {
             var cartItem = Cart.FirstOrDefault(item => item.Item6 == CartId);
             if (cartItem != default)
             {
                 Cart.Remove(cartItem);
                 GrandTotal -= cartItem.Item4; 
             }
             else
             {
                 // Handle item not found
             }
         }

         public void EditCoffeePrice(CoffeeType coffeeType, decimal newPrice)
         {

             // Find the coffee type in the CoffeeTypes list
             var coffeeTypeIndex = CoffeeTypes.FindIndex(ct => ct.Name == coffeeType.Name);
             if (coffeeTypeIndex != -1)
             {
                 // Update the price
                 CoffeeTypes[coffeeTypeIndex].Price = newPrice;

                 // Recalculate cart totals for affected items
                 foreach (var cartItem in Cart)
                 {
                     var modifiableCartItem = cartItem;
                     if (cartItem.Item1.Name == coffeeType.Name)
                     {
                         modifiableCartItem.Item4 = (cartItem.Item1.Price * cartItem.Item3) + cartItem.Item2.Sum(addIn => addIn.Price);
                         GrandTotal += cartItem.Item4; // Accumulate total price changes
                     }
                 }
             }
             else
             {
                 // Handle coffee type not found
             }
         }

         public void EditAddInPrice(AddIn addIn, decimal newPrice)
         {
             // Find the add-in in the AddIns list
             var addInIndex = AddIns.FindIndex(ai => ai.Name == addIn.Name);
             if (addInIndex != -1)
             {
                 // Update the price
                 AddIns[addInIndex].Price = newPrice;

                 // Recalculate cart totals for affected items
                 foreach (var cartItem in Cart)
                 {
                     var modifiableCartItem = cartItem;
                     var updatedTotalPrice = cartItem.Item1.Price * cartItem.Item3;
                     foreach (var addInInCart in cartItem.Item2)
                     {
                         if (addInInCart.Name == addIn.Name)
                         {
                             updatedTotalPrice += newPrice; // Use the updated price
                         }
                         else
                         {
                             updatedTotalPrice += addInInCart.Price;
                         }
                     }

                     modifiableCartItem.Item4 = updatedTotalPrice;
                     GrandTotal += cartItem.Item4 - cartItem.Item5; // Account for previous total
                 }

             }
             else
             {
                 // Handle add-in not found
             }
         }


     } */

    //new
    
    public class CoffeeService
    {
        public decimal GrandTotal { get; set; } = 0;

        public class CoffeeType
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
            public int Quantity { get; set; }

            public CoffeeType(string name, decimal price, string imageUrl, int quantity)
            {
                Name = name;
                Price = price;
                ImageUrl = imageUrl;
                Quantity = quantity;
            }
        }

        public class AddIn
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public bool Selected { get; set; }

            public AddIn(string name, decimal price, bool selected)
            {
                Name = name;
                Price = price;
                Selected = selected;
            }
        }

        public class CartItem
        {
            public CoffeeType CoffeeType { get; set; }
            public List<AddIn> SelectedAddIns { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }

            public decimal GrandTotal { get; set; }
            public Guid CartId { get; set; }

            
        }

        public class SavedData
        {
            public List<CoffeeType> CoffeeTypes { get; set; }
            public List<AddIn> AddIns { get; set; }
        }

        public List<CoffeeType> CoffeeTypes { get; private set; }
        public List<AddIn> AddIns { get; private set; }
        public List<CartItem> Cart { get; private set; } = new List<CartItem>();

        private const string CoffeeDataFilePath = "D:\\AD coursework\\courseworkDB\\courseworkDB\\Models\\coffeeData.json";

        public CoffeeService()
        {
            LoadData();
        }

        public void LoadData()
        {
            InitializeDefaultData();
            SaveData();
            if (File.Exists(CoffeeDataFilePath))
            {
                string jsonData = File.ReadAllText(CoffeeDataFilePath);
                var savedData = JsonSerializer.Deserialize<SavedData>(jsonData);
                if (savedData != null)
                {
                    CoffeeTypes = savedData.CoffeeTypes ?? new List<CoffeeType>();
                    AddIns = savedData.AddIns ?? new List<AddIn>();
                }
            }
        }

        internal void SaveData()
        {
            SavedData savedData = new SavedData
            {
                CoffeeTypes = CoffeeTypes,
                AddIns = AddIns
            };

            string jsonData = JsonSerializer.Serialize(savedData);
            File.WriteAllText(CoffeeDataFilePath, jsonData);
        }

        public void AddToCart(CoffeeType coffeeType, List<AddIn> selectedAddIns, int quantity)
        {
            var CartId = Guid.NewGuid();
            var totalAddInsPrice = selectedAddIns.Sum(addIn => addIn.Price);

            var totalCoffeePrice = coffeeType.Price * quantity;
            var totalPrice = totalCoffeePrice + totalAddInsPrice;
            GrandTotal += totalPrice;

            Cart.Add(new CartItem
            {
                CoffeeType = coffeeType,
                SelectedAddIns = selectedAddIns,
                Quantity = quantity,
                TotalPrice = totalPrice,
                CartId = CartId,
                GrandTotal = GrandTotal
            });
        }

        public void RemoveItemFromCart(Guid CartId)
        {
            var cartItem = Cart.FirstOrDefault(item => item.CartId == CartId);
            if (cartItem != default)
            {
                Cart.Remove(cartItem);
                GrandTotal -= cartItem.TotalPrice;
            }
            else
            {
                // Handle item not found
            }
        }

        public void ResetCart()
        {
            // Clear the cart
            Cart.Clear();
            GrandTotal = 0;
            SaveData();
        }

        public void EditCoffeePrice(CoffeeType coffeeType, decimal newPrice)
        {
            var coffeeToUpdate = CoffeeTypes.FirstOrDefault(ct => ct.Name == coffeeType.Name);
            if (coffeeToUpdate != null)
            {
                coffeeToUpdate.Price = newPrice;
                
                SaveData();
            }
        }

        public void EditAddInPrice(AddIn addIn, decimal newPrice)
        {
            var addInToUpdate = AddIns.FirstOrDefault(ai => ai.Name == addIn.Name);
            if (addInToUpdate != null)
            {
                addInToUpdate.Price = newPrice;
        
                SaveData();
            }
        }


        private void InitializeDefaultData()
        {
            // Initialize coffee types
            CoffeeTypes = new List<CoffeeType>
            {
                new CoffeeType("Black Coffee", 150, "Images/black.jpg", 1),
                new CoffeeType("Americano Coffee", 350, "Images/americano.jpg", 1),
                new CoffeeType("Cappuccino Coffee", 450, "Images/cappuccino.jpg", 1),
                new CoffeeType("Cortadito Coffee", 550, "Images/Cortadito.jpg", 1),
                new CoffeeType("Expresso Coffee", 650, "Images/Expresso.png", 1),
                // Add more coffee types as needed
            };

            // Initialize add-ins
            AddIns = new List<AddIn>
            {
                new AddIn("Cinnamon", 50, false),
                new AddIn("Honey", 100, false),
                new AddIn("Ginger", 60, false),
                new AddIn("Ice Cream", 150, false),
                // Add more add-ins as needed
            };
        }
    }
}
