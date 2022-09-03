# Playing with Azure Key Vault

In this repository you can found how to access to Azure Key Vault from diferent resources.

## FromAppService

In this project we have an Asp.Net Core Web Api (.net 6 - minimal api) hosted in an Azure App Service.

1. Authentication

In the Azure App Service it´s been activated the system-assigned managed identity so the service can access to Azure Key Vault without the need of store any credential anywhere. That´s a good practice in terms of security.

In order of our solution can use the system-assigned managed identity of the service where it´s hosted, we have to install the following package from Nuget, Azure.Identity*.  So, right click on _Dependencies/Managed Nuget Packages_ option, browse tab and install Azure.Identity package. You can use the Package Manager Console too: 

```
dotnet add package Azure.Identity
```

Because of we are hosting our web app on Azure App Service and we have enabled the system-assigned managed identity it´s possible to use the most simple authentication form: the `DefaultAzureCredential`

```csharp
var credential = new DefaultAzureCredential();
```


2. Accesing Key Vault

For accessing to Azure Key Vault secrets we have to install the next package `Azure.Security.KeyVault.Secrets`. As before, we can use the _Managed Nuget Package_ option or the _Package Manager Console_, that´s up to you.

```
dotnet add package Azure.Security.KeyVault.Secrets
```

Once the package is installed, we only have to create a secrets client to connect to the vault indicating the vault URL and the credential created in the step 1. With the secrets client created we only has to get the secret that we want by his name.

```csharp
var kvSecretClient = new SecretClient(
    vaultUri: new Uri("https://jcdemokeyvault.vault.azure.net/"),
    credential: credential);

KeyVaultSecret secret = kvSecretClient.GetSecret("secret-name");

return secret.Value;
```


You can found more information of the services that´s been used:
* Azure.Identity
[MicrosoftDocs] (https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme).
[Github]https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md

* Key Vault Secrets
[Github] (https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/keyvault/Azure.Security.KeyVault.Secrets)