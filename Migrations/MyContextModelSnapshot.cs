﻿// <auto-generated />
using Final_Plataformas_De_Desarrollo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Final_Plataformas_De_Desarrollo.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Carro", b =>
                {
                    b.Property<int>("idCarro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.HasKey("idCarro");

                    b.HasIndex("idUsuario")
                        .IsUnique();

                    b.ToTable("Carro");

                    b.HasData(
                        new
                        {
                            idCarro = 1,
                            idUsuario = 1
                        },
                        new
                        {
                            idCarro = 2,
                            idUsuario = 2
                        },
                        new
                        {
                            idCarro = 3,
                            idUsuario = 3
                        });
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.CarroProducto", b =>
                {
                    b.Property<int>("idCarroProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("cantidad")
                        .HasColumnType("tinyint");

                    b.Property<int>("idCarro")
                        .HasColumnType("int");

                    b.Property<int>("idProducto")
                        .HasColumnType("int");

                    b.HasKey("idCarroProducto");

                    b.HasIndex("idCarro");

                    b.HasIndex("idProducto");

                    b.ToTable("CarroProducto");
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Categoria", b =>
                {
                    b.Property<int>("idCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("idCategoria");

                    b.ToTable("Categoria");

                    b.HasData(
                        new
                        {
                            idCategoria = 1,
                            nombre = "Comida"
                        },
                        new
                        {
                            idCategoria = 2,
                            nombre = "Bebida"
                        },
                        new
                        {
                            idCategoria = 3,
                            nombre = "Ropa"
                        },
                        new
                        {
                            idCategoria = 4,
                            nombre = "Articulos de limpieza"
                        },
                        new
                        {
                            idCategoria = 5,
                            nombre = "Electrodomesticos"
                        },
                        new
                        {
                            idCategoria = 6,
                            nombre = "Informatica"
                        },
                        new
                        {
                            idCategoria = 7,
                            nombre = "Herramientas"
                        },
                        new
                        {
                            idCategoria = 8,
                            nombre = "Electronica"
                        },
                        new
                        {
                            idCategoria = 9,
                            nombre = "Mascotas"
                        },
                        new
                        {
                            idCategoria = 10,
                            nombre = "Libreria"
                        });
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Compra", b =>
                {
                    b.Property<int>("idCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.Property<decimal>("total")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("idCompra");

                    b.HasIndex("idUsuario");

                    b.ToTable("Compra");

                    b.HasData(
                        new
                        {
                            idCompra = 1,
                            idUsuario = 2,
                            total = 0m
                        });
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.CompraProducto", b =>
                {
                    b.Property<int>("idCompraProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("cantidad")
                        .HasColumnType("tinyint");

                    b.Property<int>("idCompra")
                        .HasColumnType("int");

                    b.Property<int>("idProducto")
                        .HasColumnType("int");

                    b.HasKey("idCompraProducto");

                    b.HasIndex("idCompra");

                    b.HasIndex("idProducto");

                    b.ToTable("CompraProducto");
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Producto", b =>
                {
                    b.Property<int>("idProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("cantidad")
                        .HasColumnType("int");

                    b.Property<int>("idCategoria")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("precio")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("idProducto");

                    b.HasIndex("idCategoria");

                    b.ToTable("Producto");

                    b.HasData(
                        new
                        {
                            idProducto = 1,
                            cantidad = 100,
                            idCategoria = 2,
                            nombre = "Gaseosa",
                            precio = 125m
                        },
                        new
                        {
                            idProducto = 2,
                            cantidad = 50,
                            idCategoria = 2,
                            nombre = "Cerveza",
                            precio = 120m
                        },
                        new
                        {
                            idProducto = 3,
                            cantidad = 100,
                            idCategoria = 2,
                            nombre = "Agua",
                            precio = 78m
                        },
                        new
                        {
                            idProducto = 4,
                            cantidad = 100,
                            idCategoria = 1,
                            nombre = "Papas",
                            precio = 250m
                        },
                        new
                        {
                            idProducto = 5,
                            cantidad = 100,
                            idCategoria = 1,
                            nombre = "Palitos",
                            precio = 126m
                        },
                        new
                        {
                            idProducto = 6,
                            cantidad = 100,
                            idCategoria = 1,
                            nombre = "Chizitos",
                            precio = 138m
                        },
                        new
                        {
                            idProducto = 7,
                            cantidad = 150,
                            idCategoria = 8,
                            nombre = "TV",
                            precio = 80000m
                        },
                        new
                        {
                            idProducto = 8,
                            cantidad = 30,
                            idCategoria = 6,
                            nombre = "PC",
                            precio = 60000m
                        },
                        new
                        {
                            idProducto = 9,
                            cantidad = 50,
                            idCategoria = 3,
                            nombre = "Pantalon Deportivo",
                            precio = 6500m
                        },
                        new
                        {
                            idProducto = 10,
                            cantidad = 50,
                            idCategoria = 3,
                            nombre = "Camiseta Deportiva",
                            precio = 6500m
                        },
                        new
                        {
                            idProducto = 11,
                            cantidad = 50,
                            idCategoria = 4,
                            nombre = "Lavandina",
                            precio = 49m
                        },
                        new
                        {
                            idProducto = 12,
                            cantidad = 50,
                            idCategoria = 4,
                            nombre = "Escoba",
                            precio = 340m
                        },
                        new
                        {
                            idProducto = 13,
                            cantidad = 20,
                            idCategoria = 6,
                            nombre = "Mouse",
                            precio = 1500m
                        },
                        new
                        {
                            idProducto = 14,
                            cantidad = 20,
                            idCategoria = 6,
                            nombre = "Teclado",
                            precio = 3500m
                        },
                        new
                        {
                            idProducto = 15,
                            cantidad = 20,
                            idCategoria = 6,
                            nombre = "Monitor",
                            precio = 28000m
                        },
                        new
                        {
                            idProducto = 16,
                            cantidad = 20,
                            idCategoria = 6,
                            nombre = "Notebook",
                            precio = 95000m
                        },
                        new
                        {
                            idProducto = 17,
                            cantidad = 100,
                            idCategoria = 2,
                            nombre = "Leche",
                            precio = 95m
                        },
                        new
                        {
                            idProducto = 18,
                            cantidad = 100,
                            idCategoria = 2,
                            nombre = "Energizante",
                            precio = 108m
                        },
                        new
                        {
                            idProducto = 19,
                            cantidad = 100,
                            idCategoria = 1,
                            nombre = "Mani",
                            precio = 121m
                        },
                        new
                        {
                            idProducto = 20,
                            cantidad = 100,
                            idCategoria = 1,
                            nombre = "Nachos",
                            precio = 241m
                        },
                        new
                        {
                            idProducto = 21,
                            cantidad = 50,
                            idCategoria = 3,
                            nombre = "Campera",
                            precio = 6000m
                        },
                        new
                        {
                            idProducto = 22,
                            cantidad = 50,
                            idCategoria = 3,
                            nombre = "Sweater",
                            precio = 3000m
                        },
                        new
                        {
                            idProducto = 23,
                            cantidad = 50,
                            idCategoria = 3,
                            nombre = "Jean",
                            precio = 2000m
                        },
                        new
                        {
                            idProducto = 24,
                            cantidad = 50,
                            idCategoria = 4,
                            nombre = "Detergente",
                            precio = 87m
                        },
                        new
                        {
                            idProducto = 25,
                            cantidad = 50,
                            idCategoria = 4,
                            nombre = "Pala",
                            precio = 300m
                        },
                        new
                        {
                            idProducto = 26,
                            cantidad = 50,
                            idCategoria = 4,
                            nombre = "Secador",
                            precio = 800m
                        },
                        new
                        {
                            idProducto = 27,
                            cantidad = 20,
                            idCategoria = 5,
                            nombre = "Heladera",
                            precio = 83000m
                        },
                        new
                        {
                            idProducto = 28,
                            cantidad = 20,
                            idCategoria = 5,
                            nombre = "Lavarropa",
                            precio = 78000m
                        },
                        new
                        {
                            idProducto = 29,
                            cantidad = 20,
                            idCategoria = 5,
                            nombre = "Cocina",
                            precio = 50000m
                        },
                        new
                        {
                            idProducto = 30,
                            cantidad = 20,
                            idCategoria = 5,
                            nombre = "Microondas",
                            precio = 25000m
                        },
                        new
                        {
                            idProducto = 31,
                            cantidad = 20,
                            idCategoria = 5,
                            nombre = "Licuadora",
                            precio = 6000m
                        },
                        new
                        {
                            idProducto = 32,
                            cantidad = 150,
                            idCategoria = 7,
                            nombre = "Taladro",
                            precio = 15000m
                        },
                        new
                        {
                            idProducto = 33,
                            cantidad = 150,
                            idCategoria = 7,
                            nombre = "Amoladora",
                            precio = 7000m
                        },
                        new
                        {
                            idProducto = 34,
                            cantidad = 150,
                            idCategoria = 7,
                            nombre = "Soldadora",
                            precio = 20000m
                        },
                        new
                        {
                            idProducto = 35,
                            cantidad = 150,
                            idCategoria = 7,
                            nombre = "Sierra",
                            precio = 8000m
                        },
                        new
                        {
                            idProducto = 36,
                            cantidad = 150,
                            idCategoria = 8,
                            nombre = "Hidrolavadora",
                            precio = 7000m
                        },
                        new
                        {
                            idProducto = 37,
                            cantidad = 150,
                            idCategoria = 8,
                            nombre = "Parlantes",
                            precio = 10000m
                        },
                        new
                        {
                            idProducto = 38,
                            cantidad = 150,
                            idCategoria = 8,
                            nombre = "Auriculares",
                            precio = 4500m
                        },
                        new
                        {
                            idProducto = 39,
                            cantidad = 150,
                            idCategoria = 8,
                            nombre = "Celular",
                            precio = 50000m
                        },
                        new
                        {
                            idProducto = 40,
                            cantidad = 150,
                            idCategoria = 8,
                            nombre = "Proyector",
                            precio = 45000m
                        },
                        new
                        {
                            idProducto = 41,
                            cantidad = 200,
                            idCategoria = 9,
                            nombre = "Alimento para Perros",
                            precio = 282m
                        },
                        new
                        {
                            idProducto = 42,
                            cantidad = 200,
                            idCategoria = 9,
                            nombre = "Alimento para Gatos",
                            precio = 144m
                        },
                        new
                        {
                            idProducto = 43,
                            cantidad = 200,
                            idCategoria = 9,
                            nombre = "Cuchas",
                            precio = 3000m
                        },
                        new
                        {
                            idProducto = 44,
                            cantidad = 200,
                            idCategoria = 9,
                            nombre = "Correas",
                            precio = 1200m
                        },
                        new
                        {
                            idProducto = 45,
                            cantidad = 200,
                            idCategoria = 9,
                            nombre = "Juguetes",
                            precio = 600m
                        },
                        new
                        {
                            idProducto = 46,
                            cantidad = 200,
                            idCategoria = 10,
                            nombre = "Cuaderno",
                            precio = 250m
                        },
                        new
                        {
                            idProducto = 47,
                            cantidad = 200,
                            idCategoria = 10,
                            nombre = "Marcadores",
                            precio = 1200m
                        },
                        new
                        {
                            idProducto = 48,
                            cantidad = 200,
                            idCategoria = 10,
                            nombre = "Calculadora",
                            precio = 1500m
                        },
                        new
                        {
                            idProducto = 49,
                            cantidad = 200,
                            idCategoria = 10,
                            nombre = "Resma",
                            precio = 700m
                        },
                        new
                        {
                            idProducto = 50,
                            cantidad = 200,
                            idCategoria = 10,
                            nombre = "Tablero dibujo",
                            precio = 3000m
                        });
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Usuario", b =>
                {
                    b.Property<int>("idUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("apellido")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<long>("cuit_cuil")
                        .HasColumnType("bigint");

                    b.Property<int>("dni")
                        .HasColumnType("int");

                    b.Property<string>("mail")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("rol")
                        .HasColumnType("int");

                    b.HasKey("idUsuario");

                    b.ToTable("Usuario");

                    b.HasData(
                        new
                        {
                            idUsuario = 1,
                            apellido = "Admin",
                            cuit_cuil = 34865218L,
                            dni = 123456,
                            mail = "admin@gmail.com",
                            nombre = "Admin",
                            password = "123456",
                            rol = 1
                        },
                        new
                        {
                            idUsuario = 2,
                            apellido = "Lopez",
                            cuit_cuil = 25689475L,
                            dni = 654321,
                            mail = "pepitolopez@gmail.com",
                            nombre = "Pepito",
                            password = "654321",
                            rol = 2
                        },
                        new
                        {
                            idUsuario = 3,
                            apellido = "Perez",
                            cuit_cuil = 20321548L,
                            dni = 32154869,
                            mail = "joseperez@hotmail.com",
                            nombre = "José",
                            password = "123456",
                            rol = 3
                        });
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Carro", b =>
                {
                    b.HasOne("Final_Plataformas_De_Desarrollo.Models.Usuario", "usuario")
                        .WithOne("miCarro")
                        .HasForeignKey("Final_Plataformas_De_Desarrollo.Models.Carro", "idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.CarroProducto", b =>
                {
                    b.HasOne("Final_Plataformas_De_Desarrollo.Models.Carro", "carro")
                        .WithMany("carroProducto")
                        .HasForeignKey("idCarro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Final_Plataformas_De_Desarrollo.Models.Producto", "producto")
                        .WithMany("carroProducto")
                        .HasForeignKey("idProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Compra", b =>
                {
                    b.HasOne("Final_Plataformas_De_Desarrollo.Models.Usuario", "usuario")
                        .WithMany("compras")
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.CompraProducto", b =>
                {
                    b.HasOne("Final_Plataformas_De_Desarrollo.Models.Compra", "compra")
                        .WithMany("compraProducto")
                        .HasForeignKey("idCompra")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Final_Plataformas_De_Desarrollo.Models.Producto", "producto")
                        .WithMany("compraProducto")
                        .HasForeignKey("idProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Final_Plataformas_De_Desarrollo.Models.Producto", b =>
                {
                    b.HasOne("Final_Plataformas_De_Desarrollo.Models.Categoria", "cat")
                        .WithMany("productos")
                        .HasForeignKey("idCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
