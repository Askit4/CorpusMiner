# Bootstrap

Aplica solo con `lifecycle.status: bootstrap`.

1. Ejecute `agentctl context`, `agentctl doctor` y `agentctl bootstrap`.
2. Lea `docs/SOLUTION-BRIEF.md`.
3. Cree `bootstrap/<solution-slug>` antes de editar.
4. Determine stack, arquitectura, pruebas, datos, integraciones, IaC, seguridad y observabilidad sin inventar requisitos.
5. Azure: descubrimiento de solo lectura salvo autorizacion explicita para recursos no productivos.
6. Adapte el devcontainer solo si la solucion requiere herramientas fuera del toolbox base.
7. Cree solo estructura necesaria.
8. Complete `.agent/project.yaml` con valores verificables; use `not-applicable` cuando corresponda.
9. Mantenga `direct_prod_enabled: false` y `max_direct_data_rows: 0` durante bootstrap.
10. Defina comandos reproducibles de lint, test, build, validate y operaciones cuando apliquen.
11. No implemente funcionalidad de negocio mas alla del minimo necesario para validar el toolchain.
12. Cambie a `active` solo cuando `agentctl check` pase.
13. Entregue mediante commit/push/PR desde el Codespace.
