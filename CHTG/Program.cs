using CHTG.Services;
using CHTG.Services.Interfaces;
using CHTG.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CHTG
{
    class Program
    {
        static void Main(string[] args)
        {
            string algorithm;
            bool isPicked = false;

            do
            {
                Console.WriteLine("Wybierz algorytm:");
                Console.WriteLine("1 - Algorytm zachłanny.");
                Console.WriteLine("2 - Algorytm przeszukujący.");
                Console.WriteLine("3 - Algorytm sąsiedzki.");
                algorithm = Console.ReadLine();
                if (algorithm == "1" || algorithm == "2" || algorithm == "3")
                {
                    isPicked = true;
                }
                else
                {
                    Console.WriteLine("Wybrałeś nieistniejący algorytm. Spróbuj ponownie.");
                }
            }
            while (!isPicked);

            var serviceProvider = new ServiceCollection()
            .AddTransient<ISolvable, OCNService>()
            .AddSingleton<CSVParser>()
            .BuildServiceProvider();

            serviceProvider.GetService<ISolvable>().FindOrientedChromaticNumber(Convert.ToInt32(algorithm), args[0]);

            Console.WriteLine("Wciśnij enter aby zakończyć sesję.");
            Console.ReadLine();
        }
    }
}
