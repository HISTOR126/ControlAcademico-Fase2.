-- Esquema inicial SQLite generado desde el modelo EF Core.
-- La aplicación lo crea automáticamente con EnsureCreated.
PRAGMA foreign_keys = ON;

CREATE TABLE "Calificaciones" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Calificaciones" PRIMARY KEY AUTOINCREMENT,
    "Version" INTEGER NOT NULL,
    "MatriculaId" INTEGER NOT NULL,
    "Nota" REAL NOT NULL,
    CONSTRAINT "CK_Nota_Rango" CHECK (Nota >= 0 AND Nota <= 10),
    CONSTRAINT "FK_Calificaciones_Matriculas_MatriculaId" FOREIGN KEY ("MatriculaId") REFERENCES "Matriculas" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Cursos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cursos" PRIMARY KEY AUTOINCREMENT,
    "Version" INTEGER NOT NULL,
    "Codigo" TEXT NOT NULL,
    "Nombre" TEXT NOT NULL,
    "Creditos" INTEGER NOT NULL,
    "CupoMaximo" INTEGER NOT NULL,
    "DocenteId" INTEGER NOT NULL,
    CONSTRAINT "CK_Curso_Creditos" CHECK (Creditos > 0),
    CONSTRAINT "CK_Curso_Cupo" CHECK (CupoMaximo > 0),
    CONSTRAINT "FK_Cursos_Docentes_DocenteId" FOREIGN KEY ("DocenteId") REFERENCES "Docentes" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Docentes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Docentes" PRIMARY KEY AUTOINCREMENT,
    "Version" INTEGER NOT NULL,
    "CodigoEmpleado" TEXT NOT NULL,
    "Nombre" TEXT NOT NULL,
    "Apellido" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Telefono" TEXT NOT NULL,
    "FechaNacimiento" TEXT NOT NULL,
    "Especialidad" TEXT NOT NULL
);

CREATE TABLE "Estudiantes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Estudiantes" PRIMARY KEY AUTOINCREMENT,
    "Version" INTEGER NOT NULL,
    "Carnet" TEXT NOT NULL,
    "Nombre" TEXT NOT NULL,
    "Apellido" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Telefono" TEXT NOT NULL,
    "FechaNacimiento" TEXT NOT NULL,
    "Carrera" TEXT NOT NULL,
    "Facultad" TEXT NOT NULL,
    "CicloActual" INTEGER NOT NULL,
    "CUM" REAL NOT NULL,
    CONSTRAINT "CK_Estudiante_CUM" CHECK (CUM >= 0 AND CUM <= 10)
);

CREATE TABLE "Matriculas" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Matriculas" PRIMARY KEY AUTOINCREMENT,
    "Version" INTEGER NOT NULL,
    "Codigo" TEXT NOT NULL,
    "EstudianteId" INTEGER NOT NULL,
    "CursoId" INTEGER NOT NULL,
    "FechaMatricula" TEXT NOT NULL,
    "Activa" INTEGER NOT NULL,
    CONSTRAINT "FK_Matriculas_Cursos_CursoId" FOREIGN KEY ("CursoId") REFERENCES "Cursos" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Matriculas_Estudiantes_EstudianteId" FOREIGN KEY ("EstudianteId") REFERENCES "Estudiantes" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX "IX_Calificaciones_MatriculaId" ON "Calificaciones" ("MatriculaId");

CREATE UNIQUE INDEX "IX_Cursos_Codigo" ON "Cursos" ("Codigo");

CREATE INDEX "IX_Cursos_DocenteId" ON "Cursos" ("DocenteId");

CREATE UNIQUE INDEX "IX_Docentes_CodigoEmpleado" ON "Docentes" ("CodigoEmpleado");

CREATE UNIQUE INDEX "IX_Estudiantes_Carnet" ON "Estudiantes" ("Carnet");

CREATE UNIQUE INDEX "IX_Matriculas_Codigo" ON "Matriculas" ("Codigo");

CREATE INDEX "IX_Matriculas_CursoId" ON "Matriculas" ("CursoId");

CREATE UNIQUE INDEX "IX_Matriculas_EstudianteId_CursoId" ON "Matriculas" ("EstudianteId", "CursoId") WHERE Activa = 1;
