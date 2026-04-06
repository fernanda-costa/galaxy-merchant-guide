using Localiza.MerchantGuide.Application;
using Localiza.MerchantGuide.Application.Services;
using Localiza.MerchantGuide.Domain.Services;
using Localiza.MerchantGuide.Infraestructure;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<IConsole, SystemConsole>();

services.AddSingleton<IntergalacticService>();
services.AddSingleton<RomanCalculatorService>();
services.AddSingleton<MaterialService>();
services.AddSingleton<InputService>();

services.AddSingleton<MerchantGuideService>();

var provider = services.BuildServiceProvider();

var app = provider.GetRequiredService<MerchantGuideService>();
app.Run();