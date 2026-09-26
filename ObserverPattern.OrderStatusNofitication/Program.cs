using ObserverPattern.OrderStatusNofitication;

var order = new Order(123);

var email = new EmailNotifier();
var analytics = new AnalyticsObserver();

order.StatusChanged += email.OnStatusChanged;
order.StatusChanged += analytics.OnStatusChanged;

order.ChangeStatus("Paid");
order.ChangeStatus("Shipped");
