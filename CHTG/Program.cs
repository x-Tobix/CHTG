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

            Console.Read();
        }
    }
}
