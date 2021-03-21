using QueryLanguage.Analysis;
using QueryLanguage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QueryLanguage
{
    class Program
    {
        static async Task Main()
        {
            var allCountries = new List<Country>();
            LanguageProcessor languageProcessor = new LanguageProcessor();

            while (!allCountries.Any())
            {
                Console.WriteLine("Please enter a path to the test data (or press Enter for default):");
                string file = Console.ReadLine();
                if (string.IsNullOrEmpty(file))
                    file = "Data/Countries.json";

                allCountries = await GetCountriesAsync(file);
            }

            Console.WriteLine("Data set:");
            Console.WriteLine(Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(allCountries, options: new JsonSerializerOptions() { WriteIndented = true })));

            do
            {
                Console.WriteLine();
                Console.WriteLine("Please enter expression, e.g. 'Name eq Australia or Population ge 100 and Population lt 201'");
                Console.WriteLine($"Available operators: { string.Join(',', Enum.GetValues<Operator>())}");
                string expression = Console.ReadLine();
                if (string.IsNullOrEmpty(expression))
                {
                    break;
                }

                try
                {
                    var countries = languageProcessor.Find(allCountries, expression);
                    if (countries.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Matching countries:");
                        foreach (var country in countries)
                        {
                            Console.Write($"{country.Name}; ");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find any matching countries");
                    }

                }
                catch (ExpressionInvalidException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.Message}: {ex.Details}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Something went wrong, please try again");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
            while (true);
        }

        private static async Task<List<Country>> GetCountriesAsync(string file)
        {
            try
            {
                using var openStream = File.OpenRead(file);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                return await JsonSerializer.DeserializeAsync<List<Country>>(openStream, options);
            }
            catch
            {
                Console.WriteLine($"Coudln't read countries");
                return null;
            }
        }
    }
}
