using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using courseworkDB.Services;
using Microsoft.AspNetCore.Components;
using courseworkDB.Components.Dialogs;
using MudBlazor;


namespace courseworkDB.Components.Pages
{
    public partial class Coffee
    {
        public const string Route = "/coffee";
        public string successMessage = string.Empty;
        

        [Inject]
        private CoffeeService coffeeService { get; set; }

        private void OpenEditCoffeePriceDialog(CoffeeService.CoffeeType coffeeType)
        {
            

            var parameters = new DialogParameters { ["CoffeeType"] = coffeeType };
            var options = new DialogOptions { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EditCoffeePrice>("UPDATE PRICE",parameters, options);
        }
    }
}
