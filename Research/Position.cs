using System;
using System.Collections.Generic;
using System.Text;
namespace RAP2.Research
{

    public enum EmploymentLevel { Student, A, B, C, D, E, All }

    //Class for Occupation/Position
    public class Position
    {
        public EmploymentLevel Level { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public override string ToString()
        {
            return String.Format("{0} {1} - {2}", Researcher.LevelToTitle(Level), Start.ToShortDateString(), End.ToShortDateString());
        }

    }

}
