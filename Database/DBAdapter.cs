using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using RAP2.Research;

namespace RAP2.Database
{
    //Adapter class for Database operations
    abstract class DBAdapter
    {
        private const string db = "";
        private const string user = "";
        private const string pass = "";
        private const string server = "";
        private static bool reportingErrors = true;
        private static MySqlConnection conn = null;

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        //Connect to database
        public static int Connect()
        {
            string connectString = String.Format(
                "Database={0};Data Source={1};User Id={2};Password={3}",
                db, server, user, pass);
            conn = new MySqlConnection(connectString);
            try
            {
                conn.Open();
            }
            catch (MySqlException e)
            {
                ReportError("Error opening connection - ", e);
                return -1;
            }
            return 0;
        }

        //Fetch list of researcher objects in database
        public static List<Researcher> FetchBasicResearcherlist()
        {
            string sql = "SELECT type, given_name, family_name, title, id, level, supervisor_id FROM researcher ORDER BY family_name";
            if (Connect() != 0)
            {
                Console.WriteLine("Connection failed");
                return null;
            }

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            Researcher r = new Researcher();


            List<Researcher> templist = new List<Researcher>();
            try
            {
                while (rdr.Read())
                {
                    string Researcher_Type = rdr[0].ToString();
                    r.Givenname = rdr[1].ToString();
                    r.Familyname = rdr[2].ToString();
                    r.Title = rdr[3].ToString();
                    r.id = (int)rdr[4];
                    r.Level = EmploymentLevel.Student;
                    r.Name = r.Givenname + " " + r.Familyname;

                    if (Researcher_Type == "Staff")
                    {
                        staff a = new staff(r);
                        a.Level = ParseEnum<EmploymentLevel>(rdr[5].ToString());
                        templist.Add(a);
                    }
                    else
                    {
                        Student a = new Student(r);
                        a.Supervisor = (int)rdr[6];
                        templist.Add(a);
                    }
                }
            }
            catch (MySqlException e)
            {
                ReportError("Error connecting to database: ", e);
            }
            finally
            {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }

            return templist;
        }

        //Fetch complete details of Researcher object 
        public static Researcher fetchFullResearcher(int id)
        {
            string sql = "SELECT type, given_name, family_name, title, id, level, campus, unit, email, photo, degree, " +
                "supervisor_id, utas_start, current_start FROM researcher as r " +
                "where r.id=?id";
            if (Connect() != 0)
            {
                Console.WriteLine("Connection failed");
                return null;
            }
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            MySqlDataReader rdr = cmd.ExecuteReader();

            Researcher r = new Researcher();
            try
            {
                while (rdr.Read())
                {
                    string Researcher_Type = rdr[0].ToString();
                    r.Givenname = rdr[1].ToString();
                    r.Familyname = rdr[2].ToString();
                    r.Title = rdr[3].ToString();
                    r.id = (int)rdr[4];
                    r.Level = EmploymentLevel.Student;
                    r.Name = r.Givenname + " " + r.Familyname;

                    r.Campus = rdr[6].ToString();
                    r.Unit = rdr[7].ToString();
                    r.Email = rdr[8].ToString();
                    Uri uri = new Uri(rdr[9].ToString());
                    r.Photo = uri;
                    r.StartAtUtas = (DateTime)rdr[12];
                    r.JobStart = (DateTime)rdr[13];
                    //researcher = (Researcher)student;

                    if (Researcher_Type == "Staff")
                    {
                        staff a = new staff(r);
                        a.Level = ParseEnum<EmploymentLevel>(rdr.GetString(5));
                        //a.StartAtUtas = (DateTime)rdr[12];
                        //a.JobStart = (DateTime)rdr[13];
                        r = (Researcher)a;
                    }
                    else
                    {
                        Student a = new Student(r);
                        a.Degree = rdr[10].ToString();
                        a.Supervisor = (int)rdr[11];
                        r = (Researcher)a;
                    }
                }
            }
            catch (MySqlException e)
            {
                ReportError("Failed to get from database: ", e);
            }
            finally
            {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }

            return r;
        }


        //fetch list of publications
        public static List<Publication> fetchBasicPublicationsDetails(Researcher rs)
        {
            int id = rs.id;

            string sql = "SELECT p.doi, p.title, p.year FROM publication AS p, " +
                         "researcher_publication AS r WHERE p.doi=r.doi AND " +
                         "researcher_id=?id ORDER BY year desc, title";
            if (Connect() != 0)
            {
                Console.WriteLine("Connection failed");
                return null;
            }

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<Publication> publications = new List<Publication>();

            try
            {
                while (rdr.Read())
                {
                    Publication temp_pub = new Publication();

                    temp_pub.DOI = rdr[0].ToString();
                    temp_pub.Title = rdr[1].ToString();
                    temp_pub.Year = (int)rdr[2];

                    publications.Add(temp_pub);
                }
            }
            catch (MySqlException e)
            {
                ReportError("Error connecting publication database: ", e);
            }
            finally
            {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }

            return publications;
        }

        //fetch complete details of a publication
        public static Publication completePublicationDetails(Publication pub)
        {
            string doi = pub.DOI;
            string sql = "SELECT authors, available, type, cite_as FROM publication AS p WHERE p.doi=?doi";
            if (Connect() != 0)
            {
                Console.WriteLine("Connection failed");
                return null;
            }

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("doi", doi);
            MySqlDataReader rdr = cmd.ExecuteReader();

            try
            {
                while (rdr.Read())
                {

                    pub.Authors = rdr[0].ToString();
                    pub.Available = (DateTime)rdr[1];
                    pub.Type = (OutputType)Enum.Parse(typeof(OutputType), rdr[2].ToString());
                    pub.CiteAS = rdr[3].ToString();
                }
            }
            catch (MySqlException e)
            {
                ReportError("Error connecting publication database: ", e);
            }
            finally
            {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }
            return pub;
        }

        //fetch positions prior to current year
        public static List<Position> fetchLastPostions(Researcher rs)
        {
            int id = rs.id;


            string sql = "SELECT start, end, level FROM position AS p WHERE p.id=?id";

            if (Connect() != 0)
            {
                Console.WriteLine("Connection failed");
                return null;
            }

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<Position> positions = new List<Position>();

            try
            {
                while (rdr.Read())
                {
                    Position temp_p = new Position();
                    temp_p.Level = (EmploymentLevel)Enum.Parse(typeof(EmploymentLevel), rdr.GetString(2));
                    if (temp_p.Level != rs.Level)
                    {
                        temp_p.Start = rdr.GetDateTime(0);
                        temp_p.End = rdr.GetDateTime(1);
                        positions.Add(temp_p);
                    }
                }
            }
            catch (MySqlException e)
            {
                ReportError("Error connecting position database: ", e);
            }
            finally
            {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }

            return positions;
        }

        //Report Error to the user with a Messagebox
        private static void ReportError(string msg, Exception e)
        {
            if (reportingErrors)
            {
                MessageBox.Show("An error occurred while " + msg + ". Try again by closing and restarting program.\n\nError Details:\n" + e,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
