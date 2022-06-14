using System;
using System.Collections.Generic;
using System.Text;

namespace RAP2.Research
{
    public enum OutputType { Conference, Journal, Other }

    //Class for Publications
    public class Publication
    {
        public string DOI { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public int Year { get; set; }
        public OutputType Type { get; set; }
        public string CiteAS { get; set; }
        public DateTime Available { get; set; }
        public int Age { get { return (DateTime.Today - Available).Days; } }
        public override string ToString()
        {
            return String.Format("{0} {1}", Year, Title);
        }
    }
    public class PublicationYearCount
    {
        public int Year;
        public int Count;
        public override string ToString()
        {
            return String.Format("{0} {1}", Year, Count);
        }
    }

}