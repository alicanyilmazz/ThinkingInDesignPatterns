using System;

namespace FacadePatternDemo;

internal static class Program
{
    private static void Main()
    {
        var cardService = new CardService();
        var pinService = new PinService();
        var accountService = new AccountService();
        var limitService = new DailyLimitService();
        var fraudService = new FraudService();
        var cashDispenserService = new CashDispenserService();
        var journalService = new JournalService();
        var receiptService = new ReceiptService();

        var withdrawalFacade = new AtmWithdrawalFacade(
            cardService,
            pinService,
            accountService,
            limitService,
            fraudService,
            cashDispenserService,
            journalService,
            receiptService);

        var request = new WithdrawalRequest(
            CardNumber: "5295450812345678",
            Pin: "1234",
            AccountNumber: "TR-10001",
            Amount: 1_000m);

        WithdrawalResult result = withdrawalFacade.Withdraw(request);

        Console.WriteLine();
        Console.WriteLine("SONUÇ");
        Console.WriteLine("--------------------------------");

        if (result.IsSuccess)
        {
            Console.WriteLine($"İşlem başarılı.");
            Console.WriteLine($"İşlem numarası: {result.TransactionId}");
            Console.WriteLine($"Çekilen tutar: {result.Amount:N2} TL");
            Console.WriteLine($"Kalan bakiye: {result.RemainingBalance:N2} TL");
        }
        else
        {
            Console.WriteLine("İşlem başarısız.");
            Console.WriteLine($"Hata: {result.Message}");
        }

        Console.ReadLine();
    }
}

public sealed record WithdrawalRequest(
    string CardNumber,
    string Pin,
    string AccountNumber,
    decimal Amount);

public sealed record WithdrawalResult(
    bool IsSuccess,
    string Message,
    string? TransactionId = null,
    decimal Amount = 0,
    decimal RemainingBalance = 0)
{
    public static WithdrawalResult Success(
        string transactionId,
        decimal amount,
        decimal remainingBalance)
    {
        return new WithdrawalResult(
            IsSuccess: true,
            Message: "Para çekme işlemi tamamlandı.",
            TransactionId: transactionId,
            Amount: amount,
            RemainingBalance: remainingBalance);
    }

    public static WithdrawalResult Fail(string message)
    {
        return new WithdrawalResult(
            IsSuccess: false,
            Message: message);
    }
}

public sealed class AtmWithdrawalFacade
{
    private readonly CardService _cardService;
    private readonly PinService _pinService;
    private readonly AccountService _accountService;
    private readonly DailyLimitService _limitService;
    private readonly FraudService _fraudService;
    private readonly CashDispenserService _cashDispenserService;
    private readonly JournalService _journalService;
    private readonly ReceiptService _receiptService;

    public AtmWithdrawalFacade(
        CardService cardService,
        PinService pinService,
        AccountService accountService,
        DailyLimitService limitService,
        FraudService fraudService,
        CashDispenserService cashDispenserService,
        JournalService journalService,
        ReceiptService receiptService)
    {
        _cardService = cardService;
        _pinService = pinService;
        _accountService = accountService;
        _limitService = limitService;
        _fraudService = fraudService;
        _cashDispenserService = cashDispenserService;
        _journalService = journalService;
        _receiptService = receiptService;
    }

    public WithdrawalResult Withdraw(WithdrawalRequest request)
    {
        Console.WriteLine("ATM para çekme işlemi başladı.");
        Console.WriteLine("--------------------------------");

        if (request.Amount <= 0)
        {
            return WithdrawalResult.Fail(
                "Çekilecek tutar sıfırdan büyük olmalıdır.");
        }

        if (!_cardService.IsCardValid(request.CardNumber))
        {
            return WithdrawalResult.Fail(
                "Kart doğrulanamadı.");
        }

        if (!_pinService.ValidatePin(
                request.CardNumber,
                request.Pin))
        {
            return WithdrawalResult.Fail(
                "PIN hatalı.");
        }

        Account account =
            _accountService.GetAccount(request.AccountNumber);

        if (!_accountService.HasSufficientBalance(
                account,
                request.Amount))
        {
            return WithdrawalResult.Fail(
                "Hesap bakiyesi yetersiz.");
        }

        if (!_limitService.IsWithinDailyLimit(
                account,
                request.Amount))
        {
            return WithdrawalResult.Fail(
                "Günlük para çekme limiti aşıldı.");
        }

        if (!_fraudService.IsTransactionSafe(
                request.CardNumber,
                request.Amount))
        {
            return WithdrawalResult.Fail(
                "İşlem fraud kontrolünden geçemedi.");
        }

        if (!_cashDispenserService.CanDispense(
                request.Amount))
        {
            return WithdrawalResult.Fail(
                "ATM istenen tutarı veremiyor.");
        }

        string transactionId =
            Guid.NewGuid().ToString("N");

        try
        {
            _accountService.Debit(
                account,
                request.Amount);

            _cashDispenserService.Dispense(
                request.Amount);

            _limitService.RecordWithdrawal(
                account,
                request.Amount);

            _journalService.WriteSuccessfulTransaction(
                transactionId,
                request,
                account.Balance);

            _receiptService.Print(
                transactionId,
                request.Amount,
                account.Balance);

            return WithdrawalResult.Success(
                transactionId,
                request.Amount,
                account.Balance);
        }
        catch (Exception exception)
        {
            _journalService.WriteFailedTransaction(
                transactionId,
                request,
                exception.Message);

            return WithdrawalResult.Fail(
                $"İşlem sırasında hata oluştu: {exception.Message}");
        }
    }
}

