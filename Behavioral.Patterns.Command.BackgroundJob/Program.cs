// Receivers

using Behavioral.Patterns.Command.BackgroundJob.Commands;
using Behavioral.Patterns.Command.BackgroundJob.Commands.Abstracts;
using Behavioral.Patterns.Command.BackgroundJob.Queue;
using Behavioral.Patterns.Command.BackgroundJob.Services;

EmailService emailService = new EmailService();

ReportService reportService = new ReportService();

PaymentService paymentService = new PaymentService();


// Queue

BackgroundJobQueue jobQueue = new BackgroundJobQueue();


// Worker

BackgroundJobWorker worker = new BackgroundJobWorker(jobQueue);


// -------------------------------
// COMMAND'LARI OLUŞTUR
// -------------------------------

ICommand emailCommand = new SendEmailCommand(emailService, "alican@test.com", "Siparişiniz başarıyla oluşturuldu.");


ICommand reportCommand = new GenerateReportCommand(reportService, 1001);


ICommand paymentCommand = new ProcessPaymentCommand(paymentService, 5001, 2500);


// -------------------------------
// QUEUE'YA EKLE
// -------------------------------

jobQueue.Enqueue(emailCommand);

jobQueue.Enqueue(reportCommand);

jobQueue.Enqueue(paymentCommand);


Console.WriteLine();

Console.WriteLine($"Queue'daki job sayısı: {jobQueue.Count}");


// -------------------------------
// WORKER'I ÇALIŞTIR
// -------------------------------

worker.Run();