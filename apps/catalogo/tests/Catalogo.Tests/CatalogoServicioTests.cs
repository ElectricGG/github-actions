using Catalogo.Api;

namespace Catalogo.Tests;

public class CatalogoServicioTests
{
    private readonly CatalogoServicio _servicio = new();

    [Fact]
    public void Listar_devuelve_el_catalogo_inicial()
    {
        Assert.Equal(3, _servicio.Listar().Count);
    }

    [Fact]
    public void Listar_solo_con_stock_excluye_agotados()
    {
        var productos = _servicio.Listar(soloConStock: true);

        Assert.All(productos, p => Assert.True(p.Stock > 0));
        Assert.DoesNotContain(productos, p => p.Nombre == "Zapatillas urbanas");
    }

    [Fact]
    public void Obtener_un_id_inexistente_devuelve_null()
    {
        Assert.Null(_servicio.Obtener(999));
    }

    [Fact]
    public void Crear_asigna_el_siguiente_id_y_limpia_el_nombre()
    {
        var producto = _servicio.Crear(new NuevoProducto("  Casaca  ", 199m, 10));

        Assert.Equal(4, producto.Id);
        Assert.Equal("Casaca", producto.Nombre);
    }

    [Theory]
    [InlineData("", 10, 1)]
    [InlineData("Gorra", 0, 1)]
    [InlineData("Gorra", -5, 1)]
    [InlineData("Gorra", 10, -1)]
    public void Crear_rechaza_datos_invalidos(string nombre, decimal precio, int stock)
    {
        Assert.Throws<ArgumentException>(() => _servicio.Crear(new NuevoProducto(nombre, precio, stock)));
    }
}
