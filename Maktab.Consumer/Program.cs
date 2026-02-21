using Maktab.Consumer;
using Maktab.Consumer.Helpers;
using Maktab.Consumer.Services;
using Maktab.Consumer.State;
using Maktab.Core.Interfaces.Services;
using Maktab.Domain.Services;
using Maktab.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudBlazor.Translations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add MudBlazor
builder.Services.AddMudServices();

//var baseUri = builder.Configuration["apiUrl"]; //builder.HostEnvironment.BaseAddress

builder.Services.AddAuthorizationCore();
builder.Services.AddMudTranslations();
builder.Services.AddLocalization();// options => options.ResourcesPath = "Properties");

builder.Services//.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUri) })
                .AddScoped<ISystemService, SystemService>()
                .AddScoped<ISessionService, SessionService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAddressService, AddressService>()
                .AddScoped<IChildrenService, ChildrenService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<IRoleMenuService, RoleMenuService>()
                .AddScoped<IInstitutionService, InstitutionService>()
                .AddScoped<ICourseService, CourseService>()
                .AddScoped<ICourseEnrollmentService, CourseEnrollmentService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IPaymentService, PaymentService>()
                .AddScoped<ICourseEnrollmentTransactionService, CourseEnrollmentTransaction>()
                .AddScoped<ThemeService>()
                .AddScoped<AuthenticationStateProvider, AuthStateProvider>()
                .AddSingleton<UserStateService>()
                .AddSingleton<ISystemService, SystemService>()
                .AddScoped<IGlobalizationService,GlobalizationService>()
                .AddSingleton<IClipboardService, ClipboardService>();

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


var host = builder.Build();

var globalizationService = host.Services.GetRequiredService<IGlobalizationService>();
var culture = await globalizationService.GetPersistedCultureName();
globalizationService.ApplyCultureOnUI(culture);

await host.RunAsync();
