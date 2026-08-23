using Structural.Patterns.Facade;

FraudService fraudService = new FraudService();

PaymentGateway paymentGateway = new PaymentGateway();

LedgerService ledgerService = new LedgerService();

NotificationService notificationService = new NotificationService();

PaymentFacade paymentFacade = new PaymentFacade(fraudService, paymentGateway, ledgerService, notificationService);


paymentFacade.ProcessPayment(orderId: 1001, cardNumber: "4532123412345678", amount: 2500, customerEmail: "alican@test.com");