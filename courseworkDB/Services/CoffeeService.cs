using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static MudBlazor.CategoryTypes;

namespace courseworkDB.Services
{
    
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
            public List<CartItem> Cart { get; set; }
            public decimal GrandTotal { get; set; }
        }

        public List<CoffeeType> CoffeeTypes { get; private set; }
        public List<AddIn> AddIns { get; private set; }
        public List<CartItem> Cart { get; private set; } = new List<CartItem>();

        private const string CoffeeDataFilePath = "D:\\AD coursework\\courseworkDB\\courseworkDB\\Models\\coffeeData.json";

        public CoffeeService()
        {
            LoadData();
        }

        public void LoadData() //loads the data from json file
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
                    Cart = savedData.Cart ?? new List<CartItem>();
                    GrandTotal = savedData.GrandTotal;
                }
            }
        }

        internal void SaveData() //saves the data to json file.
        {
            SavedData savedData = new SavedData
            {
                CoffeeTypes = CoffeeTypes,
                AddIns = AddIns,
                Cart = Cart,
                GrandTotal = GrandTotal
            };

            string jsonData = JsonSerializer.Serialize(savedData);
            File.WriteAllText(CoffeeDataFilePath, jsonData);
        }

        public void AddToCart(CoffeeType coffeeType, List<AddIn> selectedAddIns, int quantity) // adds the item to cart
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
            SaveData();
        }

        public void RemoveItemFromCart(Guid CartId) //removes items from cart
        {
            var cartItem = Cart.FirstOrDefault(item => item.CartId == CartId);
            if (cartItem != default)
            {
                Cart.Remove(cartItem);
                GrandTotal -= cartItem.TotalPrice;
            }
            else
            {
                
            }
            SaveData();
        }

        public void ResetCart() 
        {
            // Clear the cart
            Cart.Clear();
            GrandTotal = 0;
            SaveData();
        }

        public void EditCoffeePrice(CoffeeType coffeeType, decimal newPrice) //Edits the coffee price and saves it to json file
        {
            var coffeeToUpdate = CoffeeTypes.FirstOrDefault(ct => ct.Name == coffeeType.Name);
            if (coffeeToUpdate != null)
            {
                coffeeToUpdate.Price = newPrice;
                
                SaveData();
            }
        }

        public void EditAddInPrice(AddIn addIn, decimal newPrice) //edits the addin price.
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
            // Initialized coffee types
            CoffeeTypes = new List<CoffeeType>
            {
                new CoffeeType("Black Coffee", 150, "Images/black.jpg", 1),
                new CoffeeType("Americano Coffee", 350, "Images/americano.jpg", 1),
                new CoffeeType("Cappuccino Coffee", 450, "Images/cappuccino.jpg", 1),
                new CoffeeType("Cortadito Coffee", 550, "Images/Cortadito.jpg", 1),
                new CoffeeType("Expresso Coffee", 650, "Images/Expresso.png", 1),
                
            };

            // Initialized add-ins
            AddIns = new List<AddIn>
            {
                new AddIn("Cinnamon", 50, false),
                new AddIn("Honey", 100, false),
                new AddIn("Ginger", 60, false),
                new AddIn("Ice Cream", 150, false),
                
            };
        }
    }
}
