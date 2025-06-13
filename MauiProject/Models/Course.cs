using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiProject.Models
{
    public class Course
    {
        public int Year { get; set; }
        public string CourseName { get; set; }
        public List<Grade> Grades { get; set; }
    }
}
