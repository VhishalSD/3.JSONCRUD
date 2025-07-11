using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using JSONCRUD.Models;

namespace JSONCRUD.Utilities
{
    public class JsonFileHandler
    {
        public List<Person> LoadFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<Person>();
                }

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error loading data from JSON file: " + ex.Message);
                Console.ResetColor();
                return new List<Person>();
            }
        }

        public void SaveToJson(string filePath, List<Person> people)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath) ?? ".";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error saving data to JSON file: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}

