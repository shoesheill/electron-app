using System.Runtime.InteropServices;
using DotNetEd.CoreAdmin;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.EntityFrameworkCore;
using TicketApp.Data;
using TicketApp.Models;
using Screen = ElectronNET.API.Screen;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);
// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
if(builder.Configuration.GetValue<bool>("FullCoreAdmin"))
    builder.Services.AddCoreAdmin();
else
    builder.Services.AddCoreAdmin(new CoreAdminOptions() { IgnoreEntityTypes = new List<Type>()
    {
        typeof(Ticket),typeof(Transaction), typeof(TicketSeat),typeof(ScreenSeat),typeof(Theater)
    } });

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin() // Allow requests from any origin
            .AllowAnyHeader() // Allow any headers
            .AllowAnyMethod(); // Allow any HTTP methods (GET, POST, PUT, etc.)
    });
});
var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any()) dbContext.Database.Migrate();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapRazorPages();

app.MapControllers();
app.MapDefaultControllerRoute();
app.UseCoreAdminCustomTitle("Admin Dashboard");
if (HybridSupport.IsElectronActive)
{
    CreateMenu(); 
    Task.Run(async () =>
    {
        var window = await Electron.WindowManager.CreateWindowAsync();
       window.OnClosed += () => { Electron.App.Quit(); };
    });
}

app.Run();
void CreateMenu()  
{  
    bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);  
    MenuItem[] menu = null;

    MenuItem[] appMenu = new MenuItem[]  
    {  
        new MenuItem { Role = MenuRole.about },  
        new MenuItem { Type = MenuType.separator },  
        new MenuItem { Role = MenuRole.services },  
        new MenuItem { Type = MenuType.separator },  
        new MenuItem { Role = MenuRole.hide },  
        new MenuItem { Role = MenuRole.hideothers },  
        new MenuItem { Role = MenuRole.unhide },  
        new MenuItem { Type = MenuType.separator },  
        new MenuItem { Role = MenuRole.quit }  
    };

    var fileMenu = new MenuItem[]
    {
        new MenuItem { Label = "Home", 
            Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/") },
        new MenuItem { Label = "Dashboard", 
            Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/CoreAdmin") },
        new MenuItem { Label = "Movie", 
            Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/Movie") },
        new MenuItem { Label = "Calculator", 
            Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/seatlayout") },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = isMac?MenuRole.close: MenuRole.quit }
    };

    MenuItem[] viewMenu = new MenuItem[]  
    {  
        new MenuItem { Role = MenuRole.reload },  
        new MenuItem { Role = MenuRole.forcereload },  
        new MenuItem { Role = MenuRole.toggledevtools },  
        new MenuItem { Type = MenuType.separator },  
        new MenuItem { Role = MenuRole.resetzoom },  
        new MenuItem { Role = MenuRole.zoomin },  
        new MenuItem { Role = MenuRole.zoomout },  
        new MenuItem { Type = MenuType.separator },  
        new MenuItem { Role = MenuRole.togglefullscreen }  
    };  

    if (isMac)  
    {  
        menu = new MenuItem[]  
        {  
            new MenuItem { Label = "Electron", Type = MenuType.submenu, Submenu = appMenu },  
            new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },  
            new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu }  
        };  
    }  
    else  
    {  
        menu = new MenuItem[]  
        {  
            new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },  
            new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu }  
        };  
    }

    Electron.Menu.SetApplicationMenu(menu);  
}