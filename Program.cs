using BlazorApp2.Data;
using BlazorApp2.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Добавляем контекст базы данных
builder.Services.AddDbContextFactory<UniversityContext>(opt =>
    opt.UseSqlite($"Data Source=student.db"));

var app = builder.Build();

// Создаем и заполняем базу данных
await using var scope =
    app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var options =
    scope.ServiceProvider.GetRequiredService<DbContextOptions<UniversityContext>>();
await DatabaseUtility.EnsureDbCreatedAndSeedAsync(options);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();