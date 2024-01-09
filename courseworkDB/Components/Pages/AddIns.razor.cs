using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using courseworkDB.Components.Dialogs;
using MudBlazor;
using courseworkDB.Services;

namespace courseworkDB.Components.Pages
{
    public partial class AddIns
    {
        public const string Route = "/addins";
        public string successMessage = string.Empty;

        [Inject]
        private CoffeeService coffeeService { get; set; }


        private void OpenEditAddInPriceDialog(CoffeeService.AddIn addIn)
        {

            var parameters = new DialogParameters { ["AddIn"] = addIn };
            var options = new DialogOptions { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EditAddInsPriceDialog>("UPDATE PRICE", parameters, options);
        }
    }
}
