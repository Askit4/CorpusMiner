targetScope = 'resourceGroup'

@description('Nombre corto del entorno (ej. dev, temp-<id>, prod). Se usa para nombrar recursos de forma unica.')
param environmentName string

@description('Region de Azure para todos los recursos.')
param location string = resourceGroup().location

@description('Region para el App Service Plan/Web App. Por defecto igual a location; se puede sobreescribir si la region principal no tiene capacidad disponible para el SKU elegido.')
param appServiceLocation string = location

@description('SKU del App Service Plan. Ver modules/app-service.bicep.')
param appServicePlanSkuName string = 'B1'
param appServicePlanSkuTier string = 'Basic'

@description('Login administrador de Azure SQL.')
param sqlAdministratorLogin string

@secure()
@description('Password del administrador de Azure SQL. Nunca se pasa via parameters.json; se suministra en el momento del despliegue.')
param sqlAdministratorLoginPassword string

var resourceToken = uniqueString(resourceGroup().id, environmentName)
var tags = {
  solution: 'corpusminer'
  environment: environmentName
}

module appInsights 'modules/app-insights.bicep' = {
  name: 'app-insights'
  params: {
    location: location
    tags: tags
    resourceToken: resourceToken
  }
}

module keyVault 'modules/key-vault.bicep' = {
  name: 'key-vault'
  params: {
    location: location
    tags: tags
    resourceToken: resourceToken
  }
}

module storage 'modules/storage.bicep' = {
  name: 'storage'
  params: {
    location: location
    tags: tags
    resourceToken: resourceToken
  }
}

module communication 'modules/communication.bicep' = {
  name: 'communication'
  params: {
    tags: tags
    resourceToken: resourceToken
  }
}

module sql 'modules/sql.bicep' = {
  name: 'sql'
  params: {
    location: location
    tags: tags
    resourceToken: resourceToken
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorLoginPassword
  }
}

module appService 'modules/app-service.bicep' = {
  name: 'app-service'
  params: {
    location: appServiceLocation
    tags: tags
    resourceToken: resourceToken
    keyVaultName: keyVault.outputs.name
    appInsightsConnectionString: appInsights.outputs.connectionString
    acsSenderAddress: communication.outputs.senderAddress
    skuName: appServicePlanSkuName
    skuTier: appServicePlanSkuTier
  }
}

// Nombre calculado con la misma formula que modules/key-vault.bicep, para poder
// referenciar el recurso "existing" con un valor resoluble al inicio del despliegue
// (los outputs de modulos no sirven aqui: ver BCP120).
resource keyVaultRef 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: 'kv-${resourceToken}'
}

// Mismo motivo que keyVaultRef: se necesita listKeys() sobre el recurso real,
// que un output de modulo no puede exponer de forma segura.
resource storageAccountRef 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: 'st${resourceToken}'
}

resource sqlConnectionSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVaultRef
  name: 'sql-connection-string'
  properties: {
    value: 'Server=tcp:${sql.outputs.fullyQualifiedDomainName},1433;Database=${sql.outputs.databaseName};User ID=${sqlAdministratorLogin};Password=${sqlAdministratorLoginPassword};Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;'
  }
}

resource acsConnectionSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVaultRef
  name: 'acs-connection-string'
  properties: {
    value: communication.outputs.connectionString
  }
}

resource storageConnectionSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVaultRef
  name: 'storage-connection-string'
  properties: {
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountRef.name};AccountKey=${storageAccountRef.listKeys().keys[0].value};EndpointSuffix=core.windows.net'
  }
}

resource keyVaultSecretsUserRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVaultRef.id, 'app-service-${resourceToken}', 'KeyVaultSecretsUser')
  scope: keyVaultRef
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '4633458b-17de-408a-b874-0445c86b69e6'
    )
    principalId: appService.outputs.principalId
    principalType: 'ServicePrincipal'
  }
}

output webAppUrl string = 'https://${appService.outputs.defaultHostName}'
output sqlServerFqdn string = sql.outputs.fullyQualifiedDomainName
output storageAccountName string = storage.outputs.name
output keyVaultName string = keyVault.outputs.name
