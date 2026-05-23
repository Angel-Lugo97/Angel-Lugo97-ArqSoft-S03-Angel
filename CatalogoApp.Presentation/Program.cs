using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var itemsPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "data",
    "items.json"
);

var usuariosPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "data",
    "usuarios.json"
);

builder.Services.AddSingleton<IItemRepository>(
    new JsonItemRepository(itemsPath)
);

builder.Services.AddSingleton<IUsuarioRepository>(
    new JsonUsuarioRepository(usuariosPath)
);

builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalogo}/{action=Index}/{id?}");

app.Run();
