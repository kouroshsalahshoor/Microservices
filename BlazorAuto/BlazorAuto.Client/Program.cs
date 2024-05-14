using BlazorAuto.Client.Services;
using BlazorAuto.Client.Services.IService;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//builder.Services.AddHttpClient();
//builder.Services.AddHttpClient<ICouponService, CouponService>();

//ApplicationConstants.CouponApi = builder.Configuration["ServiceUrls:CouponApi"];

//builder.Services.AddScoped<IBaseService, BaseService>();
//builder.Services.AddScoped<ICouponService, CouponService>();

await builder.Build().RunAsync();
