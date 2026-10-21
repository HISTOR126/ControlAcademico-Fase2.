# Cambios respecto de Fase 1

- Solución y proyectos .NET 10 listos para compilar; .gitignore y flujo de CI.
- Separación de dominio, consola y pruebas sin paquetes externos.
- Registro de matrículas mediante un único servicio: valida existencia, cupo, ID y duplicados
  antes de actualizar curso, estudiante y colección. El constructor es interno.
- Cancelación consistente que libera cupo y materias; se rechaza si tiene calificación.
- Notas asociadas a una matrícula activa; una nota final por matrícula en este prototipo.
- Actualizar una nota comprueba el rango 0..10 y rechaza NaN e infinito.
- Identificadores normalizados y únicos; búsqueda de carnet tolerante a espacios y mayúsculas.
- Datos obligatorios, correo, fecha, cupos y créditos validados. Sin setters públicos que
  permitan romper esos controles. Colecciones de solo lectura desde fuera.
- Docente asociado a Curso como objeto, en lugar de guardar solo su nombre.
- Captura de errores por operación del menú; lectura numérica controlada y salida por EOF.
- Diagrama actualizado y documentación de decisiones/pedientes para Fase 2.
- Bocetos originales preservados; navegación añadida, colores de calificaciones armonizados,
  listado de estudiantes y formulario de estudiante complementarios.
- README original íntegro y ZIP original conservados con procedencia y créditos.

No se inventaron integrantes, videos, historial de commits ni requisitos de Fase 2.
El CUM sigue siendo un dato del prototipo que se actualiza explícitamente; no se calcula
automáticamente desde las notas. La fórmula ponderada debe definirse con el equipo/docente.
