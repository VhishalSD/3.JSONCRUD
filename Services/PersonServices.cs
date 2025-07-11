using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JSONCRUD.Models;
using JSONCRUD.Utilities;

namespace JSONCRUD.Services
{
    public class PersonServices
    {
        private readonly JsonFileHandler _jsonHandler;
        private List<Person> _people;

        public PersonServices(JsonFileHandler jsonHandler)
        {
            _jsonHandler = jsonHandler;
            _people = _jsonHandler.LoadFromJson("Data/people.json");
        }

        public void CreatePerson()
        {
            string firstName = ReadValidatedText("Enter First Name: ");
            string lastName = ReadValidatedText("Enter Last Name: ");
            DateTime birthDate = ReadValidatedDate("Enter Birthdate (dd-MM-yyyy): ");
            string department = ReadValidatedText("Enter Department: ");
            string company = ReadValidatedText("Enter Company: ");

            int newId = 1;
            while (_people.Any(p => p.Id == newId))
            {
                newId++;
            }

            var newPerson = new Person
            {
                Id = newId,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Department = department,
                Company = company
            };

            _people.Add(newPerson);
            Console.WriteLine("Person created successfully.");
        }

        private string ReadValidatedText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    ShowError("Input cannot be empty.");
                    continue;
                }

                if (input.Length > 30)
                {
                    ShowError("Maximum 30 characters allowed.");
                    continue;
                }

                // Toegestane tekens: letters (incl. accenten), spaties, koppeltekens, apostrof, punt
                bool isValid = input.All(c =>
                    char.IsLetter(c) || c == ' ' || c == '-' || c == '\'' || c == '.');

                if (!isValid)
                {
                    ShowError("Only letters, spaces, hyphens (-), apostrophes (') and dots (.) are allowed.");
                    continue;
                }

                // Eerste letter van elk woord met hoofdletter (met accenten behouden)
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                string capitalized = textInfo.ToTitleCase(input.ToLower());

