using Maktab.Consumer;
using Maktab.Consumer.Services;
using Maktab.Core.Interfaces.Services;
using Maktab.Domain.Services;
using Maktab.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//var baseUri = builder.Configuration["apiUrl"]; //builder.HostEnvironment.BaseAddress

builder.Services//.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUri) })
                .AddScoped<ISessionService, SessionService>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAddressService, AddressService>()
                .AddScoped<IChildrenService, ChildrenService>()
                .AddScoped<IOtherContactService, OtherContactService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddSingleton<ISystemService, SystemService>();
builder.Services.AddMudServices();

//builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(x => {
     var apiUrl = new Uri("https://maktab.azurewebsites.net/");
     //new Uri(builder.HostEnvironment.BaseAddress);
     //new Uri(builder.Configuration["apiUrl"]);

     // use fake backend if "fakeBackend" is "true" in appsettings.json
     //if (builder.Configuration["fakeBackend"] == "true")
     //{
     //     var fakeBackendHandler = new FakeBackendHandler(x.GetService<ILocalStorageService>());
     //     return new HttpClient(fakeBackendHandler) { BaseAddress = apiUrl };
     //}

     return new HttpClient() { BaseAddress = apiUrl };
});



await builder.Build().RunAsync();
