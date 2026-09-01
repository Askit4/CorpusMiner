param tags object
param resourceToken string

@description('Nombre para mostrar del remitente (ej. CorpusMiner).')
param senderDisplayName string = 'CorpusMiner'

// Email Communication Service con dominio administrado por Azure (verificacion instantanea,
// sin pasos de DNS). Para usar un dominio propio (ej. askit4.com) mas adelante, se agrega un
// dominio 'CustomerManaged' adicional y se actualiza senderAddress.
resource emailService 'Microsoft.Communication/emailServices@2023-04-01' = {
  name: 'email-${resourceToken}'
  location: 'global'
  tags: tags
  properties: {
    dataLocation: 'United States'
  }
}

resource emailDomain 'Microsoft.Communication/emailServices/domains@2023-04-01' = {
  parent: emailService
  name: 'AzureManagedDomain'
  location: 'global'
  properties: {
    domainManagement: 'AzureManaged'
  }
}

resource senderUsername 'Microsoft.Communication/emailServices/domains/senderUsernames@2023-04-01' = {
  parent: emailDomain
  name: 'donotreply'
  properties: {
    username: 'donotreply'
    displayName: senderDisplayName
  }
}

resource communicationService 'Microsoft.Communication/communicationServices@2023-04-01' = {
  name: 'acs-${resourceToken}'
  location: 'global'
  tags: tags
  properties: {
    dataLocation: 'United States'
    linkedDomains: [
      emailDomain.id
    ]
  }
}

@secure()
output connectionString string = communicationService.listKeys().primaryConnectionString
output senderAddress string = 'donotreply@${emailDomain.properties.mailFromSenderDomain}'