                return capitalized;
            }
        }

        private DateTime ReadValidatedDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (DateTime.TryParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    var minDate = DateTime.Today.AddYears(-120);
                    var maxDate = DateTime.Today;

                    if (date < minDate || date > maxDate)
                    {
                        ShowError($"Birthdate must be between {minDate:dd-MM-yyyy} and {maxDate:dd-MM-yyyy}.");
                        continue;
                    }

                    return date;
                }

                ShowError("Invalid date format. Please use dd-MM-yyyy.");
            }
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void ReadPerson()
        {
            Console.Write("Enter Person Id to read: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ShowError("Invalid input. Enter a valid number.");
                return;
            }

            var person = _people.FirstOrDefault(p => p.Id == id);

            if (person == null)
            {
                ShowError("Person not found.");
                return;
            }

            DisplayPerson(person);
        }

        // Helper functie voor validatie en capitalisatie in UpdatePerson:
        private bool IsValidName(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length > 30) return false;
            return input.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\'' || c == '.');
        }

        public void UpdatePerson()
        {
            Console.Write("Enter Person Id to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ShowError("Invalid Id input.");
                return;
            }

            var person = _people.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                ShowError("Person not found.");
                return;
            }

            Console.Write($"New First Name (current: {person.FirstName}): ");
            string firstName = Console.ReadLine()?.Trim() ?? "";
            if (IsValidName(firstName))
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                person.FirstName = textInfo.ToTitleCase(firstName.ToLower());
            }

            Console.Write($"New Last Name (current: {person.LastName}): ");
            string lastName = Console.ReadLine()?.Trim() ?? "";
            if (IsValidName(lastName))
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                person.LastName = textInfo.ToTitleCase(lastName.ToLower());
            }

            Console.Write($"New Birthdate (dd-MM-yyyy) (current: {person.BirthDate:dd-MM-yyyy}): ");
            string bdInput = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(bdInput))
            {
                if (DateTime.TryParseExact(bdInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bd))
                {
                    var minDate = DateTime.Today.AddYears(-120);
                    var maxDate = DateTime.Today;

                    if (bd < minDate || bd > maxDate)
                    {
                        ShowError($"Birthdate must be between {minDate:dd-MM-yyyy} and {maxDate:dd-MM-yyyy}. Birthdate not updated.");
                    }
                    else
                    {
                        person.BirthDate = bd;
                    }
                }
                else
                {
                    ShowError("Invalid date format. Birthdate not updated.");
                }
            }

            Console.Write($"New Department (current: {person.Department}): ");
            string department = Console.ReadLine()?.Trim() ?? "";
            if (IsValidName(department))
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                person.Department = textInfo.ToTitleCase(department.ToLower());
            }

            Console.Write($"New Company (current: {person.Company}): ");
            string company = Console.ReadLine()?.Trim() ?? "";
            if (IsValidName(company))
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                person.Company = textInfo.ToTitleCase(company.ToLower());
            }

            Console.WriteLine("Person updated successfully.");
        }

        public void ConfirmDeletePerson()
        {
            Console.Write("Enter Person Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ShowError("Invalid Id input.");
                return;
            }

            var person = _people.FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                ShowError("Person not found.");
                return;
            }

            Console.Write($"Are you sure you want to delete {person.FirstName} {person.LastName}? (y/n): ");
            string confirmation = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (confirmation == "y")
            {
                _people.Remove(person);
                Console.WriteLine("Person deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete action canceled.");
            }
        }

        public void SaveAndExit()
        {
            _jsonHandler.SaveToJson("Data/people.json", _people);
            Console.WriteLine("Data saved. Exiting program.");
        }

        public void ReadAllPersons()
        {
            if (!_people.Any())
            {
                ShowError("No persons found.");
                return;
            }

            foreach (var person in _people)
            {
                DisplayPerson(person);
            }
        }

        public void SearchByName()
        {
            Console.Write("Enter name or part of name to search: ");
            string? searchTerm = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ShowError("Search term cannot be empty.");
                return;
            }

            var results = _people.Where(p =>
                p.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!results.Any())
            {
                ShowError("No matching persons found.");
                return;
            }

            foreach (var person in results)
            {
                DisplayPerson(person);
            }
        }

        private void DisplayPerson(Person person)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"ID         : {person.Id}");
            Console.WriteLine($"First Name : {person.FirstName}");
            Console.WriteLine($"Last Name  : {person.LastName}");
            Console.WriteLine($"Birthdate  : {person.BirthDate:dd-MM-yyyy}");
            Console.WriteLine($"Department : {person.Department}");
            Console.WriteLine($"Company    : {person.Company}");
            Console.WriteLine(new string('-', 40));
            Console.ResetColor();
        }

        public void SortPersonsMenu()
        {
            if (!_people.Any())
            {
                ShowError("No persons found.");
                return;
            }

            Console.WriteLine("Sort persons by:");
            Console.WriteLine("1. Name (First Name, Last Name)");
            Console.WriteLine("2. Birthdate");
            Console.WriteLine("3. Id");

            Console.Write("Select an option (1-3): ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            List<Person> sortedList = choice switch
            {
                "1" => _people.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList(),
                "2" => _people.OrderBy(p => p.BirthDate).ToList(),
                "3" => _people.OrderBy(p => p.Id).ToList(),
                _ => null
            };

            if (sortedList == null)
            {
                ShowError("Invalid option for sorting.");
                return;
            }

            foreach (var person in sortedList)
            {
                DisplayPerson(person);
            }
        }

        public void ShowStatistics()
        {
            int totalPersons = _people.Count;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("{0," + ((40 + "STATISTICS".Length) / 2) + "}", "STATISTICS");
            Console.WriteLine(new string('=', 40));

            Console.WriteLine($"Total number of persons: {totalPersons}");

            Console.WriteLine(new string('=', 40));
            Console.ResetColor();
        }
    }
}
