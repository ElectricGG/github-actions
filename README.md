# GitHub Actions a nivel senior

**Objetivo:** diseñar, asegurar y operar CI/CD en un monorepo con varios servicios, y defender esas decisiones en una entrevista.
**Nivel inicial:** conoce para qué sirven pipelines, Docker, Kubernetes y despliegues, pero nunca escribió workflows a mano.
**Entorno:** Windows 11, 1 a 2 h al día y más los fines de semana. Gasto bajo en Azure, con recursos apagados al terminar.
**Proyecto:** `shoplite`, tienda con API de catálogo y de pedidos en .NET y web en Angular. La app base la entrega el tutor; los workflows los escribe el alumno.
**Última sesión:** 2026-11-08

## Malla

Duración estimada: 68 h guiadas, unas 6 a 8 semanas, más el proyecto final.

### Nivel 1: CI sólido

| # | Módulo | Qué aprenderás | Práctica o entregable | Requiere | Horas | Estado |
|---|--------|----------------|-----------------------|----------|-------|--------|
| 1 | Anatomía de un workflow | Eventos, jobs, steps, runners y logs | Primer workflow escrito a mano y depurado desde los logs | — | 2 | terminado |
| 2 | El lenguaje de Actions | Contextos, expresiones, `if`, variables, outputs y `workflow_dispatch` | Workflow que decide qué hacer según el evento y la rama | 1 | 3 | terminado |
| 3 | CI de la API | `setup-dotnet`, tests, cobertura, artifacts y caché | CI de .NET con tiempos medidos antes y después de la caché | 2 | 3 | en curso |
| 4 | Pipelines reales | Matrices, `needs`, service containers, `concurrency` y `timeout` | Tests de integración con Postgres en varias versiones | 3 | 3 | pendiente |
| 5 | Flujo de PR | Rulesets, checks obligatorios y CODEOWNERS | `main` protegida: nada entra sin CI en verde | 4 | 3 | pendiente |

### Nivel 2: Entrega continua

| # | Módulo | Qué aprenderás | Práctica o entregable | Requiere | Horas | Estado |
|---|--------|----------------|-----------------------|----------|-------|--------|
| 6 | Imágenes | Docker multi-stage, Buildx, caché de capas, GHCR y etiquetado | Imagen publicada y trazable a su commit | 3 | 4 | pendiente |
| 7 | Nube sin secretos | OIDC con Azure y claims del token | Login a Azure sin ninguna contraseña guardada | 2 | 3 | pendiente |
| 8 | Infraestructura como código | Terraform en CI: `plan` en el PR y `apply` al fusionar | Infra de Azure creada solo desde el pipeline, con workflows manuales `up` y `destroy` | 7 | 4 | pendiente |
| 9 | Despliegue por ambientes | Environments, aprobaciones, promoción de imagen, smoke tests y rollback | Flujo dev → staging → prod en Azure Container Apps | 6, 8 | 5 | pendiente |
| 10 | Releases | SemVer, tags, changelog automático y release-please | Release automático que termina en producción | 9 | 3 | pendiente |

### Nivel 3: Arquitectura para proyectos grandes

| # | Módulo | Qué aprenderás | Práctica o entregable | Requiere | Horas | Estado |
|---|--------|----------------|-----------------------|----------|-------|--------|
| 11 | Reutilización | Composite actions y reusable workflows, y cuándo usar cada uno | Pipeline duplicado convertido en piezas compartidas y versionadas | 4, 9 | 4 | pendiente |
| 12 | Monorepo | Filtros por ruta, matriz dinámica y checks obligatorios con jobs omitidos | Solo se construye y despliega lo que cambió | 11 | 4 | pendiente |
| 13 | Actions propias | Action en TypeScript y action en Docker, con sus tests y versiones | Action propia usada por el pipeline | 11 | 5 | pendiente |
| 14 | Seguridad | Permisos de `GITHUB_TOKEN`, script injection, `pull_request_target`, SHA pinning, lockfiles, Dependabot, CodeQL, attestations y protecciones de ejecución | Ataque al propio workflow y su corrección | 9 | 5 | pendiente |

