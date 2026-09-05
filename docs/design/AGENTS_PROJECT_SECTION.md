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
