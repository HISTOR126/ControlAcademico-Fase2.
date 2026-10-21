



**Estado:** dominio y consola funcionales, pruebas automatizadas y bocetos HTML.
La aplicación ASP.NET y la base de datos todavía están pendientes. No se dispone
del enunciado específico de Fase 2: esta base no equivale a una entrega final de esa fase.

## Procedencia y equipo



Integrantes indicados en el README original (nombres únicos):

| Integrante | Carnet |

| José Miguel Sosa Ayala | SA251597 |




# Sistema de Control Académico

Proyecto de cátedra de la asignatura Desarrollo de Aplicaciones con Software Propietario (DSP404), Universidad Don Bosco.

## Descripción

Sistema orientado al registro y control de estudiantes, docentes, cursos, matrículas y calificaciones de una institución educativa.

El proyecto utiliza Programación Orientada a Objetos en C#. Actualmente cuenta con un prototipo de consola que almacena los datos en memoria y una propuesta de interfaz para la futura aplicación web.

## Diseño del proyecto

El sistema se organiza en tres componentes:

- **Dominio:** contiene las entidades y las reglas del sistema académico.
- **Consola:** permite interactuar con el sistema mediante un menú.
- **Pruebas:** comprueban las reglas de matrículas, cupos y calificaciones.

La carpeta `wireframes` contiene los bocetos de las pantallas y la carpeta `docs` incluye el diagrama de clases y la documentación.

## Entidades principales

- **Persona:** clase abstracta que reúne los datos comunes de estudiantes y docentes.
- **Estudiante:** contiene carnet, carrera, facultad e información académica.
- **Docente:** contiene código de empleado y especialidad.
- **Curso:** representa una asignatura, su docente, créditos y cupo.
- **Matrícula:** relaciona un estudiante con un curso.
- **Calificación:** representa la nota final de una matrícula y determina su aprobación.

## Programación Orientada a Objetos

Se utiliza herencia entre Persona, Estudiante y Docente, encapsulamiento para proteger los datos y polimorfismo para obtener la información de cada tipo de persona.

Las interfaces IRegistrable e IEvaluable definen comportamientos comunes de las entidades.

## Funcionalidades actuales

- Listar personas, cursos, matrículas y calificaciones.
- Buscar estudiantes por carnet.
- Registrar matrículas y comprobar disponibilidad de cupos.
- Evitar matrículas duplicadas.
- Cancelar matrículas que todavía no tienen calificación.
- Registrar y actualizar notas entre 0 y 10.
- Validar los datos y manejar errores durante las operaciones.

## Tecnologías

- C# y .NET 10.
- Aplicación de consola.
- Colecciones genéricas List<T>.
- HTML y CSS para los bocetos de interfaz.
- Git y GitHub.








