# Data

- Todo cambio de esquema debe ser migracion versionada.
- Prefiera idempotencia y precondiciones verificables.
- Para incompatibilidades prefiera `expand -> migrate -> contract`.
- Todo script productivo debe tener preview, conteo esperado, limite, auditoria y reversa o compensacion.
- Verifique capacidad de restauracion antes de escrituras de riesgo.
- Temporal y produccion deben ejecutar los mismos archivos/hashes de migracion.
- Nunca modifique primero la base y documente despues.
- Solo el escritor de despliegue autorizado modifica datos productivos durante un run.
