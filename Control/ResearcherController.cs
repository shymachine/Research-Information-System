using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RAP2.Research;
using RAP2.Database;
namespace RAP2.Control
{
    //Controller class for Researcher
    class ResearcherController
    {
        private List<Researcher> researchers;



        private ObservableCollection<Researcher> viewableStaff;
        public ObservableCollection<Researcher> VisibleStaff
        {
            get { return viewableStaff; }
            set { }
        }
        public List<Researcher> LoadResearchers()
        {
            return (DBAdapter.FetchBasicResearcherlist());
        }
        //constructor
        public ResearcherController()
        {
            researchers = LoadResearchers();
            viewableStaff = new ObservableCollection<Researcher>(researchers);
        }

        //List visible to View class
        public ObservableCollection<Researcher> ViewableList()
        {
            return VisibleStaff;
        }

        //filter view list
        public void FilterBy(EmploymentLevel level)
        {
            viewableStaff.Clear();
            if ((level == EmploymentLevel.A) ||
                (level == EmploymentLevel.B) ||
                (level == EmploymentLevel.C) ||
                (level == EmploymentLevel.D) ||
                (level == EmploymentLevel.E) ||
                (level == EmploymentLevel.Student))
            {
                var selected = from Researcher e in researchers
                               where e.Level == level
                               select e;
                selected.ToList().ForEach(viewableStaff.Add);
            }
            else
            {
                //show every one
                var selected = from Researcher e in researchers
                               select e;
                selected.ToList().ForEach(viewableStaff.Add);
            }
        }

        public void FilterByName(string str)
        {
            viewableStaff.Clear();
            var selected = from Researcher e in researchers
                           where ((e.Givenname.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0) || (e.Familyname.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0))
                           select e;

            selected.ToList().ForEach(viewableStaff.Add);
        }

        //Load details of Researcher object from DB
        public Researcher LoadResearcherDetails(int id)
        {
            Researcher r;
            PublicationsController pub_ctrl = new PublicationsController();
            var res = (from Researcher t in researchers
                       where t.id == id
                       select t).FirstOrDefault();
            if (res.fullyLoaded == 0)
            {
                r = DBAdapter.fetchFullResearcher(id);

                res.Title = r.Title;
                res.Givenname = r.Givenname;
                res.Familyname = r.Familyname;
                res.Name = r.Givenname + " " + r.Familyname;
                res.Email = r.Email;
                res.Photo = r.Photo;
                res.Unit = r.Unit;

                res.Campus = r.Campus;

                res.Level = r.Level;
                res.JobTitle = Researcher.LevelToTitle(r.Level);

                res.JobStart = r.JobStart;
                res.StartAtUtas = r.StartAtUtas;

                if (res.Level == EmploymentLevel.Student)
                {
                    ((Student)res).Degree = ((Student)r).Degree;
                }

                res.Publications = pub_ctrl.LoadPublicationsFor(res);
                if (res.PublicationCount > 0)
                {
                    res.PYearCountList = new List<PublicationYearCount>();
                    foreach (var line in res.Publications.GroupBy(info => info.Year)
                        .Select(group => new
                        {
                            Year = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(x => x.Year))
                    {
                        PublicationYearCount py = new PublicationYearCount();
                        py.Year = line.Year;
                        py.Count = line.Count;
                        res.PYearCountList.Add(py);
                    }
                }

                /*foreach (Publication pub in res.Publications)
                {
                    int counter = 0;
                    foreach (Publication p in res.Publications)
                    {
                        if (p.Year == pub.Year)
                        {
                            counter = counter+1;
                        }
                    }
                    PublicationYearCount py = new PublicationYearCount();
                    py.Year = pub.Year;
                    py.Count = counter;
                    res.PYearCountList.Add(py);
                }*/

                if (res.Level != EmploymentLevel.Student)
                {
                    ((staff)res).PreviousPositions = DBAdapter.fetchLastPostions(res);
                    ((staff)res).Supervisions = new List<Supervision>();
                    foreach (Researcher rs in researchers)
                    {
                        if ((rs.Level == EmploymentLevel.Student) && (((Student)rs).Supervisor == res.id))
                        {
                            Supervision sp = new Supervision();
                            sp.id = ((Student)rs).Supervisor;
                            sp.name = rs.Name;
                            ((staff)res).Supervisions.Add(sp);
                        }
                    }
                }
                res.fullyLoaded = 1;
            }

            return res;
        }

        public void Display()
        {
            researchers.ForEach(Console.WriteLine);

        }

    }

}
