using StructuralPatterns.Decorator;

IPaymentService payment = new AuthorizationDecorator(new LoggingPaymentDecorator(new PaymentService()));
