# AFDS-AZ Solution Template

Template corporativo Codespace-first para soluciones Azure desarrolladas y operadas por agentes de IA.

## Modelo

El GitHub Codespace es el plano de ejecucion principal. Codex, Claude Code y GitHub Copilot trabajan desde el Codespace usando `AGENTS.md` y la interfaz compacta `agentctl`. GitHub actua principalmente como remoto Git y control de cambios.

## Crear una solucion

1. Cree un repositorio desde este template.
2. Complete `docs/SOLUTION-BRIEF.md`.
3. Cree un Codespace desde `main`.
4. Abra Codex, Claude Code o Copilot.
5. Indique: `Sigue AGENTS.md y ejecuta la tarea.`
6. El agente usa `agentctl context`; en bootstrap lee solo el brief y la politica indicada.
7. El bootstrap se entrega por PR y cambia a `active` cuando `agentctl check` pasa.

## Contrato

- `AGENTS.md`: instrucciones cortas y estables.
- `.agent/project.yaml`: configuracion de la solucion.
- `.agent/policies/`: detalle cargado solo cuando aplica.
- `bin/agentctl`: interfaz determinista y compacta para el LLM.

## Toolbox

El entorno base prepara Git/GitHub CLI, Azure CLI/Bicep, PowerShell, Terraform, Node/Python, GitHub Copilot CLI, Claude Code, Codex, jq y ripgrep.

Docker, kubectl, Helm y otros SDK o CLIs específicos se agregan durante bootstrap únicamente cuando la solución los necesita.
SDKs especificos se agregan durante bootstrap solo si la solucion los necesita.

## Validacion

Dentro del Codespace:

```bash
agentctl doctor
agentctl context
agentctl check
```
