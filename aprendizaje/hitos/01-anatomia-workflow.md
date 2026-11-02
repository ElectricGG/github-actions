1. Qué son un workflow, un job, un step y un runner.
- Un workflow es un flujo de trabajo que ejecutara una serie de cosas paso a paso.
- Un job es una tarea a ejecutar dentro del workflow
- Un step es un paso a ejecutar dentro del job, pueden haber muchos pasos.
- Un runner es la máquina virtual donde se ejecutan los jobs de nuestros workflows

2. Qué pasa en un job cuando falla un paso.
-El job se marca como fallido y los demas pasos no se ejecutaran, pasara a complete job para limpiar el proceso.

3. Para qué sirve needs y qué no hace.
-Needs indica que job se necesita esperar que se ejecute para recien ejecutar el job marcado con needs. Lo que no hace es permitir ver archivos de otro job. El job que espera no se ejecutara si el job que esta esperando falla.
4. Por qué un job no ve los archivos de otro.
- Porque cada job corre en un runner distinto, osea una maquina distinta.
5. Cómo encuentras la causa de un fallo en los logs.
-Busca la ejecución: gh run list --limit 1 te da su ID y estado.
-Abre solo lo que falló: gh run view <ID> --log-failed muestra el paso fallado y evita el resto del log.
-Lee las últimas líneas: ahí suele estar la causa.