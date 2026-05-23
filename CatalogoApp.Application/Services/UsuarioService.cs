using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public List<Usuario> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Usuario? ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public Usuario? ValidarLogin(string correo, string password)
        {
            return _repo.ValidarLogin(correo, password);
        }

        public bool CorreoExiste(string correo)
        {
            return _repo.ObtenerPorCorreo(correo) != null;
        }

        public void Registrar(Usuario usuario)
        {
            _repo.Agregar(usuario);
        }
    }
}
