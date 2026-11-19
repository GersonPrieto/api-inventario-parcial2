Imports System
Imports ApiInventario.Data
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Migrations
    <DbContextAttribute(GetType(AppDbContext))>
    <MigrationAttribute("20261119013939_Inicial")>
    Public Partial Class Inicial
        Protected Overrides Sub BuildTargetModel(modelBuilder As ModelBuilder)
            modelBuilder.HasAnnotation("ProductVersion", "10.0.15").HasAnnotation("Relational:MaxIdentifierLength", 128)
            modelBuilder.UseIdentityColumns()

            modelBuilder.Entity("ApiInventario.Models.Categoria",
                Sub(b)
                    b.Property(Of Integer)("Id").ValueGeneratedOnAdd().HasColumnType("int")
                    b.Property(Of Integer)("Id").UseIdentityColumn()
                    b.Property(Of String)("Descripcion").HasMaxLength(200).HasColumnType("nvarchar(200)")
                    b.Property(Of String)("Nombre").IsRequired().HasMaxLength(50).
                        HasColumnType("nvarchar(50)").UseCollation("Latin1_General_100_CI_AS")
                    b.HasKey("Id")
                    b.HasIndex("Nombre").IsUnique()
                    b.ToTable("Categorias", CType(Nothing, String))
                End Sub)

            modelBuilder.Entity("ApiInventario.Models.Producto",
                Sub(b)
                    b.Property(Of Integer)("Id").ValueGeneratedOnAdd().HasColumnType("int")
                    b.Property(Of Integer)("Id").UseIdentityColumn()
                    b.Property(Of Integer)("CategoriaId").HasColumnType("int")
                    b.Property(Of String)("Descripcion").HasMaxLength(200).HasColumnType("nvarchar(200)")
                    b.Property(Of DateTime)("FechaRegistro").HasColumnType("datetime2")
                    b.Property(Of String)("Nombre").IsRequired().HasMaxLength(50).
                        HasColumnType("nvarchar(50)").UseCollation("Latin1_General_100_CI_AS")
                    b.Property(Of Decimal)("Precio").HasPrecision(18, 2).HasColumnType("decimal(18,2)")
                    b.Property(Of Integer)("Stock").HasColumnType("int")
                    b.HasKey("Id")
                    b.HasIndex("CategoriaId")
                    b.HasIndex("Nombre").IsUnique()
                    b.ToTable("Productos", CType(Nothing, String),
                        Sub(t)
                            t.HasCheckConstraint("CK_Productos_Precio", "[Precio] > 0")
                            t.HasCheckConstraint("CK_Productos_Stock", "[Stock] >= 0")
                        End Sub)
                End Sub)

            modelBuilder.Entity("ApiInventario.Models.Producto",
                Sub(b)
                    b.HasOne("ApiInventario.Models.Categoria", "Categoria").WithMany("Productos").
                        HasForeignKey("CategoriaId").OnDelete(DeleteBehavior.Restrict).IsRequired()
                    b.Navigation("Categoria")
                End Sub)
            modelBuilder.Entity("ApiInventario.Models.Categoria", Sub(b) b.Navigation("Productos"))
        End Sub
    End Class
End Namespace
