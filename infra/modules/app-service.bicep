param location string
param tags object
param resourceToken string
param keyVaultName string
param appInsightsConnectionString string
param acsSenderAddress string

@description('SKU del App Service Plan. Default Basic B1; se puede bajar a Free F1 si la cuota de la suscripcion no alcanza para VMs dedicadas.')
param skuName string = 'B1'
param skuTier string = 'Basic'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'plan-${resourceToken}'
  location: location
  tags: tags
  sku: {
    name: skuName
    tier: skuTier
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: 'app-${resourceToken}'
  location: location
  tags: tags
  kind: 'app,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      appSettings: [
        {
          name: 'ConnectionStrings__DefaultConnection'
          value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=sql-connection-string)'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'Acs__ConnectionString'
          value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=acs-connection-string)'
        }
        {
          name: 'Acs__SenderAddress'
          value: acsSenderAddress
        }
      ]
    }
  }
}

output name string = webApp.name
output defaultHostName string = webApp.properties.defaultHostName
output principalId string = webApp.identity.principalId
