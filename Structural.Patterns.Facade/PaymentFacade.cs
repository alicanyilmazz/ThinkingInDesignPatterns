using System;
using System.Collections.Generic;
using System.Text;

namespace Structural.Patterns.Facade;

public class PaymentFacade
{
    private readonly FraudService _fraudService;
    private readonly PaymentGateway _paymentGateway;
    private readonly LedgerService _ledgerService;
    private readonly NotificationService _notificationService;

    public PaymentFacade(FraudService fraudService, PaymentGateway paymentGateway, LedgerService ledgerService, NotificationService notificationService)
    {
        _fraudService = fraudService;
        _paymentGateway = paymentGateway;
        _ledgerService = ledgerService;
        _notificationService = notificationService;
    }

    public void ProcessPayment(int orderId, string cardNumber, decimal amount, string customerEmail)
    {
        Console.WriteLine("Ödeme işlemi başladı.");

        Console.WriteLine("-----------------------------");


        bool fraudResult = _fraudService.CheckFraud(cardNumber, amount);


        if (!fraudResult)
        {
            Console.WriteLine("Fraud kontrolü başarısız.");

            return;
        }


        bool paymentResult = _paymentGateway.Charge(cardNumber, amount);


        if (!paymentResult)
        {
            Console.WriteLine("Ödeme alınamadı.");

            return;
        }


        _ledgerService.RecordPayment(orderId, amount);

        _notificationService.SendPaymentSuccess(customerEmail, orderId);


        Console.WriteLine("-----------------------------");

        Console.WriteLine("Ödeme işlemi başarıyla tamamlandı.");
    }
}