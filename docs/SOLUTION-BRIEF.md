# Solution Brief

Complete este archivo antes de solicitar a un agente que ejecute el bootstrap.

No es necesario definir la arquitectura técnica completa. El propósito es proporcionar suficiente contexto para que el agente pueda proponerla sin inventar requisitos de negocio.

## 1. Nombre de la solución

**Nombre:** CorpusMiner

**Slug sugerido:** corpusminer

## 2. Problema que debe resolver

Los investigadores de Askit4.com necesitan centralizar y normalizar exportaciones bibliométricas heterogéneas (Web of Science, Scopus), que hoy viven como archivos sueltos con esquemas de campos distintos entre sí, para poder acumular un corpus consultable y, a futuro, ejecutar análisis académicos (coautoría, redes de citación, co-ocurrencia de palabras clave, etc.) sobre ese corpus.

## 3. Usuarios

Investigadores/analistas internos de Askit4.com. Autoregistro habilitado, pero toda cuenta nueva queda pendiente de aprobacion por un administrador antes de poder iniciar sesion. Tres roles:

- **Admin**: aprueba/rechaza cuentas, asigna roles, publica datasets en el foro.
- **Contributor**: publica datasets en el foro.
- **Read**: consulta el foro, comenta y da like; no publica datasets.

Cuenta admin preregistrada como excepcion de bootstrap: `domingo.rojas@askit4.com` (rol Admin, aprobada desde el inicio; establece su password via "Olvide mi password").

## 4. Alcance inicial

### Incluye

- Registro y login de usuario (ASP.NET Core Identity), con aprobacion administrativa obligatoria antes del primer acceso.
- Roles Admin/Contributor/Read con modulo de administracion (`/admin/users`) para aprobar cuentas y asignar rol.
- Carga de archivos de exportacion WoS y Scopus.
- Almacenamiento del archivo crudo subido.
- Normalizacion minima a un modelo de corpus (papers, autores, journals, keywords, citas) persistido en base de datos relacional.
- Listado del corpus cargado por el usuario.
- Foro de datasets: Admin/Contributor publican datasets; cualquier usuario aprobado comenta y da like; menciones `@usuario` en comentarios notifican por correo.
- Interfaz bilingue (es/en) con selector de idioma.
- Identidad visual basada en el logo de Askit4 (paleta negro/teal).

### No incluye

- Análisis académicos (coautoría, redes de citación, co-ocurrencia de keywords) — trabajo futuro.
- Inteligencia artificial.
- Multi-tenant real / roles múltiples.
- Otros tipos de corpus (reseñas de apps Android/iOS, respuestas de encuestas) — el modelo de datos debe evitar acoplarse por completo a la semántica de "paper", pero la implementación de esos verticals queda fuera de este alcance inicial.

## 5. Capacidades esperadas

- Registro/login de usuarios.
- Carga de archivo(s) de exportación WoS/Scopus.
- Normalización a un corpus estructurado, versionado por fuente de origen.
- Consulta básica de lo cargado (qué corpus existen, cuántos registros, cuándo se cargaron).

## 6. Datos

**Fuentes de datos:** Archivos de exportación de Web of Science (formato tipo tag/RIS) y Scopus (CSV), subidos manualmente por el usuario a través de la webapp.

**Datos sensibles o regulados:** Los metadatos bibliográficos (títulos, autores, journals, citas) son mayormente de carácter público. Los datos de cuenta del usuario (correo, credenciales) son datos personales y deben tratarse como tales (sin exponerlos, sin registrarlos en texto plano).

**Clasificación esperada:** Interna (no pública por defecto). No se identifica hoy una regulación sectorial específica aplicable.

## 7. Integraciones

Ninguna en el MVP. No hay integración directa con las APIs de WoS/Scopus; los archivos son exportados manualmente por el usuario desde esas plataformas.

## 8. Inteligencia artificial

**¿Se espera usar IA?** Por definir (no en este MVP).

**Casos de uso de IA:** Ninguno en este MVP. A futuro podría aplicarse a los análisis académicos (p. ej. extracción de temas, clasificación de abstracts).

**Proveedor o restricciones conocidas:** N/A por ahora. `ai.enabled=false` mientras tanto.

## 9. Tecnología

