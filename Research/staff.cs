using System;
using System.Collections.Generic;
using System.Text;

namespace RAP2.Research
{
    //Class for Staff type of Researcher
    public class staff : Researcher
    {
        public List<Position> PreviousPositions { get; set; }
        public List<Supervision> Supervisions { get; set; }
        public float ThreeyearAverage { get { return CalcThreeYearAverage(); } }
        public string Performance { get { return CalcPerformance(); } }
        public int SupervisionCount { get { return CalcSupervisionCount(); } }
        public staff()
        {

        }
        public staff(Researcher b)
        {
            //ThreeyearAverage = 0;
            //Performance = 0;
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
        public string CalcPerformance()
        {
            float perf,calc;
            switch (Level)
            {
                case EmploymentLevel.A:
                    perf = 0.5F;
                    break;
                case EmploymentLevel.B:
                    perf = 1.0F;
                    break;
                case EmploymentLevel.C:
                    perf = 2.0F;
                    break;
                case EmploymentLevel.D:
                    perf = 3.2F;
                    break;
                case EmploymentLevel.E:
                    perf = 4.0F;
                    break;
                default:
                    perf = 0.0F;
                    break;
            }
            
            calc = ((float)Math.Round(((ThreeyearAverage / perf) * 100), 1));
            if (calc > 0.0)
            {
                return calc.ToString();
            } else
            {
                return "n/a";
            }
        }
        public float CalcThreeYearAverage()
        {
            float cnt = 0;
            foreach (Publication pub in Publications)
            {
                if ((pub.Year >= (DateTime.Now.Year) - 3) && (pub.Year < (DateTime.Now.Year)))
                {
                    cnt = cnt + 1;
                }
            }
            return (float)(Math.Round((cnt / 3), 1));
        }
        public int CalcSupervisionCount()
        {
            return Supervisions.Count;
        }
    }
    public class Supervision
    {
        public int id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return String.Format("{0} {1}", id, name);
        }
    }


}
