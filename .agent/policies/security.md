# Security, IA e identidad

- Secretos, tokens, credenciales, connection strings y datos sensibles no se guardan en Git, logs, prompts, issues ni reportes.
- Use Codespaces secrets, OIDC, identidad administrada o vaults segun corresponda.
- Cambios materiales de proveedor/modelo IA, embeddings, tools o permisos requieren `TEMP-FIRST`.
- Redacte datos sensibles antes de persistir prompts o telemetria.
- Use identidad verificable para APIs privadas; datos de navegador no prueban identidad.
- Valide identidad, roles, autorizacion y ownership en backend.
- Aplique minimo privilegio.
- Escale acciones irreversibles, exposicion de datos, pagos, fraude, cambios privilegiados o decisiones legales/contractuales.
