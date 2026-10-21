# Verificación técnica de fase 2

Fecha: 21 de octubre de 2026. Windows, SDK .NET 10.0.401 y EF Core SQLite 10.0.12.

- Solución completa en Release: 0 errores, 0 advertencias.
- 14 casos del dominio de fase 1: aprobados.
- 22 comprobaciones de integración con SQLite: aprobadas. Incluyen CRUD de las cinco
  entidades, relaciones, cupos, duplicados, cancelación, notas, rollback y versiones.
- Pruebas HTTP: 25 páginas accesibles; alta/edición/detalle/eliminación; CUM y notas
  decimales; validaciones de servidor; POST sin token rechazado; 404; API JSON;
  datos conservados después de reiniciar el proceso web. Todas aprobadas.
- Prueba visual: formulario de estudiante guardado desde navegador y detalle visible;
  revisión de panel, listado y relaciones de curso. Capturas reales en docs/capturas.

Los proyectos de pruebas son ejecutables y se ejecutan con `dotnet run`, no `dotnet test`.
La prueba HTTP crea y elimina su propia base temporal bajo tests/.tmp.
No se eliminan ni reinicializan datos de una base de usuario para ejecutar las pruebas.

Para repetir, usar los comandos del README. En el entorno de preparación se empleó
`--disable-build-servers -m:1` para evitar limitaciones del servidor local de compilación.

No se ha verificado despliegue público, todos los navegadores/dispositivos ni una carga
masiva multiusuario. Las capturas reflejan el área visible del navegador de prueba.
El estado de GitHub Actions se debe comprobar en el repositorio remoto después de subir.
