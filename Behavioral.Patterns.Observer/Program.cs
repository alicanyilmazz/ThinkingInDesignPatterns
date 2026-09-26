using Behavioral.Patterns.Observer;

var product = new ProductStock("PlayStation 6");

var emailObserver = new EmailStockObserver();
var mobileObserver = new MobileStockObserver();

product.StockChanged += emailObserver.HandleStockChanged;
product.StockChanged += mobileObserver.HandleStockChanged;

product.UpdateStock(10);