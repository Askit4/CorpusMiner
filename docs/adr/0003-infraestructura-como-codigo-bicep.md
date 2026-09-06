# 0003 - Infraestructura como código: Bicep

## Contexto

El devcontainer de este template trae tanto Azure CLI + Bicep como Terraform disponibles. La solución se despliega exclusivamente en Azure (sin requisito de portabilidad multi-nube) y todo el desarrollo se realiza dentro de este Codespace con Claude Code, Azure CLI y PowerShell.

## Decisión

Se usa **Bicep** como herramienta principal de infraestructura como código, con los módulos en `infra/`.

## Alternativas consideradas

- **Terraform**: igualmente disponible en el devcontainer, pero añade la complejidad de gestionar estado remoto (backend de Terraform) sin un beneficio claro de portabilidad multi-nube, que no es un requisito de este proyecto. Descartada.

## Consecuencias

- Los módulos de infraestructura (`infra/main.bicep`, `infra/modules/*.bicep`) se validan localmente con `az bicep build` sin necesidad de una sesión de Azure autenticada.
- El despliegue real queda pendiente de que el usuario ejecute `az login` y decida suscripción/región (ver `infra/README.md`); no se aplica infraestructura durante el bootstrap.

## Fecha

2026-08-25

## Estado

accepted
