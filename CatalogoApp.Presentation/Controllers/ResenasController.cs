using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CatalogoApp.Presentation.Controllers
{
    public class ResenasController : Controller
    {
        private readonly ItemService _itemService;

        public ResenasController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public IActionResult Agregar(int itemId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                TempData["Error"] = "Debes iniciar sesión antes de reseñar un videojuego.";
                return RedirectToAction("Login", "Usuarios", new { returnUrl = $"/Resenas/Agregar?itemId={itemId}" });
            }

            var item = _itemService.ObtenerPorId(itemId);
            if (item == null)
                return NotFound();

            ViewBag.Item = item;
            return View(new Resena { ItemId = itemId, Calificacion = 5 });
        }

        [HttpPost]
        public IActionResult Agregar(Resena resena)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre");

            if (usuarioId == null || string.IsNullOrWhiteSpace(usuarioNombre))
            {
                TempData["Error"] = "Debes iniciar sesión antes de reseñar un videojuego.";
                return RedirectToAction("Login", "Usuarios", new { returnUrl = $"/Resenas/Agregar?itemId={resena.ItemId}" });
            }

            var item = _itemService.ObtenerPorId(resena.ItemId);
            if (item == null)
                return NotFound();

            if (resena.Calificacion < 1 || resena.Calificacion > 5 || string.IsNullOrWhiteSpace(resena.Comentario))
            {
                ViewBag.Item = item;
                ViewBag.Error = "La calificación debe estar entre 1 y 5 y el comentario no puede estar vacío.";
                return View(resena);
            }

            resena.UsuarioId = usuarioId.Value;
            resena.UsuarioNombre = usuarioNombre;

            _itemService.AgregarResena(resena.ItemId, resena);

            return RedirectToAction("Detalle", "Catalogo", new { id = resena.ItemId });
        }
    }
}
