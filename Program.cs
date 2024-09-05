using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
public void ConfigureServices(IServiceCollection services)
		{   builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options=>//valida con Cookie)
            {   options.LoginPath = "Usuario/Loguin";
                options.LogoutPath = "Usuarios/Logout"; 
                options.AccessDeniedPath = "/Home/Restringuido";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5); //Tiempo de expiracion

             });
            services.AddAuthorization(options =>
			{
				//options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
				options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "Empleado"));
			}); 
		
			services.AddMvc();
			services.AddSignalR();//añade signalR
			//IUserIdProvider permite cambiar el ClaimType usado para obtener el UserIdentifier en Hub
			services.AddSingleton<IUserIdProvider, UserIdProvider>();
			// SOLO PARA INYECCIÓN DE DEPENDECIAS:
			/*
			Transient objects are always different; a new instance is provided to every controller and every service.
			Scoped objects are the same within a request, but different across different requests.
			Singleton objects are the same for every object and every request.
			*/
			services.AddTransient<IRepositorio<Propietario>, RepositorioPropietario>();
			services.AddTransient<IRepositorioPropietario, RepositorioPropietario>();
			//services.AddTransient<IRepositorio<Inquilino>, RepositorioInquilino>();
			services.AddTransient<IRepositorioInmueble, RepositorioInmueble>();
			services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();
			// SOLO SI USA ENTITY FRAMEWORK:
			services.AddDbContext<DataContext>(
				options => options.UseSqlServer(
					configuration["ConnectionStrings:DefaultConnection"]
				)
			);
			/* PARA MySql - usando Pomelo */
			//services.AddDbContext<DataContext>(
			//	options => options.UseMySql(
			//		configuration["ConnectionStrings:DefaultConnection"],
			//		ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"])
			//	)
			//);
		}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options=>//valida con Cookie)
            {   options.LoginPath = "Usuario/Loguin";
                options.LogoutPath = "Usuarios/Logout"; 
                options.AccessDeniedPath = "/Home/Restringuido";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5); //Tiempo de expiracion

             });
services.AddAuthorization(options =>
			{
				//options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
				options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "Empleado"));
			}); 
 //.AddJwtBearer(options=> //esto para que la web valide con token)
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


