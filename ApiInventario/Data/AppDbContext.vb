Imports Microsoft.EntityFrameworkCore
Imports ApiInventario.Models

Namespace Data
    Public Class AppDbContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of AppDbContext))
            MyBase.New(options)
        End Sub

        Public Property Categorias As DbSet(Of Categoria)
        Public Property Productos As DbSet(Of Producto)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            MyBase.OnModelCreating(modelBuilder)

            Dim categoriaConfig = modelBuilder.Entity(Of Categoria)()
            categoriaConfig.ToTable("Categorias")
            categoriaConfig.HasKey(Function(c) c.Id)
            categoriaConfig.Property(Function(c) c.Id).ValueGeneratedOnAdd()
            categoriaConfig.Property(Function(c) c.Nombre).
                IsRequired().HasMaxLength(50).UseCollation("Latin1_General_100_CI_AS")
            categoriaConfig.Property(Function(c) c.Descripcion).HasMaxLength(200).IsRequired(False)
            categoriaConfig.HasIndex(Function(c) c.Nombre).IsUnique()

            Dim productoConfig = modelBuilder.Entity(Of Producto)()
            productoConfig.ToTable("Productos",
                Sub(tabla)
                    tabla.HasCheckConstraint("CK_Productos_Precio", "[Precio] > 0")
                    tabla.HasCheckConstraint("CK_Productos_Stock", "[Stock] >= 0")
                End Sub)
            productoConfig.HasKey(Function(p) p.Id)
            productoConfig.Property(Function(p) p.Id).ValueGeneratedOnAdd()
            productoConfig.Property(Function(p) p.Nombre).
                IsRequired().HasMaxLength(50).UseCollation("Latin1_General_100_CI_AS")
            productoConfig.Property(Function(p) p.Descripcion).HasMaxLength(200).IsRequired(False)
            productoConfig.Property(Function(p) p.Precio).HasPrecision(18, 2).IsRequired()
            productoConfig.Property(Function(p) p.Stock).IsRequired()
            productoConfig.Property(Function(p) p.FechaRegistro).HasColumnType("datetime2").IsRequired()
            productoConfig.HasIndex(Function(p) p.Nombre).IsUnique()
            productoConfig.HasOne(Function(p) p.Categoria).
                WithMany(Function(c) c.Productos).
                HasForeignKey(Function(p) p.CategoriaId).
                IsRequired().OnDelete(DeleteBehavior.Restrict)
        End Sub
    End Class
End Namespace
