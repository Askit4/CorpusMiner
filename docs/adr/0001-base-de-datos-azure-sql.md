# 0001 - Base de datos: Azure SQL Database

## Contexto

CorpusMiner normaliza exportaciones bibliométricas de fuentes con esquemas de campos distintos (Web of Science, Scopus) en un corpus de papers, autores, journals, keywords y citas. Los análisis académicos previstos a futuro (coautoría, redes de citación, co-ocurrencia de keywords) son intrínsecamente relacionales: requieren joins y agregaciones entre esas entidades. El usuario también expresó preferencia explícita por una base de datos estructurada frente a una base documental, por familiaridad.

## Decisión

Se usa **Azure SQL Database** como almacenamiento principal. El soporte JSON nativo de Azure SQL (`JSON_VALUE`, `JSON_QUERY`, `OPENJSON`, tipo `JSON`) se usa para los campos variables entre WoS/Scopus y futuros tipos de corpus, sin necesidad de un modelo full-documento.

## Alternativas consideradas

- **Cosmos DB**: modelo documental flexible, pero los joins/agregaciones relacionales que requiere el análisis bibliométrico son costosos y torpes de expresar; further, escalado global/horizontal no es una necesidad real de este proyecto (uso interno, volumen bajo-moderado). Descartada.
- **PostgreSQL (Azure Database for PostgreSQL)**: JSONB le daría flexibilidad de esquema equivalente, pero sin ventaja clara sobre Azure SQL para este caso, y con menor familiaridad/preferencia del usuario y del resto del stack elegido (.NET/EF Core tiene integración igualmente madura con ambos, pero Azure SQL simplifica la superficie de herramientas dentro de un stack 100% Microsoft/Azure). Descartada.

## Consecuencias

- Todo cambio de esquema debe ser una migración de Entity Framework Core versionada en el repositorio (alineado con `.agent/policies/data.md`).
- Los análisis futuros (coautoría, citas, co-ocurrencia) se implementan con consultas SQL relacionales, no con reestructuración documental.
- La flexibilidad de campos entre WoS/Scopus/futuros corpus se resuelve con columnas `JSON` dentro de tablas relacionales, no con colecciones sin esquema.

## Fecha

2026-08-25

## Estado

accepted
