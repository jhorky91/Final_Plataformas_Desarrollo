using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Final_Plataformas_De_Desarrollo.Migrations
{
    public partial class Entidades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    idCategoria = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dni = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido = table.Column<string>(type: "varchar(50)", nullable: false),
                    mail = table.Column<string>(type: "varchar(50)", nullable: false),
                    password = table.Column<string>(type: "varchar(50)", nullable: false),
                    cuit_cuil = table.Column<long>(type: "bigint", nullable: false),
                    esAdmin = table.Column<bool>(type: "bit", nullable: false),
                    esEmpresa = table.Column<bool>(type: "bit", nullable: false),
                    intentos = table.Column<int>(type: "int", nullable: false),
                    bloqueado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    idProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.idProducto);
                    table.ForeignKey(
                        name: "FK_Producto_Categoria_idCategoria",
                        column: x => x.idCategoria,
                        principalTable: "Categoria",
                        principalColumn: "idCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carro",
                columns: table => new
                {
                    idCarro = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carro", x => x.idCarro);
                    table.ForeignKey(
                        name: "FK_Carro_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    idCompra = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(nullable: false),
                    total = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.idCompra);
                    table.ForeignKey(
                        name: "FK_Compra_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarroProducto",
                columns: table => new
                {
                    idCarroProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCarro = table.Column<int>(nullable: false),
                    idProducto = table.Column<int>(nullable: false),
                    cantidad = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarroProducto", x => x.idCarroProducto);
                    table.ForeignKey(
                        name: "FK_CarroProducto_Carro_idCarro",
                        column: x => x.idCarro,
                        principalTable: "Carro",
                        principalColumn: "idCarro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarroProducto_Producto_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Producto",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompraProducto",
                columns: table => new
                {
                    idCompraProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCompra = table.Column<int>(nullable: false),
                    idProducto = table.Column<int>(nullable: false),
                    cantidad = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraProducto", x => x.idCompraProducto);
                    table.ForeignKey(
                        name: "FK_CompraProducto_Compra_idCompra",
                        column: x => x.idCompra,
                        principalTable: "Compra",
                        principalColumn: "idCompra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompraProducto_Producto_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Producto",
                        principalColumn: "idProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "idCategoria", "fechaCreacion", "nombre" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 1, 20, 12, 30, 0, 0, DateTimeKind.Unspecified), "Comida" },
                    { 2, new DateTime(2019, 9, 2, 9, 40, 0, 0, DateTimeKind.Unspecified), "Bebida" },
                    { 3, new DateTime(2021, 10, 21, 8, 10, 0, 0, DateTimeKind.Unspecified), "Ropa" },
                    { 4, new DateTime(2021, 11, 22, 7, 58, 0, 0, DateTimeKind.Unspecified), "Articulos de limpieza" },
                    { 5, new DateTime(2020, 12, 25, 3, 1, 0, 0, DateTimeKind.Unspecified), "Electrodomesticos" },
                    { 6, new DateTime(2022, 2, 9, 10, 20, 0, 0, DateTimeKind.Unspecified), "Informatica" },
                    { 7, new DateTime(2021, 7, 30, 10, 20, 0, 0, DateTimeKind.Unspecified), "Herramientas" },
                    { 8, new DateTime(2019, 5, 19, 10, 20, 0, 0, DateTimeKind.Unspecified), "Electronica" },
                    { 9, new DateTime(2020, 4, 12, 15, 20, 0, 0, DateTimeKind.Unspecified), "Mascotas" },
                    { 10, new DateTime(2020, 10, 20, 10, 20, 0, 0, DateTimeKind.Unspecified), "Libreria" }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "idUsuario", "apellido", "bloqueado", "cuit_cuil", "dni", "esAdmin", "esEmpresa", "intentos", "mail", "nombre", "password" },
                values: new object[,]
                {
                    { 1, "Admin", false, 34865218L, 123456, true, false, 0, "admin@gmail.com", "Admin", "123456" },
                    { 2, "Lopez", false, 25689475L, 654321, false, false, 0, "pepitolopez@gmail.com", "Pepito", "654321" },
                    { 3, "Perez", false, 20321548L, 32154869, false, true, 0, "joseperez@hotmail.com", "José", "123456" }
                });

            migrationBuilder.InsertData(
                table: "Carro",
                columns: new[] { "idCarro", "idUsuario" },
                values: new object[,]
                {
                    { 3, 3 },
                    { 2, 2 },
                    { 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "idProducto", "cantidad", "fechaCreacion", "idCategoria", "nombre", "precio" },
                values: new object[,]
                {
                    { 50, 200, new DateTime(2021, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Tablero dibujo", 3000m },
                    { 15, 20, new DateTime(2020, 9, 25, 9, 55, 0, 0, DateTimeKind.Unspecified), 6, "Monitor", 28000m },
                    { 16, 20, new DateTime(2021, 7, 28, 4, 30, 0, 0, DateTimeKind.Unspecified), 6, "Notebook", 95000m },
                    { 32, 150, new DateTime(2019, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Taladro", 15000m },
                    { 33, 150, new DateTime(2021, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Amoladora", 7000m },
                    { 34, 150, new DateTime(2019, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Soldadora", 20000m },
                    { 35, 150, new DateTime(2019, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Sierra", 8000m },
                    { 36, 150, new DateTime(2021, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Hidrolavadora", 7000m },
                    { 7, 150, new DateTime(2020, 12, 19, 10, 30, 0, 0, DateTimeKind.Unspecified), 8, "TV", 80000m },
                    { 37, 150, new DateTime(2019, 4, 25, 4, 3, 0, 0, DateTimeKind.Unspecified), 8, "Parlantes", 10000m },
                    { 38, 150, new DateTime(2021, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Auriculares", 4500m },
                    { 39, 150, new DateTime(2019, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Celular", 50000m },
                    { 41, 200, new DateTime(2020, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Alimento para Perros", 282m },
                    { 42, 200, new DateTime(2021, 7, 10, 21, 3, 0, 0, DateTimeKind.Unspecified), 9, "Alimento para Gatos", 144m },
                    { 43, 200, new DateTime(2021, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Cuchas", 3000m },
                    { 44, 200, new DateTime(2020, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Correas", 1200m },
                    { 45, 200, new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Juguetes", 600m },
                    { 46, 200, new DateTime(2020, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Cuaderno", 250m },
                    { 47, 200, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Marcadores", 1200m },
                    { 48, 200, new DateTime(2021, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Calculadora", 1500m },
                    { 49, 200, new DateTime(2021, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Resma", 700m },
                    { 14, 20, new DateTime(2020, 9, 19, 10, 47, 0, 0, DateTimeKind.Unspecified), 6, "Teclado", 3500m },
                    { 40, 150, new DateTime(2020, 5, 15, 20, 3, 0, 0, DateTimeKind.Unspecified), 8, "Proyector", 45000m },
                    { 13, 20, new DateTime(2021, 1, 10, 9, 20, 0, 0, DateTimeKind.Unspecified), 6, "Mouse", 1500m },
                    { 31, 20, new DateTime(2021, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Licuadora", 6000m },
                    { 5, 100, new DateTime(2019, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Palitos", 126m },
                    { 6, 100, new DateTime(2020, 1, 2, 14, 30, 0, 0, DateTimeKind.Unspecified), 1, "Chizitos", 138m },
                    { 19, 100, new DateTime(2020, 8, 11, 4, 30, 0, 0, DateTimeKind.Unspecified), 1, "Mani", 121m },
                    { 20, 100, new DateTime(2020, 1, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), 1, "Nachos", 241m },
                    { 1, 100, new DateTime(2020, 12, 20, 10, 20, 0, 0, DateTimeKind.Unspecified), 2, "Gaseosa", 125m },
                    { 2, 50, new DateTime(2022, 2, 7, 9, 30, 0, 0, DateTimeKind.Unspecified), 2, "Cerveza", 120m },
                    { 3, 100, new DateTime(2022, 1, 20, 1, 0, 0, 0, DateTimeKind.Unspecified), 2, "Agua", 78m },
                    { 17, 100, new DateTime(2019, 1, 22, 10, 50, 0, 0, DateTimeKind.Unspecified), 2, "Leche", 95m },
                    { 18, 100, new DateTime(2019, 5, 18, 1, 20, 0, 0, DateTimeKind.Unspecified), 2, "Energizante", 108m },
                    { 9, 50, new DateTime(2021, 11, 10, 12, 45, 0, 0, DateTimeKind.Unspecified), 3, "Pantalon Deportivo", 6500m },
                    { 10, 50, new DateTime(2021, 12, 9, 10, 30, 0, 0, DateTimeKind.Unspecified), 3, "Camiseta Deportiva", 6500m },
                    { 21, 50, new DateTime(2020, 6, 12, 2, 30, 0, 0, DateTimeKind.Unspecified), 3, "Campera", 6000m },
                    { 22, 50, new DateTime(2020, 8, 16, 3, 0, 0, 0, DateTimeKind.Unspecified), 3, "Sweater", 3000m },
                    { 23, 50, new DateTime(2019, 10, 10, 3, 40, 0, 0, DateTimeKind.Unspecified), 3, "Jean", 2000m },
                    { 11, 50, new DateTime(2021, 4, 17, 5, 40, 0, 0, DateTimeKind.Unspecified), 4, "Lavandina", 49m },
                    { 12, 50, new DateTime(2020, 5, 15, 8, 10, 0, 0, DateTimeKind.Unspecified), 4, "Escoba", 340m },
                    { 24, 50, new DateTime(2019, 12, 25, 12, 34, 0, 0, DateTimeKind.Unspecified), 4, "Detergente", 87m },
                    { 25, 50, new DateTime(2020, 12, 24, 10, 20, 0, 0, DateTimeKind.Unspecified), 4, "Pala", 300m },
                    { 26, 50, new DateTime(2020, 4, 9, 10, 30, 0, 0, DateTimeKind.Unspecified), 4, "Secador", 800m },
                    { 27, 20, new DateTime(2020, 11, 20, 10, 10, 0, 0, DateTimeKind.Unspecified), 5, "Heladera", 83000m },
                    { 28, 20, new DateTime(2020, 9, 19, 12, 20, 0, 0, DateTimeKind.Unspecified), 5, "Lavarropa", 78000m },
                    { 29, 20, new DateTime(2019, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Cocina", 50000m },
                    { 30, 20, new DateTime(2019, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Microondas", 25000m },
                    { 8, 30, new DateTime(2021, 12, 23, 12, 0, 0, 0, DateTimeKind.Unspecified), 6, "PC", 60000m },
                    { 4, 100, new DateTime(2019, 2, 1, 3, 0, 0, 0, DateTimeKind.Unspecified), 1, "Papas", 250m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carro_idUsuario",
                table: "Carro",
                column: "idUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarroProducto_idCarro",
                table: "CarroProducto",
                column: "idCarro");

            migrationBuilder.CreateIndex(
                name: "IX_CarroProducto_idProducto",
                table: "CarroProducto",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_idUsuario",
                table: "Compra",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_CompraProducto_idCompra",
                table: "CompraProducto",
                column: "idCompra");

            migrationBuilder.CreateIndex(
                name: "IX_CompraProducto_idProducto",
                table: "CompraProducto",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_idCategoria",
                table: "Producto",
                column: "idCategoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarroProducto");

            migrationBuilder.DropTable(
                name: "CompraProducto");

            migrationBuilder.DropTable(
                name: "Carro");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
