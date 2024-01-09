using courseworkDB.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;


namespace courseworkDB
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif      
            
            builder.Services.AddScoped<AuthenticationService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddSingleton<CoffeeService>(sp =>
            {
                var coffeeService = new CoffeeService();
                coffeeService.LoadData();  // Load data when the service is instantiated
                return coffeeService;
            });
            builder.Services.AddSingleton<MembershipService>(sp =>
            {
                var membershipService = new MembershipService();
                membershipService.LoadData();  // Load data when the service is instantiated
                return membershipService;
            });
            builder.Services.AddSingleton<OrderService>(sp =>
            {
                var coffeeService = new CoffeeService();
                var orderService = new OrderService(coffeeService);
                orderService.LoadData(); // Load data when the service is instantiated
                return orderService;
            });
            
            builder.Services.AddMudServices();
            return builder.Build();
        }
    }
}
