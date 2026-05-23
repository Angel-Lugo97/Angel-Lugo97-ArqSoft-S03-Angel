using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonItemRepository : IItemRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _opciones = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public JsonItemRepository(string filePath)
        {
            _filePath = filePath;

            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public List<Item> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<Item>();

            var json = File.ReadAllText(_filePath);
            var items = JsonSerializer.Deserialize<List<Item>>(json, _opciones) ?? new List<Item>();

            foreach (var item in items)
            {
                item.Resenas ??= new List<Resena>();
            }

            return items;
        }

        public Item? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(i => i.Id == id);
        }

        public void Agregar(Item item)
        {
            var items = ObtenerTodos();

            item.Id = items.Count > 0 ? items.Max(i => i.Id) + 1 : 1;
            item.Resenas ??= new List<Resena>();

            items.Add(item);
            Guardar(items);
        }

        public void Eliminar(int id)
        {
            var items = ObtenerTodos();
            var aEliminar = items.FirstOrDefault(i => i.Id == id);

            if (aEliminar != null)
            {
                items.Remove(aEliminar);
                Guardar(items);
            }
        }

        public void AgregarResena(int itemId, Resena resena)
        {
            var items = ObtenerTodos();
            var item = items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                return;

            item.Resenas ??= new List<Resena>();

            resena.Id = item.Resenas.Count > 0 ? item.Resenas.Max(r => r.Id) + 1 : 1;
            resena.ItemId = itemId;
            resena.Fecha = DateTime.Now;

            item.Resenas.Add(resena);
            Guardar(items);
        }

        private void Guardar(List<Item> items)
        {
            var json = JsonSerializer.Serialize(items, _opciones);
            File.WriteAllText(_filePath, json);
        }
    }
}
