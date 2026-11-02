# Errores encontrados

| Hito | Error | Causa | Solución |
|------|-------|-------|----------|
| 1 | `Process completed with exit code 1` en `exit 1` | Fallo forzado: un código distinto de 0 significa error, y los pasos normales siguientes se omiten | Quitar el comando que falla |
| 1 | `cat: mensaje.txt: No such file or directory` | Los jobs corrían en paralelo y `leer` falló antes de que `saludar` creara el archivo | `needs: saludar` ordena los jobs, pero no comparte archivos |
| 1 | El mismo error de `cat` después de agregar `needs` | Cada job corre en otra máquina y el archivo se queda en la primera | Pasar archivos con artifacts (hito 3) |
| 1 | `hostname` igual en dos jobs de máquinas distintas | `hostname` no identifica al runner; causa exacta sin confirmar | Comparar el Worker ID del log o `runner_id` de la API |
