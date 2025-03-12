using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRandomizer.Models
{
    public class Student
    {
        public Student (string name, string surname)
        {
            this.name = name;
            this.surname = surname;
            isPresent = true;
            number = 0;
            chosenQueueIndex = 0;
        }
        public string name { get; set; }
        public string surname { get; set; }
        public bool isPresent { get; set; }
        public int number { get; set; }
        public int chosenQueueIndex { get; set; }
    }
}
