// ---------------------------------------
// Create Strategies
// ---------------------------------------

using Services;

ICardServiceStrategy esbStrategy = new EsbCardServiceStrategy();

ICardServiceStrategy webServiceStrategy = new WebServiceCardServiceStrategy();


// ---------------------------------------
// Create Resolver
// ---------------------------------------

ICardServiceStrategyResolver resolver = new CardServiceStrategyResolver(new ICardServiceStrategy[] { esbStrategy,webServiceStrategy });


// ---------------------------------------
// Create Context
// ---------------------------------------

var cardService = new CardService(resolver);


// =======================================
// ESB
// =======================================

var request = new CardValidationRequest
{
    CardNumber = "5295451234567890",
    Amount = 1000
};

var esbResponse = cardService.ValidateDebitCardRequest(request,ServiceType.Esb);

Console.WriteLine(esbResponse.Message);


// =======================================
// WebService
// =======================================

var webServiceResponse = cardService.ValidateDebitCardRequest(request,ServiceType.WebService);

Console.WriteLine(webServiceResponse.Message);


// =======================================
// ESB-only operations
// =======================================

IEsbSpecialService esbSpecialService = new EsbSpecialService();

var pinChangeResponse = esbSpecialService.DoPinChange(new DoPinChangeRequest { CardNumber = "5295451234567890" });

var cashResponse = esbSpecialService.DoCashWithDrawal(new DoCashWithDrawalRequest { Amount = 1000 });
