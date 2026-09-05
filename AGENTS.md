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

## Diseno visual, marca y requisitos legales de CorpusMiner

Para cualquier tarea que cree o modifique interfaces de usuario, frontend, portales,
dashboards, formularios, pantallas de login, componentes visuales, reportes, graficos,
redes, mapas conceptuales o contenido web:

- Lea antes de implementar:
  - `docs/design/DESIGN_SYSTEM.md`
  - `docs/design/BRAND_ASSETS.md`

- Use los assets oficiales ubicados en `assets/brand/`.
- CorpusMiner es la marca principal del producto.
- Askit4 es la marca corporativa/endosante. Use la atribucion `A product of Askit4`
  segun `docs/design/BRAND_ASSETS.md`.
- Use los favicons oficiales de CorpusMiner; no deje el favicon generico del framework.
- No invente, redibuje, sustituya ni aproxime logos.
- No derive colores desde screenshots o imagenes; use `docs/design/DESIGN_SYSTEM.md`.
- Preserve la personalidad visual: academica, cientifica, moderna, data-driven,
  confiable, tecnica, minimalista y exploratoria.
- Priorice claridad cientifica, trazabilidad y reproducibilidad sobre novedad visual.
- No declare terminada una tarea de UI sin verificar accesibilidad WCAG AA,
  uso de assets correctos y cumplimiento del sistema de diseno.

Para cualquier tarea que cree o modifique autenticacion, cuentas, formularios,
telemetria, cookies, almacenamiento, carga de archivos, exportaciones, integraciones,
IA, procesamiento de documentos, datos de investigacion o paginas publicas:

- Lea antes de implementar:
  - `docs/legal/PRIVACY_AND_SECURITY_REQUIREMENTS.md`
  - `docs/legal/WEBSITE_LEGAL_REQUIREMENTS.md`
  - `docs/legal/INTELLECTUAL_PROPERTY.md`

- Trate privacidad por diseno y minimizacion de datos como requisitos funcionales.
- No agregue trackers, cookies no esenciales, analytics, pixels o SDKs externos sin
  documentar finalidad, datos enviados, proveedor, retencion y mecanismo de consentimiento.
- No envie corpus, documentos, metadata o datos de usuario a proveedores de IA o terceros
  sin que la integracion este documentada y aprobada para ese uso.
- No registre secretos, tokens, contenido sensible, documentos completos ni datos personales
  innecesarios en logs.
- Respete retencion, borrado, exportacion y trazabilidad definidos para el producto.
- No elimine enlaces de Privacy, Terms, Legal/About o atribucion corporativa requeridos
  para superficies publicas.
- No declare cumplimiento legal absoluto. Las plantillas legales requieren validacion
  juridica antes de publicarse como documentos vinculantes.

### Excepcion de lectura en bootstrap

Si el modo `bootstrap` limita inicialmente la lectura a `docs/SOLUTION-BRIEF.md` y
`.agent/policies/bootstrap.md`:

- si la configuracion incluye UI, frontend, branding o assets visuales, lea ademas:
  - `docs/design/DESIGN_SYSTEM.md`
  - `docs/design/BRAND_ASSETS.md`

- si la configuracion incluye autenticacion, datos de usuario, uploads, almacenamiento,
  telemetria, IA, integraciones externas o sitio publico, lea ademas:
  - `docs/legal/PRIVACY_AND_SECURITY_REQUIREMENTS.md`
  - `docs/legal/WEBSITE_LEGAL_REQUIREMENTS.md`
  - `docs/legal/INTELLECTUAL_PROPERTY.md`


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
