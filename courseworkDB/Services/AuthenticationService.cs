using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseworkDB.Services
{
    public class AuthenticationService
    {

        private bool _isAuthenticated = false;
        private bool _isAuthenticatedAdmin = false;
        private string _authenticatedUser = string.Empty;
        private string _errorMessage = string.Empty;

        public bool IsAuthenticated => _isAuthenticated;

        public bool IsAuthenticatedAdmin => _isAuthenticatedAdmin;
        public string AuthenticatedUser => _authenticatedUser;
        public string ErrorMessage => _errorMessage;

        public async Task<bool> Authenticate(string password)
        {
            
            bool isValid = await SecurePasswordValidation(password);
            

            if (isValid)
            {
                _isAuthenticated = true;

                // Determine the role based on the password
                if (password == "admin@123")
                {
                    _authenticatedUser = "Admin";
                }
                else if (password == "staff@123")
                {
                    _authenticatedUser = "Staff";
                }
                else
                {
                    _errorMessage = "Invalid password!";
                    return false;
                }

                _errorMessage = string.Empty;
            }
            else
            {
                _errorMessage = "Invalid password!";
            }

            return isValid;
        }

        private async Task<bool> SecurePasswordValidation(string password)
        {
            
            return password == "staff@123" || password == "admin@123"; 
        }

        public async Task<bool> AuthenticateAdmin(string password)
        {

            bool isValidAdmin = await SecureEditPricePasswordValidation(password);

            if (isValidAdmin)
            {
                _isAuthenticatedAdmin = true;
            }
            else
            {
                _errorMessage = "Invalid password!";
            }

            return isValidAdmin;
        }

        public void Logout()
        {
            _isAuthenticated = false;
            _isAuthenticatedAdmin = false;
            _authenticatedUser = string.Empty;
            _errorMessage = string.Empty;
        }

        

        private async Task<bool> SecureEditPricePasswordValidation(string password)
        {

            return password == "admin";
        }
    }
}
