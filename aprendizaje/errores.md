# Errores encontrados

| Hito | Error | Causa | Solución |
|------|-------|-------|----------|
| 1 | `Process completed with exit code 1` en `exit 1` | Fallo forzado: un código distinto de 0 significa error, y los pasos normales siguientes se omiten | Quitar el comando que falla |
| 1 | `cat: mensaje.txt: No such file or directory` | Los jobs corrían en paralelo y `leer` falló antes de que `saludar` creara el archivo | `needs: saludar` ordena los jobs, pero no comparte archivos |
| 1 | El mismo error de `cat` después de agregar `needs` | Cada job corre en otra máquina y el archivo se queda en la primera | Pasar archivos con artifacts (hito 3) |
| 1 | `hostname` igual en dos jobs de máquinas distintas | `hostname` no identifica al runner; causa exacta sin confirmar | Comparar el Worker ID del log o `runner_id` de la API |
| 2 | `HTTP 403: Must have admin rights to Repository` al lanzar un workflow | La cuenta activa de `gh` era `cosmos-dev8`, que solo puede leer el repo | `gh auth switch -u ElectricGG` |
| 2 | `HTTP 422: Unexpected inputs provided` | El commit con los `inputs` no estaba en el remoto; GitHub lee el YAML de allá | `git push` antes de `gh workflow run` |
| 2 | `HTTP 422: Workflow does not have 'workflow_dispatch' trigger` y run de 0 s con la ruta del archivo como nombre | YAML inválido: `run: echo "Cuidado: producción"` tiene `: ` en texto sin comillas | Comillas alrededor de todo el valor o bloque `run: \|` |
| 2 | Clave `steps:` repetida en un job | En YAML una clave no puede repetirse en el mismo bloque | Un solo `steps:` con todos los pasos |
| 2 | `if: ${{ inputs.ambiente }} == 'prod'` siempre verdadero | Solo una parte iba en la expresión; el resto queda como texto no vacío | `if: inputs.ambiente == 'prod'` |
