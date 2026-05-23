using CatalogoApp.Domain.Models;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> ObtenerTodos();
        Usuario? ObtenerPorId(int id);
        Usuario? ObtenerPorCorreo(string correo);
        void Agregar(Usuario usuario);
        Usuario? ValidarLogin(string correo, string password);
    }
}