public sealed class CardService
{
    public bool IsCardValid(string cardNumber)
    {
        bool isValid =
            !string.IsNullOrWhiteSpace(cardNumber) &&
            cardNumber.Length == 16;

        Console.WriteLine(
            isValid
                ? "1. Kart doğrulandı."
                : "1. Kart doğrulanamadı.");

        return isValid;
    }
}

public sealed class PinService
{
    public bool ValidatePin(
        string cardNumber,
        string pin)
    {
        bool isValid = pin == "1234";

        Console.WriteLine(
            isValid
                ? "2. PIN doğrulandı."
                : "2. PIN doğrulanamadı.");

        return isValid;
    }
}

public sealed class AccountService
{
    public Account GetAccount(string accountNumber)
    {
        Console.WriteLine("3. Hesap bilgileri getirildi.");

        return new Account(
            accountNumber,
            balance: 10_000m);
    }

    public bool HasSufficientBalance(
        Account account,
        decimal amount)
    {
        bool hasSufficientBalance =
            account.Balance >= amount;

        Console.WriteLine(
            hasSufficientBalance
                ? "4. Bakiye yeterli."
                : "4. Bakiye yetersiz.");

        return hasSufficientBalance;
    }

    public void Debit(
        Account account,
        decimal amount)
    {
        if (account.Balance < amount)
        {
            throw new InvalidOperationException(
                "Bakiye yetersiz.");
        }

        account.DecreaseBalance(amount);

        Console.WriteLine(
            $"8. Hesaptan {amount:N2} TL düşüldü.");
    }
}

public sealed class DailyLimitService
{
    private const decimal DailyWithdrawalLimit = 5_000m;

    private decimal _withdrawnToday = 1_000m;

    public bool IsWithinDailyLimit(
        Account account,
        decimal amount)
    {
        bool isWithinLimit =
            _withdrawnToday + amount <=
            DailyWithdrawalLimit;

        Console.WriteLine(
            isWithinLimit
                ? "5. Günlük limit uygun."
                : "5. Günlük limit aşıldı.");

        return isWithinLimit;
    }

    public void RecordWithdrawal(
        Account account,
        decimal amount)
    {
        _withdrawnToday += amount;

        Console.WriteLine(
            "10. Günlük çekim tutarı güncellendi.");
    }
}

public sealed class FraudService
{
    public bool IsTransactionSafe(
        string cardNumber,
        decimal amount)
    {
        bool isSafe = amount <= 4_000m;

        Console.WriteLine(
            isSafe
                ? "6. Fraud kontrolü başarılı."
                : "6. Fraud kontrolü başarısız.");

        return isSafe;
    }
}

public sealed class CashDispenserService
{
    private decimal _availableCash = 50_000m;

    public bool CanDispense(decimal amount)
    {
        bool canDispense =
            amount <= _availableCash &&
            amount % 10 == 0;

        Console.WriteLine(
            canDispense
                ? "7. ATM tutarı verebilir."
                : "7. ATM tutarı veremiyor.");

        return canDispense;
    }

    public void Dispense(decimal amount)
    {
        if (!CanDispenseWithoutLogging(amount))
        {
            throw new InvalidOperationException(
                "ATM'de yeterli para bulunmuyor.");
        }

        _availableCash -= amount;

        Console.WriteLine(
            $"9. ATM {amount:N2} TL verdi.");
    }

    private bool CanDispenseWithoutLogging(
        decimal amount)
    {
        return amount <= _availableCash &&
               amount % 10 == 0;
    }
}

public sealed class JournalService
{
    public void WriteSuccessfulTransaction(
        string transactionId,
        WithdrawalRequest request,
        decimal remainingBalance)
    {
        Console.WriteLine(
            $"11. Journal kaydı yazıldı. " +
            $"TransactionId: {transactionId}");
    }

    public void WriteFailedTransaction(
        string transactionId,
        WithdrawalRequest request,
        string errorMessage)
    {
        Console.WriteLine(
            $"Başarısız işlem journal'a yazıldı. " +
            $"Hata: {errorMessage}");
    }
}

public sealed class ReceiptService
{
    public void Print(
        string transactionId,
        decimal amount,
        decimal remainingBalance)
    {
        Console.WriteLine("12. Makbuz basıldı.");
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"İşlem No : {transactionId}");
        Console.WriteLine($"Tutar     : {amount:N2} TL");
        Console.WriteLine(
            $"Bakiye    : {remainingBalance:N2} TL");
    }
}

public sealed class Account
{
    public Account(
        string accountNumber,
        decimal balance)
    {
        AccountNumber = accountNumber;
        Balance = balance;
    }

    public string AccountNumber { get; }

    public decimal Balance { get; private set; }

    public void DecreaseBalance(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount));
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException(
                "Bakiye yetersiz.");
        }

        Balance -= amount;
    }
}