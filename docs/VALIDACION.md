# Verificación de la entrega

Fecha: 21 de octubre de 2026. Entorno local: Windows y SDK .NET 10.0.401.

## Comandos

```sh
dotnet build ControlAcademico.slnx --configuration Release --disable-build-servers -m:1
dotnet run --project tests/ControlAcademico.Pruebas --configuration Release --no-build
```

La compilación con un único nodo evita el fallo del servidor de compilación observado
en este entorno. No requiere cambios en las fuentes ni dependencias adicionales.

## Cobertura

- Matrícula actualiza colección, materias y cupo.
- Matrícula duplicada o sin cupo no deja cambios parciales.
- Cancelación libera cupo y permite una matrícula nueva.
- Notas fuera de rango, NaN e infinito se rechazan conservando el valor anterior.
- Límites 0, 6 y 10 de la nota y aprobación.
- Calificación requiere matrícula activa y no se duplica.
- Matrículas con nota no se cancelan.
- Búsqueda tolera mayúsculas, espacios y entrada nula.
- ID de matrícula y datos de personas no se duplican.
- Cupo no puede bajar por debajo de inscritos.
- Nombre, correo y fecha inválidos se rechazan.
- CUM validado y colecciones protegidas contra escritura externa.

Resultado: **14 casos aprobados, 0 fallidos**. Compilación sin errores ni advertencias.
Además se ejecutó el menú con entradas redirigidas para comprobar búsqueda, rechazo
de nota 25 sin perder el menú y actualización de nota con separador decimal coma.

## Límites

Estas pruebas verifican el dominio y el prototipo de consola. No verifican ASP.NET,
persistencia, concurrencia entre solicitudes, visualización en todos los navegadores,
ni el flujo de GitHub Actions en un repositorio remoto todavía no creado.
Los enlaces internos HTML se comprobaron por existencia de sus destinos.
