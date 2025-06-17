using MauiProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiProject.Services
{
    public class StudentDataService
    {
        public async Task<List<Student>> LoadStudentsDataAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("students_data.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var students = JsonConvert.DeserializeObject<List<Student>>(json);
            return students;
        }
    }
}