**Stack obligatorio, si existe:** Ninguno impuesto externamente.

**Stack preferido, si existe:** .NET / ASP.NET Core (Blazor Web App, interactividad Server) + ASP.NET Core Identity + Entity Framework Core, sobre Azure SQL Database. Infraestructura como codigo con Bicep. Envio de correo transaccional y de notificaciones (confirmacion, reset de password, menciones @usuario) via Azure Communication Services Email (dominio administrado por Azure). Decision tomada considerando que todo el desarrollo se hace con Claude Code dentro de este Codespace (que ya trae .NET SDK, Azure CLI + Bicep y PowerShell) y que el analisis bibliometrico es intrinsecamente relacional.

**Tecnologías prohibidas o restricciones:** Ninguna conocida.

## 10. Azure

**Tenant conocido:** Askit4 (`86dae9fc-f0c4-4be1-819e-a6f54d7b7220`).

**Suscripcion conocida:** Askit4Ops (`c8824713-3428-42b8-9abe-76a8c66a857f`).

**Region preferida:** `centralus`.

**Recursos existentes que deben reutilizarse:** Resource group `CorpusMiner` ya creado (sirve como ambiente de desarrollo/pruebas y, por ahora, tambien como el unico ambiente "productivo" mientras el app no esta en uso real).

## 11. Requisitos no funcionales conocidos

- **disponibilidad:** sin SLA formal en el MVP (best-effort de Azure App Service).
- **rendimiento:** cargas de archivos del orden de decenas de miles de registros deben procesarse en segundos a pocos minutos; sin objetivo estricto todavía.
- **seguridad:** autenticación obligatoria para subir/ver datos; sin secretos ni cadenas de conexión en Git (ver `.agent/policies/security.md`).
- **privacidad:** los datos de cuenta se tratan como datos personales, con mínimo privilegio de acceso.
- **recuperación:** sin objetivo de RPO/RTO formal todavía; se define al provisionar Azure SQL.
- **costo:** minimizar costo durante la fase de validación (SKUs serverless/básicos en Azure SQL y App Service).
- **volumen esperado:** bajo en el MVP — uso interno, pocos usuarios, corpus creciente pero moderado.

## 12. Criterios de aceptación del MVP

- Un usuario puede registrarse y autenticarse en la webapp.
- Un usuario autenticado puede subir un archivo de exportación WoS o Scopus.
- El sistema almacena el archivo crudo y crea al menos un registro de corpus asociado al usuario en la base de datos.
- El usuario puede ver que su carga quedó registrada (listado básico).

## 13. Restricciones y decisiones humanas ya tomadas

- **Base de datos:** Azure SQL Database (no Cosmos DB, no PostgreSQL) — por la naturaleza relacional del análisis bibliométrico (joins/agregaciones entre papers, autores, journals y citas) y por preferencia explícita del usuario hacia una base de datos estructurada. El soporte JSON nativo de Azure SQL cubre la flexibilidad de esquema necesaria para los campos variables entre WoS/Scopus y futuros tipos de corpus.
- **Backend/frontend:** .NET / ASP.NET Core (Blazor Web App) + ASP.NET Core Identity + EF Core.
- **IaC:** Bicep.- **Correo:** Azure Communication Services Email (dominio administrado por Azure para arrancar; se puede migrar a dominio propio askit4.com mas adelante).
- **Seguridad de acceso:** autoregistro + aprobacion administrativa obligatoria + roles Admin/Contributor/Read. Cuenta admin preregistrada: `domingo.rojas@askit4.com`.- `ai.enabled=false` durante esta fase.
- Todo el desarrollo se realiza con Claude Code dentro de este Codespace, que ya dispone de Azure CLI y PowerShell.

## 14. Preguntas abiertas

- ¿Qué tenant/suscripción/región de Azure se usará? (bloqueado hasta que el usuario ejecute `az login` interactivo).
- ¿Se necesitarán roles/permisos multiusuario más allá de un solo tipo de cuenta?
- ¿Qué formato exacto de exportación WoS se debe soportar primero (texto plano etiquetado, RIS, BibTeX)?
- ¿Se requerirá Entra External ID a futuro para SSO corporativo, en lugar de ASP.NET Core Identity?
