1. Qué hay en el runner al arrancar y para qué sirve `actions/checkout`.
- La imagen trae sistema operativo, Git, Docker, SDKs (.NET, Node, Python) y CLIs (`az`, `gh`, `kubectl`), pero no el código: la carpeta de trabajo está vacía.
- `checkout` clona el commit exacto del run (`github.sha`). Por defecto baja solo ese commit (`fetch-depth: 1`); para historial o tags se usa `fetch-depth: 0`.

2. `uses:` y `with:`.
- `uses:` ejecuta una action: otro repo con un `action.yml` que declara sus inputs. `with:` le pasa esos inputs, como argumentos de una función. `@v4` es un tag que puede moverse; un SHA es inmutable (módulo 14).
- El `name:` del paso es la etiqueta en el log; `with.name` es un parámetro de la action.

3. `defaults.run.working-directory`.
- Hace que cada `run:` arranque en esa carpeta. Solo aplica a `run:`, nunca a `uses:`: las rutas de `setup-dotnet`, `upload-artifact` y `hashFiles` van desde la raíz del repo.

4. Restore, build y test separados, en Release.
- `dotnet restore`, `dotnet build -c Release --no-restore` y `dotnet test -c Release --no-build`. Separados se mide cada fase y se sabe dónde falló.
- Release porque se prueba lo que se despliega ("test what you ship"). Lo obligatorio es usar la misma configuración en build y test: `--no-build` busca los binarios de esa configuración.

5. Cobertura.
- coverlet inserta contadores en el DLL y registra qué líneas y qué ramas ejecutaron los tests. Líneas: se ejecutó al menos una vez. Ramas: se probaron todos los caminos de cada decisión.
- 100 % de líneas puede ser 83 % de ramas. La cobertura no dice si el código funciona, solo qué no se probó; un test sin asserts también sube la cobertura.

6. Artifacts.
- `upload-artifact` guarda una carpeta en GitHub con un nombre (90 días por defecto) y `download-artifact` la trae por ese nombre en otro job o con `gh run download`. Son genéricos: lo que guardan depende del nombre y del `path`.
- La subida va con `if: always()` para tener los resultados justo cuando fallan los tests.
- `outputs` llevan texto corto entre jobs; los artifacts llevan archivos.

7. Caché.
- `actions/cache` restaura una carpeta al inicio y la guarda en el paso Post al final del job. Va después de lo que necesita su llave (checkout) y antes de lo que la usa (restore).
- La llave debe cambiar cuando cambia el contenido: `hashFiles` de todo lo que define versiones (`.csproj`, `Directory.Packages.props`, lockfiles). Las versiones flotantes no cambian la llave.
- Hit: la llave principal existe y no se vuelve a guardar (las cachés son inmutables). Miss: se descarga y se guarda. `restore-keys` restaura la más parecida y solo se baja lo que falta.
- Medido en 10 runs: sin ganancia neta en este proyecto (6.8 s contra 1.6 s + 5.2 s). La caché se mide, no se agrega por costumbre.

8. Quality gate.
- Un job descarga la cobertura, la compara con mínimos fijos en el YAML (80 % de líneas y de ramas) y hace `exit 1` si no llega. Un mínimo que se pasa como input no protege nada.
- `$GITHUB_STEP_SUMMARY` muestra Markdown en la página del run; `::error::` crea una anotación visible.
- `${{ a || b }}` usa `b` cuando `a` está vacío.

Entregable: `.github/workflows/catalogo-ci.yaml`. Runs 42516959221 (primer CI), 42519015375 (cobertura), 42540973400 (cache miss), 42541036265 a 42541071743 (cache hit), 42548796011 (gate en rojo con 90 %) y 42553190166 (final en verde).
