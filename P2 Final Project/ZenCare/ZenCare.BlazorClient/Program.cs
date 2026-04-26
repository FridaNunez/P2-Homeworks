using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZenCare.BlazorClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Usar HTTP para evitar problemas de certificado en desarrollo
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5207/")
});

await builder.Build().RunAsync();