### Nivel 4: Operación senior

| # | Módulo | Qué aprenderás | Práctica o entregable | Requiere | Horas | Estado |
|---|--------|----------------|-----------------------|----------|-------|--------|
| 15 | Runners propios | Self-hosted, autoescalado con ARC y sus riesgos | Runners autoescalables en Kubernetes local | 4 | 5 | pendiente |
| 16 | Kubernetes | Helm y despliegue a un clúster desde el pipeline | Despliegue a un clúster local con verificación de rollout | 6, 15 | 4 | pendiente |
| 17 | Operar a escala | Costos, tiempos, métricas, `actionlint`, debug, pasos paralelos, políticas de organización y migración desde Azure DevOps | Pipeline optimizado con mejora medida | 12 | 4 | pendiente |
| 18 | Entrevistas y GH-200 | Diseño de pipelines en pizarra, preguntas típicas, incidentes y temario oficial | Simulacro de entrevista y mapa de brechas para la GH-200 | Todos | 4 | pendiente |

### Proyecto final sin guía

Construir solo y de cero el CI/CD de un sistema distinto, con requisitos de seguridad, despliegue y escala. El enunciado se entrega al terminar la malla.

**Paso actual:** módulo 3, paso 4 (cierre): pasar la cobertura a otro job con `download-artifact` y exigir un mínimo. Medición de caché: sin caché Restaurar promedio 6.8 s (5 runs); con caché (hit en 5 de 5) Caché 1.6 s + Restaurar 5.2 s = 6.8 s. Sin ganancia neta: el proyecto tiene pocos paquetes y el restore se va en arrancar MSBuild, no en descargar.

## Decisiones

- Modo proyecto: un solo sistema que crece, más parecido al trabajo real que ejemplos sueltos.
- Repo público `ElectricGG/github-actions`: los runners estándar son gratis en repos públicos.
- Organización gratuita de GitHub desde el hito 11: permite practicar workflows compartidos y políticas.
- Azure como nube: reutiliza experiencia previa y `az` ya está instalado.
- Clústeres locales en lugar de AKS: AKS cobra aunque no se use.
- Kubernetes se mantiene en los hitos 15 y 16: aparece en empresas grandes y en entrevistas senior.
- Workflows `up` y `destroy` desde el hito 8: permiten borrar y recrear la nube durante las pausas.
- Runner con versión fija en los workflows reales desde el hito 3: `ubuntu-latest` puede cambiar de versión sin avisar en el repo.
## Conceptos por reforzar

- Cada job corre en una máquina limpia y aislada: los archivos no pasan solos de un job a otro.
- Cada `run` es un proceso de shell nuevo: sus variables no pasan al siguiente paso, y una variable vacía no hace fallar el job.
- Diagnóstico de fallos: `gh run list`, `gh run view <ID> --log-failed` y leer la línea justo encima de `##[error]`.
- Un 403 de la API casi siempre es de identidad o permisos, no del YAML: revisar primero `gh auth status` (la cuenta activa debe ser `ElectricGG`).
- `gh workflow run` usa el YAML del repo remoto, no el local: sin `git push` los cambios no existen para GitHub.
- "Workflow does not have X trigger", o un run de 0 s con la ruta del archivo como nombre, indica YAML inválido. En texto sin comillas, `: ` rompe el YAML; en `run:` usar `|`.
- Un `if` va completo dentro de `${{ }}` (o sin `${{ }}`), nunca a medias: `${{ a }} == 'b'` es un texto no vacío y siempre da verdadero.
- `${{ }}` se pega en el script antes de que bash lo lea: los inputs van por `env:` para evitar inyección.

## Recursos activos en la nube

- (ninguno por ahora)
