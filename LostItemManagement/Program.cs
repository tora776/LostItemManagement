using LostItemManagement.Controllers;
using LostItemManagement.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// ログ設定
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // コンソールにログを出力
builder.Logging.AddDebug();   // デバッグ出力

// Add services to the container.
builder.Services.AddControllersWithViews();
// DIコンテナ登録処理
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddScoped<LostRepository>(); // LostRepositoryをDIコンテナに登録
builder.Services.AddScoped<LostService>();    // LostServiceをDIコンテナに登録

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
