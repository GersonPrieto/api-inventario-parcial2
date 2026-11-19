Imports System
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Migrations
    ' Esquema generado a partir de AppDbContext con EF Core 10 y adaptado a VB.
    Public Partial Class Inicial
        Inherits Migration

        Protected Overrides Sub Up(migrationBuilder As MigrationBuilder)
            migrationBuilder.CreateTable(
                name:="Categorias",
                columns:=Function(tabla) New With {
                    .Id = tabla.Column(Of Integer)(type:="int", nullable:=False).
                        Annotation("SqlServer:Identity", "1, 1"),
                    .Nombre = tabla.Column(Of String)(type:="nvarchar(50)", maxLength:=50,
                        nullable:=False, collation:="Latin1_General_100_CI_AS"),
                    .Descripcion = tabla.Column(Of String)(type:="nvarchar(200)", maxLength:=200, nullable:=True)
                },
                constraints:=Sub(tabla)
                                 tabla.PrimaryKey("PK_Categorias", Function(x) x.Id)
                             End Sub)

            migrationBuilder.CreateTable(
                name:="Productos",
                columns:=Function(tabla) New With {
                    .Id = tabla.Column(Of Integer)(type:="int", nullable:=False).
                        Annotation("SqlServer:Identity", "1, 1"),
                    .Nombre = tabla.Column(Of String)(type:="nvarchar(50)", maxLength:=50,
                        nullable:=False, collation:="Latin1_General_100_CI_AS"),
                    .Descripcion = tabla.Column(Of String)(type:="nvarchar(200)", maxLength:=200, nullable:=True),
                    .Precio = tabla.Column(Of Decimal)(type:="decimal(18,2)", precision:=18, scale:=2, nullable:=False),
                    .Stock = tabla.Column(Of Integer)(type:="int", nullable:=False),
                    .FechaRegistro = tabla.Column(Of DateTime)(type:="datetime2", nullable:=False),
                    .CategoriaId = tabla.Column(Of Integer)(type:="int", nullable:=False)
                },
                constraints:=Sub(tabla)
                                 tabla.PrimaryKey("PK_Productos", Function(x) x.Id)
                                 tabla.CheckConstraint("CK_Productos_Precio", "[Precio] > 0")
                                 tabla.CheckConstraint("CK_Productos_Stock", "[Stock] >= 0")
                                 tabla.ForeignKey(
                                     name:="FK_Productos_Categorias_CategoriaId",
                                     column:=Function(x) x.CategoriaId,
                                     principalTable:="Categorias", principalColumn:="Id",
                                     onDelete:=ReferentialAction.Restrict)
                             End Sub)

            migrationBuilder.CreateIndex(name:="IX_Categorias_Nombre", table:="Categorias", column:="Nombre", unique:=True)
            migrationBuilder.CreateIndex(name:="IX_Productos_CategoriaId", table:="Productos", column:="CategoriaId")
            migrationBuilder.CreateIndex(name:="IX_Productos_Nombre", table:="Productos", column:="Nombre", unique:=True)
        End Sub

        Protected Overrides Sub Down(migrationBuilder As MigrationBuilder)
            migrationBuilder.DropTable(name:="Productos")
            migrationBuilder.DropTable(name:="Categorias")
        End Sub
    End Class
End Namespace
