using ControlAcademico.Modelos;
using ControlAcademico.Servicios;

int aprobadas = 0, fallidas = 0;
void Prueba(string nombre, Action verificar)
{
    try { verificar(); aprobadas++; Console.WriteLine("OK " + nombre); }
    catch (Exception ex) { fallidas++; Console.WriteLine("FALLO " + nombre + ": " + ex.Message); }
}
void Igual<T>(T actual, T esperado) { if (!EqualityComparer<T>.Default.Equals(actual, esperado)) throw new Exception($"Esperado {esperado}, recibido {actual}"); }
void Rechaza(Action operacion)
{
    try { operacion(); } catch (Exception e) when (e is ArgumentException or InvalidOperationException) { return; }
    throw new Exception("La operación inválida fue aceptada.");
}
(ControlAcademicoService s, Estudiante e, Curso c) Preparar(int cupo = 2)
{
    var s = new ControlAcademicoService();
    var d = new Docente(1,"Carlos","Martínez","carlos@example.edu","7000",new DateTime(1980,1,1),"DOC-1","Sistemas");
    var e = new Estudiante(2,"Ana","López","ana@example.edu","7000",new DateTime(2000,1,1),"CT-1","Sistemas","Ingeniería");
    s.RegistrarPersona(d);s.RegistrarPersona(e);
    var c = new Curso("DSP404","Curso",4,d,cupo);s.RegistrarCurso(c);
    return (s,e,c);
}
Prueba("Matrícula actualiza cupos, materias y colección", () => {var(s,e,c)=Preparar();s.Matricular("M1","ct-1","dsp404");Igual(c.Inscritos,1);Igual(e.MateriasInscritas.Single(),"DSP404");Igual(s.Matriculas.Count,1);});
Prueba("Duplicado no cambia ningún estado", () => {var(s,e,c)=Preparar();s.Matricular("M1","CT-1","DSP404");Rechaza(()=>s.Matricular("M2","CT-1","DSP404"));Igual(c.Inscritos,1);Igual(e.MateriasInscritas.Count,1);Igual(s.Matriculas.Count,1);});
Prueba("Sin cupo rechaza segundo estudiante sin efectos parciales", () => {var(s,e,c)=Preparar(1);s.Matricular("M1","CT-1","DSP404");var otro=new Estudiante(3,"Juan","Pérez","juan@example.edu","7000",new DateTime(2000,1,1),"CT-2","Sistemas","Ingeniería");s.RegistrarPersona(otro);Rechaza(()=>s.Matricular("M2","CT-2","DSP404"));Igual(otro.MateriasInscritas.Count,0);Igual(c.Inscritos,1);Igual(s.Matriculas.Count,1);});
Prueba("Cancelar libera cupo y permite rematricular", () => {var(s,e,c)=Preparar(1);s.Matricular("M1","CT-1","DSP404");s.CancelarMatricula("m1");Igual(c.Inscritos,0);Igual(e.MateriasInscritas.Count,0);s.Matricular("M2","CT-1","DSP404");Igual(c.Inscritos,1);Rechaza(()=>s.CancelarMatricula("M1"));Igual(c.Inscritos,1);});
Prueba("Nota negativa, excesiva, NaN e infinito se rechazan sin modificar nota", () => {var(s,e,c)=Preparar();s.Matricular("M1","CT-1","DSP404");var n=s.RegistrarCalificacion(1,"M1",8);foreach(var v in new[]{-1d,25d,double.NaN,double.PositiveInfinity}){Rechaza(()=>n.ActualizarNota(v));Igual(n.Nota,8d);}});
Prueba("Límites de aprobación y rango", () => {var(s,e,c)=Preparar();s.Matricular("M1","CT-1","DSP404");var n=s.RegistrarCalificacion(1,"M1",0);Igual(n.EstaAprobado(),false);n.ActualizarNota(6);Igual(n.EstaAprobado(),true);n.ActualizarNota(10);Igual(n.Nota,10d);});
Prueba("No permite calificar sin matrícula ni duplicar nota", () => {var(s,e,c)=Preparar();Rechaza(()=>s.RegistrarCalificacion(1,"NO-EXISTE",8));s.Matricular("M1","CT-1","DSP404");s.RegistrarCalificacion(1,"M1",8);Rechaza(()=>s.RegistrarCalificacion(2,"M1",7));Rechaza(()=>s.CancelarMatricula("M1"));Igual(c.Inscritos,1);});
Prueba("Búsqueda ignora espacios y mayúsculas; null no falla", () => {var(s,e,c)=Preparar();Igual(s.BuscarEstudiante(" ct-1 "),e);Igual(s.BuscarEstudiante(null),null);});
Prueba("ID de matrícula duplicado no cambia estado", () => {var(s,e,c)=Preparar();s.Matricular("M1","CT-1","DSP404");Rechaza(()=>s.Matricular(" m1 ","CT-1","DSP404"));Igual(s.Matriculas.Count,1);});
Prueba("Cupo no puede bajar por debajo de inscritos", () => {var(s,e,c)=Preparar();s.Matricular("M1","CT-1","DSP404");s.RegistrarPersona(new Estudiante(3,"Juan","Pérez","juan@example.edu","7000",new DateTime(2000,1,1),"CT-2","Sistemas","Ingeniería"));s.Matricular("M2","CT-2","DSP404");Rechaza(()=>c.CambiarCupo(1));Igual(c.CupoMaximo,2);Igual(c.Inscritos,2);});
Prueba("Persona inválida rechazada", () => {Rechaza(()=>new Estudiante(1," ","López","ana@example.edu","7000",new DateTime(2000,1,1),"CT-1","Sistemas","Ingeniería"));Rechaza(()=>new Estudiante(1,"Ana","López","incorrecto","7000",new DateTime(2000,1,1),"CT-1","Sistemas","Ingeniería"));Rechaza(()=>new Estudiante(1,"Ana","López","ana@example.edu","7000",DateTime.Today.AddDays(1),"CT-1","Sistemas","Ingeniería"));});
Prueba("Identificadores de personas no se duplican", () => {var(s,e,c)=Preparar();Rechaza(()=>s.RegistrarPersona(new Estudiante(2,"Otra","Persona","otra@example.edu","7000",new DateTime(2000,1,1),"CT-2","Sistemas","Ingeniería")));Rechaza(()=>s.RegistrarPersona(new Estudiante(3,"Otra","Persona","otra@example.edu","7000",new DateTime(2000,1,1)," ct-1 ","Sistemas","Ingeniería")));Igual(s.Personas.Count,2);});
Prueba("CUM no acepta valores inválidos", () => {var(s,e,c)=Preparar();Rechaza(()=>e.ActualizarCUM(11));Igual(e.CUM,0d);e.ActualizarCUM(5);Igual(e.EstadoAcademico,"En Riesgo");e.ActualizarCUM(7);Igual(e.EstadoAcademico,"Activo");});
Prueba("Colecciones públicas no permiten añadir registros", () => {var(s,e,c)=Preparar();Igual(((ICollection<Persona>)s.Personas).IsReadOnly,true);Igual(((ICollection<string>)e.MateriasInscritas).IsReadOnly,true);});
Console.WriteLine($"Resultado: {aprobadas} aprobadas; {fallidas} fallidas.");
Environment.ExitCode = fallidas == 0 ? 0 : 1;
