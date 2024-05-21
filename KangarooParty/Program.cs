using Microsoft.EntityFrameworkCore;
using KangarooParty.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var DBConnStr = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("KangarooParty"));
DBConnStr.Password = builder.Configuration["DBpass"];
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(DBConnStr.ConnectionString));

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

