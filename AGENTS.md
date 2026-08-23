---
standard: AFDS-AZ
version: 1.3.0-draft
scope: repository
canonical: true
language: es
---

# AFDS-AZ Agent Contract

Fuente canonica para Codex, Claude Code, GitHub Copilot y otros agentes del repositorio.

## Inicio

1. Ejecute `agentctl context` antes de explorar manualmente Git, Azure o configuracion.
2. Use `agentctl` para operaciones soportadas; no repita consultas que ya devuelve `agentctl`.
3. Use Bash como shell operativo por defecto.
4. Mantenga stdout compacto. Envie salida extensa a `.agent/runs/` y lea solo el log necesario cuando falle.
5. No declare exito sin evidencia observable.

## Modo

`MODE` proviene de `.agent/project.yaml`.

### bootstrap

- Ejecute `agentctl bootstrap`.
- Lea solo `docs/SOLUTION-BRIEF.md` y `.agent/policies/bootstrap.md` antes de configurar el repositorio.
- No implemente funcionalidad de negocio ni escriba/despliegue produccion.
- Trabaje en `bootstrap/<solution-slug>`.
- Termine con `agentctl check`.

### active

- Clasifique el cambio como `DIRECT-PROD` o `TEMP-FIRST`.
- Lea politicas solo cuando apliquen: deployment, data, security.
- Ejecute comandos del proyecto mediante `agentctl run <nombre>` cuando esten definidos.

## Invariantes

- Git es memoria durable; Codespace es estado temporal; Azure es estado vivo.
- No despliegue codigo, IaC, migraciones ni scripts cuyo SHA exacto no exista primero en remoto.
- `main` representa el ultimo estado productivo validado salvo configuracion explicita distinta.
- Preserve trabajo ajeno; no revierta cambios no propios sin autorizacion.
- Secretos, tokens, credenciales y datos sensibles no se escriben en Git, logs, prompts, issues ni reportes.
- Solo un escritor modifica produccion durante un run y debe existir lock/concurrencia.
- Cambios de datos/esquema deben existir primero como scripts/migraciones versionados.
- Promueva el mismo SHA/digest validado; no reconstruya para produccion.
- Ante duda use `TEMP-FIRST`.
- Placeholder critico o configuracion ambigua bloquea escrituras y despliegues.

## Multiagente

- El orquestador integra y mantiene invariantes.
- Diagnostico y validacion/riesgo deben ser perspectivas independientes.
- Dos agentes no editan el mismo archivo simultaneamente.
- Use ramas/worktrees separados cuando exista paralelismo real.
- Sin paralelismo real registre `PARALLELISM_DEGRADED`.

## Git

- `bootstrap/<slug>`: configuracion inicial.
- `run/<run-id>`: candidato integrado.
- `agent/<run-id>/<rol>`: trabajo aislado.
- `prod/<UTC>-<sha-corto>`: tag productivo.
- Antes de publicar: `agentctl check`, pruebas pertinentes y `git diff --check`.

## Cloud y produccion

- Verifique tenant, suscripcion, ambiente y recurso objetivo antes de escribir.
- Descubrimiento inicial debe ser de solo lectura.
- `deployment.direct_prod_enabled=false` bloquea `DIRECT-PROD`.
- Cambios de IA, identidad, permisos, red, topologia, contratos incompatibles, migraciones destructivas o resultados inciertos son `TEMP-FIRST`.
- Un fallo `DIRECT-PROD` reclasifica el siguiente intento como `TEMP-FIRST`.

## Finalizacion

Una tarea termina solo cuando cambios y evidencia estan versionados, pruebas requeridas pasaron, el ambiente objetivo reporta la version esperada cuando aplica, rollback permanece disponible, recursos temporales fueron limpiados o tienen TTL, Git y produccion no quedan ambiguos y `agentctl check` pasa.
