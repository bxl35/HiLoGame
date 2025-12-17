using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HiloGame.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);



builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var baseUrl = builder.Configuration.GetValue<string>("ApplicationSettings:BaseApiUrlHttps");
if (string.IsNullOrEmpty(baseUrl))
{
    baseUrl = "https://localhost:5001";
}

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl!) });

await builder.Build().RunAsync();
