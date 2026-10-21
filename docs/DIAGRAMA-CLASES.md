# Diagramas del sistema

El modelo relacional y la arquitectura web de fase 2 están en [ARQUITECTURA-FASE2.md](ARQUITECTURA-FASE2.md).

El siguiente diagrama conserva las clases del dominio de fase 1, utilizadas por la consola y como validación base en la web.

# Diagrama de clases vigente

Las propiedades públicas que no declaran actualización son de solo lectura.
Los constructores de Matrícula y Calificación son internos al dominio.

```mermaid
classDiagram
    class Persona {
      <<abstract>>
      +int Id
      +string Nombre
      +string Apellido
      +string Email
      +string Telefono
      +DateTime FechaNacimiento
      +ObtenerNombreCompleto() string
      +ObtenerEdad() int
      +ObtenerDetalle() string
    }
    class Estudiante {
      +string Carnet
      +string Carrera
      +string Facultad
      +int CicloActual
      +double CUM
      +int CreditosAprobados
      +string EstadoAcademico
      +IReadOnlyList~string~ MateriasInscritas
      +ActualizarCUM(double)
      +ActualizarCreditosAprobados(int)
      +VerificarEgresado(int) bool
      +ObtenerDetalle() string
    }
    class Docente {
      +string CodigoEmpleado
      +string Especialidad
      +ObtenerDetalle() string
    }
    class Curso {
      +string Codigo
      +string Nombre
      +int Creditos
      +Docente DocenteAsignado
      +int CupoMaximo
      +int Inscritos
      +TieneCupoDisponible() bool
      +CambiarCupo(int)
      +ObtenerDetalle() string
    }
    class Matricula {
      +string IdMatricula
      +Estudiante Alumno
      +Curso CursoAsignado
      +DateTime FechaMatricula
      +bool Activa
      +ObtenerDetalle() string
    }
    class Calificacion {
      +int Id
      +Matricula Matricula
      +Estudiante Alumno
      +Curso Asignatura
      +double Nota
      +ActualizarNota(double)
      +EstaAprobado() bool
      +ObtenerEstado() string
      +ObtenerDetalle() string
    }
    class IEvaluable {
      <<interface>>
      +EstaAprobado() bool
      +ObtenerEstado() string
    }
    class IRegistrable {
      <<interface>>
      +ObtenerDetalle() string
    }
    class ControlAcademicoService {
      +IReadOnlyList~Persona~ Personas
      +IReadOnlyList~Curso~ Cursos
      +IReadOnlyList~Matricula~ Matriculas
      +IReadOnlyList~Calificacion~ Calificaciones
      +RegistrarPersona(Persona)
      +RegistrarCurso(Curso)
      +BuscarEstudiante(string) Estudiante
      +Matricular(string,string,string) Matricula
      +CancelarMatricula(string)
      +RegistrarCalificacion(int,string,double) Calificacion
    }
    Persona <|-- Estudiante
    Persona <|-- Docente
    IRegistrable <|.. Curso
    IRegistrable <|.. Matricula
    IRegistrable <|.. Calificacion
    IEvaluable <|.. Calificacion
    Docente "1" <-- "0..*" Curso : asignado
    Estudiante "1" <-- "0..*" Matricula : alumno
    Curso "1" <-- "0..*" Matricula : curso
    Matricula "1" <-- "0..1" Calificacion : nota final
    ControlAcademicoService o-- Persona
    ControlAcademicoService o-- Curso
    ControlAcademicoService o-- Matricula
    ControlAcademicoService o-- Calificacion
```
