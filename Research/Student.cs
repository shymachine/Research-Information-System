using System;
using System.Collections.Generic;
using System.Text;

namespace RAP2.Research
{
    //Class for Student type of Researcher
    class Student : Researcher
    {
        public string Degree { get; set; }
        public int Supervisor { get; set; }
        public Student(Researcher b)
        {
            Degree = "NA";
            this.Campus = b.Campus;
            this.Email = b.Email;
            this.Familyname = b.Familyname;
            this.Givenname = b.Givenname;
            this.id = b.id;
            this.JobStart = b.JobStart;
            this.JobTitle = b.JobTitle;
            this.Level = b.Level;
            this.Name = b.Name;
            //this.PreviousPositions = b.PreviousPositions;
            this.Photo = b.Photo;
            this.Publications = b.Publications;
            this.StartAtUtas = b.StartAtUtas;
            this.Title = b.Title;
            this.Unit = b.Unit;
        }
    }

}
