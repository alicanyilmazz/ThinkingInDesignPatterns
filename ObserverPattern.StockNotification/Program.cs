using ObserverPattern.StockNotification;

var product = new ProductStock("PlayStation 6");

var emailObserver = new EmailStockObserver();
var mobileObserver = new MobileStockObserver();

product.Attach(emailObserver);
product.Attach(mobileObserver);

product.UpdateStock(10);
