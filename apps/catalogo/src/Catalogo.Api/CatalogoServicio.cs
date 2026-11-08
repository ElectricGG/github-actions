namespace Catalogo.Api;

public class CatalogoServicio
{
    private readonly List<Producto> _productos =
    [
        new(1, "Polo básico", 39.90m, 120),
        new(2, "Jean clásico", 129.00m, 45),
        new(3, "Zapatillas urbanas", 219.50m, 0),
    ];

    private readonly Lock _candado = new();

    public IReadOnlyList<Producto> Listar(bool soloConStock = false)
    {
        lock (_candado)
        {
            return soloConStock
                ? _productos.Where(p => p.Stock > 0).ToList()
                : _productos.ToList();
        }
    }

    public Producto? Obtener(int id)
    {
        lock (_candado)
        {
            return _productos.FirstOrDefault(p => p.Id == id);
        }
    }

    public Producto Crear(NuevoProducto nuevo)
    {
        Validar(nuevo);

        lock (_candado)
        {
            var id = _productos.Count == 0 ? 1 : _productos.Max(p => p.Id) + 1;
            var producto = new Producto(id, nuevo.Nombre.Trim(), nuevo.Precio, nuevo.Stock);
            _productos.Add(producto);
            return producto;
        }
    }

    private static void Validar(NuevoProducto nuevo)
    {
        if (string.IsNullOrWhiteSpace(nuevo.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (nuevo.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor que cero.");

        if (nuevo.Stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");
    }
}
