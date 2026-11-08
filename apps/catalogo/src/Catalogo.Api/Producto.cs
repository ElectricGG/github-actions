namespace Catalogo.Api;

public record Producto(int Id, string Nombre, decimal Precio, int Stock);

public record NuevoProducto(string Nombre, decimal Precio, int Stock);
