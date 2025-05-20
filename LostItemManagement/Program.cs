using LostItemManagement.Controllers;
using LostItemManagement.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// ���O�ݒ�
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // �R���\�[���Ƀ��O���o��
builder.Logging.AddDebug();   // �f�o�b�O�o��

// Add services to the container.
builder.Services.AddControllersWithViews();
// DI�R���e�i�o�^����
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddScoped<LostRepository>(); // LostRepository��DI�R���e�i�ɓo�^
builder.Services.AddScoped<LostService>();    // LostService��DI�R���e�i�ɓo�^

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
