using Parser;
using System;
using System.IO;

namespace MapTrailFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[---------------- " +
                "Ab's Post-Audio Object Deleter Tool Thing to Fix Things (TM)" +
                " ----------------]\n\n");
            string location;
            if (args?.Length > 0)
            {
                location = args[0];
            }
            else
            {
                Console.WriteLine("Enter the map path. (Hint: Drag and drop the folder over the exe next time)");
                location = Console.ReadLine();
            }
            if (!Directory.Exists(location))
            {
                Console.WriteLine($"Couldn't find directory {location}.");
                Console.ReadKey();
                return;
            }
            var fixer = new MapFixer(location);
            var result = fixer.FixAll().GetAwaiter().GetResult();
            Console.WriteLine($"\n\n\n\n[---- DONE ----] \n\nRemoved {result} post-song objects.");
            Console.ReadKey();
        }
    }
}
