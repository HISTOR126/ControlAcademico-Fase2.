"""Pruebas HTTP con SQLite temporal. Ejecutar después de compilar en Release."""
from pathlib import Path
from urllib.request import build_opener, HTTPCookieProcessor, Request
from urllib.parse import urlencode
from urllib.error import HTTPError
from http.cookiejar import CookieJar
from html.parser import HTMLParser
import subprocess, tempfile, socket, time, os, json

raiz=Path(__file__).resolve().parents[1]
dll=raiz/'src/ControlAcademico.Web/bin/Release/net10.0/ControlAcademico.Web.dll'
assert dll.is_file(), 'Compile la solución en Release antes de ejecutar esta prueba.'
sock=socket.socket();sock.bind(('127.0.0.1',0));puerto=sock.getsockname()[1];sock.close()
url=f'http://127.0.0.1:{puerto}'
cliente=build_opener(HTTPCookieProcessor(CookieJar()))
class Inputs(HTMLParser):
    def __init__(self):super().__init__();self.datos={}
    def handle_starttag(self,tag,attrs):
        d=dict(attrs)
        if tag=='input' and d.get('name'):self.datos[d['name']]=d.get('value','')
def consultar(ruta,datos=None):
    r=Request(url+ruta,data=urlencode(datos).encode() if datos is not None else None)
    try:
        with cliente.open(r,timeout=15) as x:return x.status,x.read().decode(),x.url
    except HTTPError as e:return e.code,e.read().decode(),e.url
def campos(ruta):
    estado,texto,_=consultar(ruta);assert estado==200,(ruta,estado,texto[:300]);p=Inputs();p.feed(texto);return p.datos
def guardar(modulo,datos,id=0):
    f=campos(f'/academico/{modulo}/{id}/editar' if id else f'/academico/{modulo}/nuevo');f.update(datos)
    estado,texto,final=consultar(f'/academico/{modulo}/guardar',f)
    assert estado==200,(modulo,estado,texto)
    return int(final.rsplit('/',1)[1]),texto

carpeta_temporal=raiz/'tests'/'.tmp'
carpeta_temporal.mkdir(exist_ok=True)
with tempfile.TemporaryDirectory(prefix='academico-http-',dir=carpeta_temporal) as temporal:
    assert Path(temporal).resolve().is_relative_to(carpeta_temporal.resolve())
    entorno=os.environ.copy();entorno['Academico__Archivo']=str(Path(temporal)/'test.db');entorno['Academico__SembrarDatos']='true'
    with open(Path(temporal)/'servidor.log','w',encoding='utf-8') as log:
        servidor=subprocess.Popen(['dotnet',str(dll),'--urls',url,'--contentRoot',str(raiz/'src/ControlAcademico.Web')],env=entorno,stdout=log,stderr=log)
        try:
            for intento in range(60):
                try:
                    if consultar('/')[0]==200:break
                except OSError:time.sleep(.2)
            else:raise AssertionError('El servidor no inició.')
            for m in ['estudiantes','docentes','cursos','matriculas','calificaciones']:
                for ruta in [f'/academico/{m}',f'/academico/{m}/nuevo',f'/academico/{m}/1',f'/academico/{m}/1/editar',f'/academico/{m}/1/eliminar']:
                    assert consultar(ruta)[0]==200,ruta
            print('OK 25 páginas de listado, formulario, detalle y eliminación')
            assert consultar('/academico/estudiantes/guardar',{'Codigo':'SIN-TOKEN'})[0]==400
            print('OK POST sin antiforgery rechazado')
            payload={'Codigo':'QA-HTTP','Nombre':'Luisa','Apellido':'Prueba','Email':'luisa@example.edu','Telefono':'7000','FechaNacimiento':'2001-01-01','Carrera':'Sistemas','Facultad':'Ingeniería','CicloActual':'2','CUM':'8.25'}
            alumno,detalle=guardar('estudiantes',payload);assert '8.25' in detalle
            print('OK alta web con CUM decimal')
            servidor.terminate();servidor.wait(timeout=15)
            servidor=subprocess.Popen(['dotnet',str(dll),'--urls',url,'--contentRoot',str(raiz/'src/ControlAcademico.Web')],env=entorno,stdout=log,stderr=log)
            for intento in range(60):
                try:
                    if consultar('/')[0]==200:break
                except OSError:time.sleep(.2)
            else:raise AssertionError('El servidor no reinició.')
            assert 'Luisa' in consultar(f'/academico/estudiantes/{alumno}')[1]
            print('OK datos conservados después de reiniciar el proceso web')
            payload['Nombre']='Luisa Editada';guardar('estudiantes',payload,alumno)
            assert 'Luisa Editada' in consultar(f'/academico/estudiantes/{alumno}')[1]
            print('OK edición y detalle web')
            formulario=campos('/academico/estudiantes/nuevo');formulario.update(payload)
            assert consultar('/academico/estudiantes/guardar',formulario)[0]==422
            print('OK duplicado rechazado en servidor')
            matricula,_=guardar('matriculas',{'Codigo':'QA-MAT','EstudianteId':alumno,'CursoId':1,'Activa':'true'})
            nota,_=guardar('calificaciones',{'MatriculaId':matricula,'Nota':'9.50'})
            respuesta=json.loads(consultar('/api/estudiantes/qa-http/situacion')[1]);assert respuesta['matriculas'][0]['nota']==9.5
            print('OK matrícula, nota decimal y API JSON con datos persistidos')
            f=campos(f'/academico/calificaciones/{nota}/editar');f.update({'MatriculaId':matricula,'Nota':'25'})
            assert consultar('/academico/calificaciones/guardar',f)[0]==422
            print('OK validación de nota en servidor')
            assert consultar('/api/estudiantes/NO-EXISTE/situacion')[0]==404
            assert consultar('/academico/estudiantes/99999')[0]==404
            assert consultar('/academico/desconocido')[0]==404
            print('OK registros y módulos inexistentes retornan 404')
            for m,i in [('calificaciones',nota),('matriculas',matricula),('estudiantes',alumno)]:
                f=campos(f'/academico/{m}/{i}/eliminar');assert consultar(f'/academico/{m}/{i}/eliminar',f)[0]==200
                assert consultar(f'/academico/{m}/{i}')[0]==404
            print('OK eliminación web de nota, matrícula y estudiante')
            print('Resultado: todas las comprobaciones HTTP aprobadas.')
        finally:
            servidor.terminate();servidor.wait(timeout=15)
