# Comandos aprendidos

## Git y GitHub CLI

| Comando | Para qué sirve | Ejemplo real |
|---------|----------------|--------------|
| `gh auth switch` | Cambia la cuenta activa de `gh` en toda la PC | `gh auth switch -u ElectricGG` |
| `git init` | Convierte la carpeta en un repo de Git | `git init -b main` |
| `gh repo create` | Crea el repo en GitHub y sube el código local | `gh repo create github-actions --public --source . --push` |
| `gh repo view` | Muestra datos del repo | `gh repo view --json url,visibility,defaultBranchRef` |

## GitHub Actions

| Comando | Para qué sirve | Ejemplo real |
|---------|----------------|--------------|
| `gh run list` | Lista las ejecuciones y su estado | `gh run list --limit 1` |
| `gh run view --log` | Muestra el log completo de una ejecución | `gh run view 41178508234 --log` |
| `gh run view --log-failed` | Muestra solo los pasos que fallaron | `gh run view 41387353064 --log-failed` |
| `findstr` | Filtra líneas de una salida en Windows | `gh run view 41389593958 --log \| findstr "MAQUINA="` |
| `gh workflow run` | Lanza un workflow con `workflow_dispatch` y sus inputs | `gh workflow run salidas.yaml -f nombre=ana -f ambiente=prod` |
| `gh run cancel` | Cancela una ejecución en curso | `gh run cancel 42419022705` |
| `gh auth status` | Muestra las cuentas de `gh` y cuál está activa | `gh auth status` |
| `gh run download` | Descarga un artifact de un run | `gh run download 42548698652 -n cobertura` |
| `gh cache list` | Lista las cachés guardadas del repo | `gh cache list` |
| `dotnet test --collect` | Corre los tests midiendo cobertura | `dotnet test -c Release --no-build --collect:"XPlat Code Coverage" --results-directory ./resultados` |

## Docker

## Azure

## Terraform

## Kubernetes y Helm
