using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/keyVaultSecretWithSystemAssignedManagedIdentity", () =>
{
    //En este caso en nuestro Azure App Service tenemos habilitado la system-assigned managed identity y no
    //tenemos ninguna user-assigned managed identity asociado al App Service.
    var credential = new DefaultAzureCredential(options: new DefaultAzureCredentialOptions
    {
        ExcludeAzureCliCredential = true,
        ExcludeAzurePowerShellCredential = true,
        ExcludeEnvironmentCredential = true,
        ExcludeInteractiveBrowserCredential = true,
        ExcludeSharedTokenCacheCredential = true,
        ExcludeVisualStudioCodeCredential = true,
        ExcludeVisualStudioCredential = true
        //only allow managed identity
    });

    //Con esta credential ya debemos poder acceder a KeyVault
    var kvSecretClient = new SecretClient(
        vaultUri: new Uri("https://jcdemokeyvault.vault.azure.net/"),
        credential: credential);

    KeyVaultSecret secret = kvSecretClient.GetSecret("secret-name");

    return secret.Value;
})
.WithName("GetKeyVaultSecretWithSystemAssignedManagedIdentity");


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}