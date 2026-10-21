# Plan de continuidad a Fase 2

## Alcance conocido

El documento de Fase 1 indica continuar la misma idea y equipo en una aplicación
ASP.NET conectada a base de datos. Falta el enunciado específico de Fase 2; las
siguientes decisiones son propuestas técnicas, no requisitos confirmados del docente.

## Pasos de implementación

1. Confirmar enunciado de Fase 2, versión de ASP.NET exigida y motor de BD permitido.
2. Crear un proyecto ASP.NET y reutilizar el dominio; usar modelos de entrada propios
   para formularios. No exponer las entidades directamente a la edición del navegador.
3. Diseñar persistencia: personas, estudiantes, docentes, cursos, matrículas y notas;
   claves foráneas, IDs únicos y política de borrado. Adaptar constructores/mapeo al ORM.
4. Reemplazar el almacenamiento en memoria por acceso a BD. Registrar servicios con
   un ciclo de vida adecuado; no compartir listas como almacenamiento de una web.
5. Ejecutar matrícula y cancelación dentro de transacciones. Añadir restricciones e
   índices únicos y control de concurrencia para evitar superar cupos con peticiones simultáneas.
6. Implementar pantallas reales: dashboard, listados, alta/edición y detalles; conectar
   matrículas y calificaciones. Añadir paginación y mensajes de validación.
7. Si el enunciado exige acceso por usuarios, definir roles y autorización en el servidor.
   Configurar protección de formularios, manejo de errores y secretos fuera del repositorio.
8. Pruebas de integración contra BD, migraciones, datos de demostración y guía de instalación.
9. Actualizar diagrama, capturas, presentación y video conforme a la rúbrica final.

## Decisiones que faltan

- Períodos/ciclos académicos: hoy se impide duplicar una matrícula activa estudiante-curso.
  Para repetir una materia en otro ciclo debe modelarse el período/sección explícitamente.
- Si hay varias evaluaciones por materia, crear Evaluacion y sus ponderaciones; hoy
  Calificacion representa una sola nota final por matrícula.
- Definir fórmula del CUM, créditos aprobados, retiros y fechas límite; no se deducen
  automáticamente porque el documento no fija esas reglas.
- CRUD completo de estudiantes/docentes/cursos pendiente en la interfaz web.
- La solución se verificó en .NET 10, no en .NET Framework 4.8; confirmar la versión del curso.

## Criterio de preparación

Esta base está preparada para empezar la implementación web: compila, tiene pruebas
de las reglas actuales y conserva la procedencia. No se presenta como fase 2 terminada.
