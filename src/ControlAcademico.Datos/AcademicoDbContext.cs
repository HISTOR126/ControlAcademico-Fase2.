using ControlAcademico.Persistencia;
using Microsoft.EntityFrameworkCore;
namespace ControlAcademico.Datos;
public sealed class AcademicoDbContext(DbContextOptions<AcademicoDbContext> options) : DbContext(options)
{
 public DbSet<EstudianteRegistro> Estudiantes => Set<EstudianteRegistro>();
 public DbSet<DocenteRegistro> Docentes => Set<DocenteRegistro>();
 public DbSet<CursoRegistro> Cursos => Set<CursoRegistro>();
 public DbSet<MatriculaRegistro> Matriculas => Set<MatriculaRegistro>();
 public DbSet<CalificacionRegistro> Calificaciones => Set<CalificacionRegistro>();
 protected override void OnModelCreating(ModelBuilder b)
 {
  b.Entity<EstudianteRegistro>().ToTable("Estudiantes", t => t.HasCheckConstraint("CK_Estudiante_CUM", "CUM >= 0 AND CUM <= 10"));
  b.Entity<DocenteRegistro>().ToTable("Docentes");
  b.Entity<CursoRegistro>().ToTable("Cursos", t => {t.HasCheckConstraint("CK_Curso_Cupo", "CupoMaximo > 0");t.HasCheckConstraint("CK_Curso_Creditos", "Creditos > 0");});
  b.Entity<MatriculaRegistro>().ToTable("Matriculas");
  b.Entity<CalificacionRegistro>().ToTable("Calificaciones", t => t.HasCheckConstraint("CK_Nota_Rango", "Nota >= 0 AND Nota <= 10"));
  b.Entity<EstudianteRegistro>().HasIndex(e=>e.Carnet).IsUnique();
  b.Entity<DocenteRegistro>().HasIndex(e=>e.CodigoEmpleado).IsUnique();
  b.Entity<CursoRegistro>().HasIndex(e=>e.Codigo).IsUnique();
  b.Entity<MatriculaRegistro>().HasIndex(e=>e.Codigo).IsUnique();
  b.Entity<MatriculaRegistro>().HasIndex(e=>new {e.EstudianteId,e.CursoId}).IsUnique().HasFilter("Activa = 1");
  b.Entity<CalificacionRegistro>().HasIndex(e=>e.MatriculaId).IsUnique();
  b.Entity<CursoRegistro>().HasOne<DocenteRegistro>().WithMany().HasForeignKey(e=>e.DocenteId).OnDelete(DeleteBehavior.Restrict);
  b.Entity<MatriculaRegistro>().HasOne<EstudianteRegistro>().WithMany().HasForeignKey(e=>e.EstudianteId).OnDelete(DeleteBehavior.Restrict);
  b.Entity<MatriculaRegistro>().HasOne<CursoRegistro>().WithMany().HasForeignKey(e=>e.CursoId).OnDelete(DeleteBehavior.Restrict);
  b.Entity<CalificacionRegistro>().HasOne<MatriculaRegistro>().WithMany().HasForeignKey(e=>e.MatriculaId).OnDelete(DeleteBehavior.Restrict);
  foreach(var entity in b.Model.GetEntityTypes())
  {
   b.Entity(entity.ClrType).Property("Version").IsConcurrencyToken();
   foreach(var prop in entity.GetProperties().Where(p=>p.ClrType==typeof(string))) prop.SetMaxLength(200);
  }
 }
}
