using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonUsuarioRepository : IUsuarioRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _opciones = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public JsonUsuarioRepository(string filePath)
        {
            _filePath = filePath;

            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);

            if (!File.Exists(_filePath))
                Guardar(new List<Usuario>());
        }

        public List<Usuario> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<Usuario>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Usuario>>(json, _opciones) ?? new List<Usuario>();
        }

        public Usuario? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Id == id);
        }

        public Usuario? ObtenerPorCorreo(string correo)
        {
            return ObtenerTodos().FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
        }

        public void Agregar(Usuario usuario)
        {
            var usuarios = ObtenerTodos();
            usuario.Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1;
            usuarios.Add(usuario);
            Guardar(usuarios);
        }

        public Usuario? ValidarLogin(string correo, string password)
        {
            return ObtenerTodos().FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);
        }

        private void Guardar(List<Usuario> usuarios)
        {
            var json = JsonSerializer.Serialize(usuarios, _opciones);
            File.WriteAllText(_filePath, json);
        }
    }
}
