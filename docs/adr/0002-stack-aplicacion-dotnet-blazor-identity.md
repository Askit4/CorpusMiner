# 0002 - Stack de aplicación: .NET / Blazor / ASP.NET Core Identity

## Contexto

El MVP de CorpusMiner requiere una webapp con registro/login de usuario y carga de archivos, desplegable en Azure, desarrollada íntegramente con Claude Code dentro de este Codespace. El Codespace ya trae .NET SDK preinstalado (imagen base `universal`) sin necesidad de modificar el devcontainer. La base de datos elegida es Azure SQL ([0001](0001-base-de-datos-azure-sql.md)), y el roadmap incluye futuras visualizaciones de análisis (coautoría, redes de citación, co-ocurrencia de keywords).

## Decisión

Se usa **ASP.NET Core** con **Blazor Web App** (interactividad Server) + **ASP.NET Core Identity** (autenticación, respaldada por la misma Azure SQL Database) + **Entity Framework Core** como ORM.

## Alternativas consideradas

- **Razor Pages**: igual de válido para el registro/login del MVP (el scaffold de Identity es el mismo en ambos casos), pero las futuras visualizaciones interactivas de redes de citación/coautoría requerirían una capa SPA/JS adicional. Blazor Server cubre esa interactividad en C# puro sin salir del stack .NET. Descartada por mayor costo futuro.
- **Node.js/Python + framework aparte**: requeriría añadir un runtime/tooling nuevo al devcontainer sin necesidad real, y tendría peor integración nativa con Azure SQL y con EF Core Migrations (que ya satisfacen el requisito de `.agent/policies/data.md` de migraciones versionadas). Descartada.
- **Entra External ID en vez de ASP.NET Core Identity**: opción válida a futuro para SSO corporativo, pero añade complejidad de configuración no justificada para el MVP actual (un solo tipo de cuenta, sin requisito de SSO todavía). Queda como pregunta abierta en `docs/SOLUTION-BRIEF.md`.

## Consecuencias

- Registro/login se obtienen directamente del scaffold de ASP.NET Core Identity (`dotnet new blazor -au Individual`), sin trabajo adicional.
- Las futuras UIs de análisis (coautoría, citas, co-ocurrencia) se construyen como componentes Blazor server-side.
- Migrar a Entra External ID en el futuro es una migración de proveedor de autenticación, no un cambio de framework.

## Fecha

2026-08-25

## Estado

accepted
