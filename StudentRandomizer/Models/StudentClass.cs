using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRandomizer.Models
{
    public class StudentClass
    {
        public List<Student> students { get; set; }
        public string Name { get; set; }

        public Student GetRandomStudent()
        {
            return students[new Random().Next(students.Count)];
        }
    }
}
