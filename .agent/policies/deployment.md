# Deployment

`DIRECT-PROD` solo es posible con `deployment.direct_prod_enabled=true` y cambio pequeno, entendido, reversible, observable, compatible y con rollback claro.

Use `TEMP-FIRST` ante duda y para cambios de IA, identidad, permisos, red, topologia, servicios/SKU relevantes, contratos incompatibles, migraciones destructivas, cambios coordinados, observabilidad insuficiente o rollback incierto.

- Publique el SHA candidato antes de desplegar.
- Use el mismo SHA/digest validado en temporal y produccion.
- Adquiera lock y registre `PREVIOUS_PROD_SHA` antes de escribir produccion.
- Valide health, journeys, errores, telemetria y datos.
- Si falla haga rollback y conserve evidencia.
- Si valida alinee Git con produccion y cree tag `prod/<UTC>-<sha-corto>`.
- Un fallo `DIRECT-PROD` obliga `TEMP-FIRST` en el siguiente intento.
- Recursos temporales deben tener TTL y limpieza.
