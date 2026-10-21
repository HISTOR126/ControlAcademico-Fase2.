# Control Académico - base para Fase 2

Continuación del proyecto de cátedra DSP404. Registra estudiantes, docentes, cursos,
matrículas y calificaciones. Esta entrega corrige el prototipo de Fase 1 y separa las
reglas del negocio de la consola para reutilizarlas al construir la aplicación web.

**Estado:** dominio y consola funcionales, pruebas automatizadas y bocetos HTML.
La aplicación ASP.NET y la base de datos todavía están pendientes. No se dispone
del enunciado específico de Fase 2: esta base no equivale a una entrega final de esa fase.

## Procedencia y equipo

Basado en https://github.com/marceloguerrero-debug/ControlAcademico/tree/1-main,
recibido como ZIP el 21 de octubre de 2026. El ZIP completo recibido está en
`referencia/ControlAcademico-Fase1-original.zip`; contiene los archivos sin modificar.
No se importó el historial Git porque el ZIP no lo incluye. Esta copia no transfiere autoría
ni cambia la pertenencia al equipo. El destino solicitado por el usuario es HISTOR126.

Integrantes indicados en el README original (nombres únicos):

| Integrante | Carnet |
|---|---|
| Marcelo Eduardo Guerrero Rodriguez | GR241968 |
| Daniela Jazmin Torres Ramos | TR232197 |
| José Miguel Sosa Ayala | SA251597 |
| Susana Nicole Valle Méndez | VM253306 |
| Alessandra Guadalupe González Burgos | GB253116 |

El original repite a Alessandra en dos asignaciones. Se conserva ese documento en
`docs/README-Fase1-original.md`; el equipo debe confirmar la distribución real de tareas.
Video original: https://drive.google.com/file/d/1njLirsd0M4dwIirE8Bt67I102BxHhEUm/view
(contenido y permisos del video no verificados).

## Requisitos y ejecución

- SDK de .NET 10. Esta base usa .NET 10 en lugar de .NET Framework 4.8 del prototipo original.
- Un editor o IDE compatible con el SDK instalado. No requiere paquetes NuGet externos.
- Abrir `ControlAcademico.slnx` o ejecutar desde la carpeta raíz:

```sh
dotnet build ControlAcademico.slnx
dotnet run --project src/ControlAcademico.Consola
dotnet run --project tests/ControlAcademico.Pruebas
```

El último comando ejecuta las pruebas y devuelve código distinto de cero si alguna falla.
Se utiliza un ejecutable de pruebas sin dependencias externas; no se ejecuta mediante `dotnet test`.

Datos de muestra: estudiantes CT-0001, CT-0002 y CT-0003, curso DSP404,
matrículas MAT-0001 y MAT-0002. Las dos matrículas iniciales tienen notas.
Para probar el ciclo completo, use la opción 6 con MAT-0003, CT-0003 y DSP404;
la opción 8 permite cancelarla mientras no tenga nota. Para calificarla use la opción 7
con ID de nota 3. No se permite cancelar matrículas calificadas.
La consola permite listar, buscar, matricular, registrar/actualizar notas y cancelar matrículas.
Al salir, los datos nuevos se pierden: siguen almacenados en memoria como en Fase 1.

## Organización

```text
src/ControlAcademico.Dominio/    Entidades, contratos y servicio en memoria
src/ControlAcademico.Consola/    Menú, entradas y presentación
tests/ControlAcademico.Pruebas/ Pruebas de reglas y regresiones
wireframes/                    Bocetos originales y pantallas complementarias
docs/                          Diagrama, cambios, plan web y guía GitHub
referencia/                    ZIP original íntegro
```

El dominio no usa `Console`: retorna datos y detalles que la consola presenta.
Los métodos `MostrarInformacion`/`MostrarDetalle` del original se sustituyeron por
`ObtenerDetalle`. Los constructores y setters se ajustaron para proteger las reglas.
Los nombres de las entidades principales se conservan.

## Diseño y continuidad

Abra `wireframes/index.html` en un navegador. Son propuestas visuales con datos de
ejemplo, sin conexión a C# ni persistencia. El diagrama actualizado está en
`docs/DIAGRAMA-CLASES.md`, que GitHub muestra con Mermaid.

Vea `docs/CAMBIOS.md`, `docs/PLAN-FASE2.md`, `docs/VALIDACION.md` y
`docs/SUBIR-A-GITHUB.md`. El diagrama PNG anterior sigue dentro del ZIP original
como referencia histórica; el documento Mermaid es el diagrama vigente.
