using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BluesoftBankAPI.Models
{
    public partial class BluesoftBankContext : DbContext
    {
        public BluesoftBankContext()
        {
        }

        public BluesoftBankContext(DbContextOptions<BluesoftBankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ciudad> Ciudads { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<ClienteCuenta> ClienteCuenta { get; set; } = null!;
        public virtual DbSet<Cuenta> Cuenta { get; set; } = null!;
        public virtual DbSet<Movimiento> Movimientos { get; set; } = null!;
        public virtual DbSet<TipoCliente> TipoClientes { get; set; } = null!;
        public virtual DbSet<TipoCuenta> TipoCuenta { get; set; } = null!;
        public virtual DbSet<TipoMovimiento> TipoMovimientos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.ToTable("Ciudad");

                entity.Property(e => e.NombreCiudad)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Papellido)
                    .HasMaxLength(20)
                    .HasColumnName("PApellido")
                    .IsFixedLength();

                entity.Property(e => e.Pnombre)
                    .HasMaxLength(20)
                    .HasColumnName("PNombre")
                    .IsFixedLength();

                entity.Property(e => e.Sapellido)
                    .HasMaxLength(20)
                    .HasColumnName("SApellido")
                    .IsFixedLength();

                entity.Property(e => e.Snombre)
                    .HasMaxLength(20)
                    .HasColumnName("SNombre")
                    .IsFixedLength();

                entity.Property(e => e.Snombre)
                    .HasMaxLength(20)
                    .HasColumnName("SNombre")
                    .IsFixedLength();

                entity.Property(e => e.NumeroIdentificador)
                    .HasMaxLength(50)
                    .HasColumnName("NumeroIdentificador");

                //entity.HasOne(d => d.IdTipoClienteNavigation)
                //    .WithMany(p => p.Clientes)
                //    .HasForeignKey(d => d.IdTipoCliente)
                //    .HasConstraintName("FK_Cliente_TipoCliente");
            });

            modelBuilder.Entity<ClienteCuenta>(entity =>
            {
                entity.ToTable("Cliente_Cuenta");

                entity.Property(e => e.IdCliente)
                    .HasColumnName("IdCliente");

                entity.Property(e => e.IdCuenta)
                    .HasColumnName("IdCuenta");

                //entity.HasOne(d => d.IdClienteNavigation)
                //    .WithMany(p => p.ClienteCuenta)
                //    .HasForeignKey(d => d.IdCliente)
                //    .HasConstraintName("FK_Cliente_Cuenta_Cliente");

                //entity.HasOne(d => d.IdCuentaNavigation)
                //    .WithMany(p => p.ClienteCuenta)
                //    .HasForeignKey(d => d.IdCuenta)
                //    .HasConstraintName("FK_Cliente_Cuenta_Cuenta");
            });

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.NumeroCuenta)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.SaldoCuenta)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.IdCiudad)
                    .HasColumnName("IdCiudad");

                entity.Property(e => e.IdTipoCuenta)
                    .HasColumnName("IdTipoCuenta");

                //entity.HasOne(d => d.IdCiudadNavigation)
                //    .WithMany(p => p.Cuenta)
                //    .HasForeignKey(d => d.IdCiudad)
                //    .HasConstraintName("FK_Cuenta_Ciudad");

                //entity.HasOne(d => d.IdTipoCuentaNavigation)
                //    .WithMany(p => p.Cuenta)
                //    .HasForeignKey(d => d.IdTipoCuenta)
                //    .HasConstraintName("FK_Cuenta_TipoCuenta");
            });

            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.ToTable("Movimiento");

                entity.Property(e => e.Monto)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.IdMovimiento)
                    .HasColumnName("IdMovimiento");

                entity.Property(e => e.IdCuenta)
                    .HasColumnName("IdCuenta");

                entity.Property(e => e.IdCiudad)
                    .HasColumnName("IdCiudad");

                entity.Property(e => e.FechaMovimiento).HasColumnType("FechaMovimiento");

                //entity.HasOne(d => d.IdCiudadNavigation)
                //    .WithMany(p => p.Movimientos)
                //    .HasForeignKey(d => d.IdCiudad)
                //    .HasConstraintName("FK_Movimiento_Ciudad");

                //entity.HasOne(d => d.IdCuentaNavigation)
                //    .WithMany(p => p.Movimientos)
                //    .HasForeignKey(d => d.IdCuenta)
                //    .HasConstraintName("FK_Movimiento_Cuenta");

                //entity.HasOne(d => d.IdMovimientoNavigation)
                //    .WithMany(p => p.Movimientos)
                //    .HasForeignKey(d => d.IdMovimiento)
                //    .HasConstraintName("FK_Movimiento_TipoMovimiento");
            });

            modelBuilder.Entity<TipoCliente>(entity =>
            {
                entity.ToTable("TipoCliente");

                entity.Property(e => e.TipoCliente1)
                    .HasMaxLength(50)
                    .HasColumnName("TipoCliente")
                    .IsFixedLength();
            });

            modelBuilder.Entity<TipoCuenta>(entity =>
            {
                entity.HasKey(e => e.IdTc);

                entity.Property(e => e.IdTc).HasColumnName("IdTC");

                entity.Property(e => e.TipoCuentaN)
                    .HasMaxLength(50)
                    .HasColumnName("TipoCuenta")
                    .IsFixedLength();
            });

            modelBuilder.Entity<TipoMovimiento>(entity =>
            {
                entity.ToTable("TipoMovimiento");

                entity.Property(e => e.NombreMovimiento)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        internal object Query<T>()
        {
            throw new NotImplementedException();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
