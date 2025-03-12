using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRandomizer.Models
{
    class ClassManager
    {
        public static string currentClass;
        public static int todaysLuckyNumber;

        public static void ChangeCurrentClass(string selectedClass)
        {
            currentClass = Path.Combine(Path.Combine(FileSystem.AppDataDirectory, $"{selectedClass}.txt")); ;
        }
        public static void GenerateTodaysLuckyNumber()
        {
            todaysLuckyNumber = (int)((Math.Sin(DateTime.Now.Month) % 0.1) * DateTime.Now.Day + DateTime.Now.Year) % 34;
        }
    }
}
