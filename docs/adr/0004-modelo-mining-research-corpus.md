# 0004 - Modelo de datos: MiningResearch / Corpus / Paper

## Contexto

CorpusMiner necesitaba su primera implementación real del corpus bibliométrico: un usuario crea un `MiningResearch` (proyecto de investigación), dentro de el crea uno o mas `Corpus`, y a cada `Corpus` sube archivos WoS/Scopus que se combinan automáticamente. Los análisis (coautoría, redes de citación, y a futuro extracción de teorías/constructos vía LLM) son trabajo posterior — este ADR cubre solo el modelo de ingesta y almacenamiento.

## Decisión

- Jerarquía `MiningResearch` (1) → `Corpus` (N) → `Paper` (N), con `CorpusSourceFile` (el archivo crudo subido) y `PaperSourceRecord` (los campos crudos que un archivo aportó a un `Paper` especifico) como evidencia/trazabilidad.
- La fusión ocurre **dentro de un mismo Corpus**, al subir un archivo: cada registro parseado se empareja contra los `Paper` existentes por `Doi` (si está presente) o por `Titulo` normalizado + `Año`; si hay match, se fusiona (se completan campos nulos, se agrega el JSON crudo de la nueva fuente); si no, se crea un `Paper` nuevo.
- Los campos que aún no se normalizan (autores, journal detallado, keywords, referencias citadas) viven como texto crudo dentro de `Paper.RawMetadataJson` (JSON por `CorpusSourceType`: `{"WebOfScience": {...}, "Scopus": {...}}`), sobre una columna `nvarchar(max)` — no se usa el tipo `json` nativo (aún muy nuevo en el ecosistema EF Core/Azure SQL) para no depender de algo poco probado.
- El parseo se ejecuta de forma síncrona en el mismo request de subida (con estado `Uploaded → Parsing → Parsed/Failed` en `CorpusSourceFile` para dar feedback), cargando los `Paper` existentes del Corpus en memoria una sola vez por archivo (en vez de una query por registro) para que archivos de decenas de miles de filas no generen miles de round-trips a Azure SQL.
- El archivo crudo se guarda en Blob Storage (`corpus-raw-uploads`, ya declarado en infra desde el bootstrap) vía una connection string en Key Vault, siguiendo el mismo patrón ya usado para SQL y ACS — no se introduce autenticación por Managed Identity/RBAC solo para esta feature.

## Alternativas consideradas

- **Combinar corpus con corpus** (un botón para fusionar dos `Corpus` ya existentes): descartado por ahora — no fue pedido y la combinación real (la que sí se pidió) ocurre a nivel de archivos dentro de un mismo Corpus.
- **Normalizar Author/Journal/Keyword en tablas propias desde ahora**: descartado — sin un análisis real que las consuma (coautoría, co-ocurrencia), normalizar esas entidades ahora sería adivinar la forma correcta. Se normalizan cuando el primer análisis que las necesite se implemente.
- **Cola de procesamiento en background** para el parseo: descartado para este alcance — los volúmenes esperados (decenas de miles de registros) se procesan en segundos/minutos de forma síncrona; una cola es una optimización a futuro si el volumen crece.

## Consecuencias

- Subir el mismo archivo dos veces, o dos archivos con papers superpuestos, no duplica `Paper` — los fusiona, y cada fusión queda evidenciada en `PaperSourceRecord`.
- El próximo análisis bibliométrico que necesite autores/keywords estructurados tendrá que parsearlos desde `RawMetadataJson` primero, o iniciar la normalización de esas entidades como parte de esa misma tarea.

## Fecha

2026-09-03

## Estado

accepted
