using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Models
{
    public class Student
    {
        public string id { get; set; }
        public string ObjType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public double AvgGrade { get; set; }
        public Adress[] Adresses { get; set; }
        public Course[] courses { get; set; }

        public static List<Student> ConvertStringIntoList(string studentsAsList)
        {
            if (string.IsNullOrEmpty(studentsAsList)) return new List<Student>();
            return System.Text.Json.JsonSerializer.Deserialize<List<Student>>(studentsAsList);
        }
        public override string ToString()
        {
            string studentDataStr = string.Empty;
            studentDataStr += "Hi im student #" + id;
            studentDataStr += (string.IsNullOrEmpty(FirstName)) ?
                               "\nNo first name documented at the moment" :
                               "\nMy first name is:" + FirstName;
            studentDataStr += (string.IsNullOrEmpty(LastName)) ?
                               "\nNo first name documented at the moment" :
                               "\nMy last name is:" + LastName;
            int addCounter = 0;
            string addrinfo = string.Empty;
            if (Adresses != null && Adresses.Length > 0)
            {
                for (int i = 0; i < Adresses.Length; i++)
                {
                    if (Adresses[i] != null)
                    {
                        addCounter++;
                        addrinfo += Adresses[i].ToString();

                    }
                }
            }
            studentDataStr += (addCounter == 0) ?
                            "\nNo addresses are documented at the moment" :
                            $"\n{addCounter} addresses are docoemnted: {addrinfo}";
            return studentDataStr;
        }
        public Student()
        {
            ObjType = GetType().Name;
        }
    }
}
