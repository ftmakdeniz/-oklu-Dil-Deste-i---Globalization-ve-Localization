using GlobalizationandLocalization.Services;
using Microsoft.AspNetCore.Localization;
using System.Reflection;
using System.Globalization;
using GlobalizationandLocalization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
#region localizer
builder.Services.AddSingleton<LanguageService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddMvcLocalization().AddDataAnnotationsLocalization(options =>
    options.DataAnnotationLocalizerProvider = (Type, factory) =>
    {
        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
        return factory.Create(nameof(SharedResource), assemblyName.Name);
    }
);
builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportCultures = new List<CultureInfo>
        {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR"),
        };
        options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
        options.SupportedCultures = supportCultures;
        options.SupportedUICultures = supportCultures;
        options.SupportedUICultures = supportCultures;
        options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
    });
#endregion
// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



internal class ISOptions<T>
{
    internal void Value(RequestLocalizationOptions obj)
    {
        throw new NotImplementedException();
    }
}

