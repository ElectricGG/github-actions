using System.Net;
using System.Net.Http.Json;
using Catalogo.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalogo.Tests;

public class ApiTests(WebApplicationFactory<Program> fabrica) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _cliente = fabrica.CreateClient();

    [Fact]
    public async Task Salud_responde_ok()
    {
        var respuesta = await _cliente.GetAsync("/salud");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
    }

    [Fact]
    public async Task Producto_inexistente_responde_404()
    {
        var respuesta = await _cliente.GetAsync("/productos/999");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task Crear_producto_valido_responde_201()
    {
        var respuesta = await _cliente.PostAsJsonAsync("/productos", new NuevoProducto("Medias", 15m, 50));

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        var creado = await respuesta.Content.ReadFromJsonAsync<Producto>();
        Assert.Equal("Medias", creado!.Nombre);
    }

    [Fact]
    public async Task Crear_producto_sin_precio_responde_400()
    {
        var respuesta = await _cliente.PostAsJsonAsync("/productos", new NuevoProducto("Medias", 0m, 50));

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }
}
