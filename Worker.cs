using EmailSenderService.Database; 
using Microsoft.Extensions.Logging;

namespace EmailSenderService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEmailSender _emailSender;
    private readonly EmailRepository _emailRepository;

    public Worker(ILogger<Worker> logger, IEmailSender emailSender, EmailRepository emailRepository)
    {
        _logger = logger;
        _emailSender = emailSender;
        _emailRepository = emailRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
               
                var pendingEmails = await _emailRepository.GetPendingEmailsAsync();

                foreach (var emailData in pendingEmails)
                {
                    
                    await _emailSender.SendEmailAsync(emailData);

                    
                    await _emailRepository.MarkEmailAsSentAsync(emailData.Code);
                }

                _logger.LogInformation("Processed {count} pending emails at: {time}", pendingEmails.Count, DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing emails at: {time}", DateTimeOffset.Now);
            }

            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}