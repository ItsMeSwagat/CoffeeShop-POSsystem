using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using courseworkDB.Models;
using courseworkDB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;



namespace courseworkDB.Components.Pages
{
    public partial class Home
    {
        public const string Route = "/dashboard";
        public string successMessage = string.Empty;

        
        

        [Inject]
        private CoffeeService coffeeService { get; set; }

        
        List<CoffeeService.AddIn> selectedAddIns = new List<CoffeeService.AddIn>();


        private void AddToCart(CoffeeService.CoffeeType coffeeType, List<CoffeeService.AddIn> selectedAddIns)
        {
            this.selectedAddIns = coffeeService.AddIns.Where(addIn => addIn.Selected).ToList();
            coffeeService.AddToCart(coffeeType, this.selectedAddIns, coffeeType.Quantity);
            successMessage = "Coffee Added Successfully!!";
     
        }

        private void RemoveItem(Guid CartId)
        {
            coffeeService.RemoveItemFromCart(CartId);
            // Refresh the cart display here
        }


    }
}
