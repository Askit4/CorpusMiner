# Infraestructura (Bicep) — CorpusMiner

Desplegado en el resource group `CorpusMiner` (suscripción Askit4Ops, region `centralus`). Este RG sirve hoy como ambiente único de desarrollo/pruebas y, mientras el app no tenga uso productivo real, también como el ambiente "productivo".

## Contenido

- `main.bicep`: punto de entrada a nivel resource group. Orquesta los módulos y crea el secreto de conexión SQL en Key Vault más el role assignment que permite a la Web App leerlo.
- `main.parameters.json`: `environmentName=prod`, `location=centralus`, `sqlAdministratorLogin=cmsqladmin`. El password (`sqlAdministratorLoginPassword`) es `@secure()` y se pasa en el momento del despliegue (`--parameters sqlAdministratorLoginPassword=$SECRET`), nunca en este archivo ni en el repo.
- `modules/app-service.bicep`: App Service Plan Linux + Web App (.NET), identidad administrada system-assigned. SKU parametrizable (`skuName`/`skuTier`, default `B1`/`Basic`); el primer despliegue se hizo en `F1`/`Free` por falta de cuota de VMs dedicadas, y el plan ya fue escalado a `B1`/`Basic` el 2026-09-03 (`az appservice plan update -g CorpusMiner -n plan-e3s76vyfxpaec --sku B1`) una vez que Azure otorgó cuota — coincide con el default del módulo, así que un redespliegue desde `infra/main.bicep` no lo revertiría.
- `modules/sql.bicep`: Azure SQL logical server + base de datos serverless (`GP_S_Gen5`, auto-pause), regla de firewall para servicios de Azure.
- `modules/storage.bicep`: Storage Account + contenedor blob `corpus-raw-uploads` (acceso privado) para los archivos WoS/Scopus originales.
- `modules/key-vault.bicep`: Key Vault con autorización RBAC.
- `modules/app-insights.bicep`: Log Analytics Workspace + Application Insights.
- `modules/communication.bicep`: Azure Communication Services Email con dominio administrado por Azure (verificación instantánea, sin DNS). Usado para confirmación de cuenta, reset de password y notificaciones de menciones (`@usuario`) en el foro. El connection string se guarda en Key Vault (`acs-connection-string`); el remitente (`donotreply@<dominio>.azurecomm.net`) se pasa como app setting. Migrar a un dominio propio (`askit4.com`) es un cambio posterior sin rediseño (agregar dominio `CustomerManaged` + verificación DNS).

## Redesplegar / actualizar infraestructura

```bash
az deployment group create \
  --resource-group CorpusMiner \
  --template-file infra/main.bicep \
  --parameters infra/main.parameters.json \
  --parameters sqlAdministratorLoginPassword=$SQL_ADMIN_PASSWORD
```

El password de SQL no se persiste en el repo. Si se pierde, usar `az sql server update` para restablecerlo (esto no borra datos).

## Verificación segura sin desplegar

```bash
az bicep build --file infra/main.bicep
az deployment group what-if --resource-group CorpusMiner --template-file infra/main.bicep --parameters infra/main.parameters.json --parameters sqlAdministratorLoginPassword=$SQL_ADMIN_PASSWORD
```
