using Armadillo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Armadillo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Programa> Programa { get; set; }
        public virtual DbSet<Hoja> Hoja { get; set; }
        public virtual DbSet<Tipo> Tipo { get; set; }
        public virtual DbSet<Campo> Campo { get; set; }
        public virtual DbSet<Dato> Dato { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUserLogin<int>>()
            //.Property(login => login.UserId)
            //.ForMySQLHasColumnType("PK")
            //.UseSqlServerIdentityColumn()
            //.UseMySQLAutoIncrementColumn("AI");

            modelBuilder.Entity<Hoja>()
                    .HasOne(d => d.Programa)
                    .WithMany(p => p.Hojas)
                    .HasForeignKey(d => d.Id);

            modelBuilder.Entity<Campo>()
                    .HasOne(d => d.Hoja)
                    .WithMany(p => p.Campos)
                    .HasForeignKey(d => d.Id);

            modelBuilder.Entity<Campo>()
                    .HasOne(d => d.Tipo)
                    .WithMany(p => p.Campos)
                    .HasForeignKey(d => d.Id);

            modelBuilder.Entity<Dato>()
                    .HasOne(d => d.Campo)
                    .WithMany(p => p.Datos)
                    .HasForeignKey(d => d.Id);

            /*Agregar los tipos de datos*/
            List<Tipo> tipos =new List<Tipo>();
            tipos.Append(new Models.Tipo { Nombre="Texto"});
            tipos.Append(new Models.Tipo { Nombre = "Número" });
            tipos.Append(new Models.Tipo { Nombre = "Fecha" });
            tipos.Append(new Models.Tipo { Nombre = "Cálculo" });
            tipos.Append(new Models.Tipo { Nombre = "Lista" });

            modelBuilder.Entity<Tipo>().HasData(tipos);

        }
    }
}