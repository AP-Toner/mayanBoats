using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace mayanBoats.Models
{
    public partial class MayanDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MayanDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MayanDbContext(DbContextOptions<MayanDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Cupone> Cupones { get; set; }
        public virtual DbSet<HistoriaPaquete> HistoriaPaquetes { get; set; }
        public virtual DbSet<Hora> Horas { get; set; }
        public virtual DbSet<Paquete> Paquetes { get; set; }
        public virtual DbSet<Pregunta> Preguntas { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Renta> Rentas { get; set; }
        public virtual DbSet<TipoProducto> TipoProductos { get; set; }
        public virtual DbSet<TransaccionPaypal> TransaccionPaypals { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Clientes");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cupone>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Cupones");

                entity.Property(e => e.Cupon)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.Descuento).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<HistoriaPaquete>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("HistoriaPaquetes");

                entity.Property(e => e.FechaCaptura)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Paquete)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Hora>(entity =>
            {
                entity.HasKey(e => e.IdHora);
                entity.ToTable("Horas");

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);
                entity.Property(e => e.Hora1)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Hora");
            });

            modelBuilder.Entity<Paquete>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Paquetes");

                entity.Property(e => e.Activo).HasColumnName("activo");
                entity.Property(e => e.FechaCaptura)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Paquete1).HasColumnName("Paquete");
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Preguntas");

                entity.Property(e => e.Activo).HasColumnName("activo");
                entity.Property(e => e.Asunto)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Correo)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.FechaCaptura)
                    .HasMaxLength(40)
                    .IsUnicode(false);
                entity.Property(e => e.FechaRespuesta)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValue("01/01/1980");
                entity.Property(e => e.Mensaje).IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Respuesta).IsUnicode(false);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Producto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Renta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Rentas");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.CostoAdicional).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);
                entity.Property(e => e.FechaCaptura).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.HoraFinal)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.HoraInicial)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.IdPagoPayPal)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.Monto).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Telefono)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("TipoProducto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransaccionPaypal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("TransaccionPaypal");

                entity.Property(e => e.Accion)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.BeneficiarioCorreo)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.BeneficiarioId)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.CompraMoneda)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.CompradorApellido)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.CompradorCodigoPais)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.CompradorCorreo)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.CompradorId)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.CompradorNombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);
                entity.Property(e => e.FechaCaptura)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.FechaCreacion)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.IdPaypal)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.PagoEstatus)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("usuarios");

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
