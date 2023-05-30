using VandalFood.BLL.Dependency_Injection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddDependencies();
builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", config =>
        {
            config.Cookie.Name = "User.Cookie";
            config.LoginPath = "/Home/Authenticate";
        });

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Get}/{id?}");

app.Run();
