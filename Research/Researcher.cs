using System;
using System.Collections.Generic;
using System.Text;

namespace RAP2.Research
{
    //Class for Researcher
    public class Researcher
    {
        public List<Publication> Publications { get; set; }
        public List<PublicationYearCount> PYearCountList { get; set; }
        public int fullyLoaded = 0;
        public string Givenname { get; set; }
        public string Familyname { get; set; }
        public int id { get; set; }
        public string Title { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
        public Uri Photo { get; set; }
        public EmploymentLevel Level { get; set; }
        public string Name { get; internal set; }
        public string JobTitle { get; set; }
        public DateTime JobStart { get; set; }
        public DateTime StartAtUtas { get; set; }
        public string Unit { get; set; }
        public int PublicationCount { get { return CalPubCount(); } }

        public float Tenure
        {
            get
            {
                return (float)(Math.Round(((DateTime.Now - StartAtUtas).Days / 365.0), 1));
            }
        }
        public override string ToString()
        {
            return string.Format("{0},{1}({2})", Familyname, Givenname, Title);
        }
        //public Position CurrentPosition() { return null; }
        //public Position EarliestPosition() { return null; }
        public DateTime EarliestStart() { return DateTime.Today; }

        public static string LevelToTitle(EmploymentLevel level)
        {
            switch (level)
            {
                case EmploymentLevel.A:
                    return "Postdoc";
                case EmploymentLevel.B:
                    return "Lecturer";
                case EmploymentLevel.C:
                    return "Senior Lecturer";
                case EmploymentLevel.D:
                    return "Associate Professor";
                case EmploymentLevel.E:
                    return "Professor";
                case EmploymentLevel.Student:
                    return "Student";
                default:
                    break;
            }

            return "Level not valid!";
        }

        public int CalPubCount()
        {
            return Publications.Count;
        }
    }


}

