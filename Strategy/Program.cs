using Strategy.Strategy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddScoped<
    IPaymentStrategy,
    CreditCardPaymentStrategy>();

builder.Services.AddScoped<
    IPaymentStrategy,
    BankTransferPaymentStrategy>();

builder.Services.AddScoped<
    IPaymentStrategy,
    WalletPaymentStrategy>();

//builder.Services.AddKeyedScoped<
//    IPaymentStrategy,
//    CreditCardPaymentStrategy>(
//        PaymentType.CreditCard);

//builder.Services.AddKeyedScoped<
//    IPaymentStrategy,
//    BankTransferPaymentStrategy>(
//        PaymentType.BankTransfer);

//builder.Services.AddKeyedScoped<
//    IPaymentStrategy,
//    WalletPaymentStrategy>(
//        PaymentType.Wallet);

builder.Services.AddScoped<
    IPaymentStrategyResolver,
    PaymentStrategyResolver>();

builder.Services.AddScoped<PaymentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
