using Colors.Net;
using Colors.Net.StringColorExtensions;
using ConsoleTables;
using Hw14.Configuration;
using Hw14.Contracts;
using Hw14.Entities;
using Hw14.Repositories;
using Hw14.Services;
using static Hw14.Repositories.TransactionRepository;

ICardService cardService = new CardService(new CardRepository(new BankDbContext()));
ITransactiionService transactionService = new TransactionService(new TransactionRepository(new BankDbContext()), new CardRepository(new BankDbContext()));
bool loggedIn = false;

while (true)
{
    Console.Clear();
    ColoredConsole.WriteLine("********* Welcome to Bank System *********".DarkGreen());
    ColoredConsole.WriteLine("1.Login".DarkBlue());
    ColoredConsole.WriteLine("2.Exit".DarkRed());

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ColoredConsole.WriteLine("Enter Card Number: ".DarkYellow());
            string cardNumber = Console.ReadLine();
            ColoredConsole.WriteLine("Enter Password: ".DarkYellow());
            string password = Console.ReadLine();

            try
            {
                cardService.Login(cardNumber, password);
                var currentCard = cardService.GetCurrentCard();
                if (currentCard == null)
                {
                    ColoredConsole.WriteLine("User Not Found||Invalid Password".DarkRed());
                    Console.ReadKey();
                    break;
                }
                if(currentCard.Password != password)
                {
                    ColoredConsole.WriteLine("User Not Found||Invalid Password".DarkRed());
                    Console.ReadKey();
                    break;
                }
               
                loggedIn = true;
                ColoredConsole.WriteLine("Login Successful".DarkGreen());
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ColoredConsole.WriteLine($"Error: {ex.Message}".DarkRed());
                Console.ReadKey();
            }

            break;

        case "2":
            return;

        default:
            ColoredConsole.WriteLine("Invalid Choice".DarkRed());
            Console.ReadKey();
            break;
    }

    if (loggedIn)
    {
        var currentCard = cardService.GetCurrentCard();
        try
        {
            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                ColoredConsole.WriteLine("********* Bank Menu *********".DarkGreen());
                ColoredConsole.WriteLine("1.View User Transactions".DarkBlue());
                ColoredConsole.WriteLine("2.Fund Transfer".DarkBlue());
                ColoredConsole.WriteLine("3.Show Balance".DarkBlue());
                ColoredConsole.WriteLine("4.Exit".DarkRed());


                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        var transactions = transactionService.GetTransactionsByCardNumber(currentCard.CardNumber);
                        if (transactions.Any())
                        {
                            var table = ConsoleTable.From<Transactiion>(transactions);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            table.Write();
                            Console.ResetColor();
                        }
                        else
                        {
                            ColoredConsole.WriteLine("No Transaction History Found".DarkRed());
                        }
                        Console.ReadKey();
                        break;

                    case "2":
                        ColoredConsole.WriteLine("Enter Amount to Transfer: ".DarkYellow());
                        float amount = float.Parse(Console.ReadLine());
                        ColoredConsole.WriteLine("Enter Recipient Card Number: ".DarkYellow());
                        string recipientCardNumber = Console.ReadLine();

                        bool transferSuccess = transactionService.TransferFunds(currentCard.CardNumber, recipientCardNumber, amount);
                        if (transferSuccess)
                        {
                            ColoredConsole.WriteLine("Transfer Successful".DarkGreen());
                            ColoredConsole.WriteLine($"Balance {currentCard.Balance}$-{amount}$".DarkBlue());

                        }
                        else
                        {
                            ColoredConsole.WriteLine("Transfer Failed".DarkRed());
                        }
                        Console.ReadKey();
                        break;

                    case "3":
                        try
                        {
                            var balance = cardService.GetCardBalance(currentCard.CardNumber);
                            ColoredConsole.WriteLine($"Your Balance: {balance}$".DarkGreen());
                        }
                        catch (Exception ex)
                        {
                            ColoredConsole.WriteLine($"Error: {ex.Message}".DarkRed());
                        }
                        Console.ReadKey();
                        break;
                    case "4":
                        cardService.Logout();
                        inMenu = false;
                        ColoredConsole.WriteLine("Logged Out".DarkRed());
                        Console.ReadKey();
                        break;

                    default:
                        ColoredConsole.WriteLine("Invalid Option".DarkRed());
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ColoredConsole.WriteLine($"Error: {ex.Message}".DarkRed());
            Console.ReadKey();
        }
    }
}
