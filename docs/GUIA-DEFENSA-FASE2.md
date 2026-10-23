# Guía para ejecutar y defender el proyecto

## Preparación

Instalar SDK .NET 10, descargar el código y abrir la solución. Restaurar, compilar
y ejecutar la web con los comandos del README. Abrir http://localhost:5188 en Chrome.
La primera restauración necesita Internet. El archivo SQLite se crea localmente;
las demostraciones no requieren servidor de base de datos ni Figma.

## Recorrido de demostración

1. Mostrar dashboard y relacionar sus contadores con listados reales.
2. Crear un docente de prueba con código único, nombres, correo, teléfono,
   nacimiento y especialidad. Mostrar detalle y editar especialidad.
3. Crear estudiante con carnet único. Probar correo inválido y después datos válidos.
4. Crear curso para el docente con cupo 1 y créditos positivos.
5. Matricular al estudiante. Intentar duplicar matrícula y superar el cupo con otro alumno.
6. Mostrar curso con inscritos y relaciones. Registrar nota 8.5 y editarla a 9.
7. Intentar nota 25 desde un formulario o demostrar la prueba de servidor; se rechaza.
8. Mostrar /api/estudiantes/CARNET/situacion y explicar la respuesta JSON.
9. Cerrar el servidor, volver a iniciarlo y mostrar que los datos se conservan.
10. Intentar eliminar curso con matrícula: explicar protección de relaciones.
11. Eliminar nota, matrícula, curso y personas de prueba en orden; demostrar CRUD.
12. Mostrar solución, capas, contexto EF, contrato, transacciones y pruebas.

## Qué debe comprender el equipo

- Diferencia entre la consola en memoria y la web con persistencia.
- Qué significa herencia, encapsulamiento e interfaz en las clases de fase 1.
- Por qué la presentación llama al negocio y no escribe SQL.
- Cómo EF Core convierte los registros en tablas y relaciones.
- Por qué una transacción evita dejar operaciones incompletas.
- Diferencia entre cancelar una matrícula y eliminar un registro.
- Cómo se evitan duplicados, notas inválidas y actualizaciones obsoletas.
- Qué hace el endpoint GET y cómo responde si el estudiante no existe.
- Qué contienen las pruebas de dominio, SQLite y HTTP.

## Distribución de preparación sugerida

Asignar entre los integrantes: entidades y continuidad; base de datos y EF; reglas
de matrículas/notas; controladores, validación y vistas; API, pruebas y documentación.
Esta es una propuesta de estudio, no evidencia de autoría o trabajo ya realizado.
Todos deben conocer el flujo completo y defender su parte presencialmente.

## Antes de entregar

Confirmar repositorio exigido por el docente, integrantes y carnets; revisar PDF,
capturas e instrucciones; ejecutar pruebas; comprobar que todos puedan iniciar la
web en sus equipos. Entregar en aula virtual y asistir a la defensa presencial.
El documento señala 22 de octubre como fecha de entrega.
