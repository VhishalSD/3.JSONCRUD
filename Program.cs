using System;
using JSONCRUD.Services;
using JSONCRUD.Utilities;

class Program
{
    static void Main()
    {
        var jsonHandler = new JsonFileHandler();
        var service = new PersonServices(jsonHandler);

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("{0," + ((40 + "PERSON MANAGEMENT MENU".Length) / 2) + "}", "PERSON MANAGEMENT MENU");
            Console.WriteLine(new string('=', 40));
            Console.ResetColor();

            Console.WriteLine("1.  Add Person");
            Console.WriteLine("2.  View Person by ID");
            Console.WriteLine("3.  Update Person");
            Console.WriteLine("4.  Delete Person");
            Console.WriteLine("5.  Show All Persons");
            Console.WriteLine("6.  Search by Name");
            Console.WriteLine("7.  Sort Persons");
            Console.WriteLine("8.  Show Statistics");
            Console.WriteLine("9.  Save and Exit");
            Console.WriteLine(new string('-', 40));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Select an option (1-9): ");
            Console.ResetColor();

            string? input = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    service.CreatePerson();
                    break;
                case "2":
                    service.ReadPerson();
                    break;
                case "3":
                    service.UpdatePerson();
                    break;
                case "4":
                    service.ConfirmDeletePerson();
                    break;
                case "5":
                    service.ReadAllPersons();
                    break;
                case "6":
                    service.SearchByName();
                    break;
                case "7":
                    service.SortPersonsMenu();
                    break;
                case "8":
                    service.ShowStatistics();
                    break;
                case "9":
                    service.SaveAndExit();
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please enter a number between 1 and 9.");
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
