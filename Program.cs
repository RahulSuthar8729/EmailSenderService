using EmailSenderService;
using EmailSenderService.Database; 
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<PdfGenerator>();
builder.Services.AddTransient<IEmailSender, EmailService>();
builder.Services.AddTransient<EmailRepository>(); 

builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();