global using System.Net.Http.Json;
global using BlazorEcommerce.Client.Services;
global using BlazorEcommerce.Client.Services.AddressService;
global using BlazorEcommerce.Client.Services.AdService;
global using BlazorEcommerce.Client.Services.AuthService;
global using BlazorEcommerce.Client.Services.CartService;
global using BlazorEcommerce.Client.Services.CategoryService;
global using BlazorEcommerce.Client.Services.MailService;
global using BlazorEcommerce.Client.Services.OrderService;
global using BlazorEcommerce.Client.Services.PrintingService;
global using BlazorEcommerce.Client.Services.ProductService;
global using BlazorEcommerce.Client.Services.ProductTypeService;
global using BlazorEcommerce.Client.Services.ToastService;
global using BlazorEcommerce.Shared;
global using Blazored.LocalStorage;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.JSInterop;
using BlazorEcommerce.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// LocalStorage
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// DI
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddSingleton<BrowserService>();
builder.Services.AddScoped<ToastService>();

// Authroriztaion
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Mailing
builder.Services.AddScoped<IMailService, MailService>();

// Printing
builder.Services.AddScoped<IPrintingService, PrintingService>();

await builder.Build().RunAsync();
