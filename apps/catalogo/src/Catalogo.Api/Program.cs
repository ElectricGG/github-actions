using Catalogo.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<CatalogoServicio>();

var app = builder.Build();

app.MapGet("/salud", () => Results.Ok(new { estado = "ok" }));

app.MapGet("/productos", (bool? soloConStock, CatalogoServicio servicio) =>
    Results.Ok(servicio.Listar(soloConStock ?? false)));

app.MapGet("/productos/{id:int}", (int id, CatalogoServicio servicio) =>
    servicio.Obtener(id) is { } producto ? Results.Ok(producto) : Results.NotFound());

app.MapPost("/productos", (NuevoProducto nuevo, CatalogoServicio servicio) =>
{
    try
    {
        var producto = servicio.Crear(nuevo);
        return Results.Created($"/productos/{producto.Id}", producto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

public partial class Program;
