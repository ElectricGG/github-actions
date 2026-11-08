1. Qué es un contexto y cuáles usaste.
- Es la raíz de toda expresión dentro de `${{ }}` o de un `if:`. Usé `github` (evento, rama, actor), `inputs` (parámetros de `workflow_dispatch`), `steps` (pasos del mismo job), `needs` (jobs de los que depende el job) y `env`.

2. Cómo pasa un dato de un paso a otro y de un job a otro.
- Entre pasos: `echo "nombre=valor" >> "$GITHUB_OUTPUT"` y se lee con `steps.<id>.outputs.<nombre>`. Una variable de bash no sobrevive al paso.
- Entre jobs: el job lo publica en `outputs:` y el otro lo lee con `needs.<job>.outputs.<nombre>`. GitHub hace de mensajero porque cada job es otra máquina. Solo viaja texto.
- `needs` no se hereda: para leer un job hay que listarlo en el propio `needs:`.

3. Qué hace `needs.<job>` y qué no es.
- `needs:` hace esperar al job; la expresión `needs.<job>...` solo lee su resultado ya terminado (`outputs` y `result`). No es una instancia: el job corre una sola vez y todos leen el mismo resultado, como varios `await` del mismo `Task`.

4. Cómo se escribe bien un `if`.
- Completo dentro de `${{ }}` o sin `${{ }}`, nunca a medias. `${{ a }} == 'b'` da un texto no vacío y siempre es verdadero.
- Si empieza con `!`, va obligatoriamente con `${{ }}`: un `!` al inicio rompe el YAML.
- `if` en un job salta toda la máquina; `if` en un paso salta solo ese paso.

5. Funciones de estado.
- Todo job y paso tiene un `success()` implícito. `failure()` corre si algo falló, `cancelled()` si se canceló y `always()` siempre.
- En un job revisan los jobs de `needs`; en un paso revisan los pasos anteriores del mismo job.
- Un job omitido no cuenta como éxito ni como fallo. Para avisos se usa un job con `if: always()` y pasos que miran `needs.<job>.result` (`success`, `failure`, `cancelled`, `skipped`).

6. `inputs` en `workflow_dispatch`.
- Tipos `string`, `boolean`, `choice`, `number` y `environment`. Se leen con `inputs.*`, que respeta el tipo; `github.event.inputs.*` siempre es texto y `'false'` cuenta como verdadero.
- En un push, `inputs.*` llega vacío.
- `gh workflow run` usa el YAML del remoto: sin push no existen los inputs nuevos.

7. Por qué los valores van por `env:` y no con `${{ }}` dentro del `run`.
- `${{ }}` se pega en el script antes de que bash lo lea, así que un input malicioso se vuelve código. Por `env:` llega como variable y nunca se ejecuta.

Entregable: `.github/workflows/decidir.yaml`, runs 42442118796 (feature, ninguno), 42418898260 (push main, staging), 42419022705 (dev con simulacro) y 42419273396 (prod sin simulacro, falla y avisa).
