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
    //Controller class for Publications
    public class PublicationsController
    {
        public ObservableCollection<Publication> VisiblePubs { get; set; }
        public ObservableCollection<Publication> GetViewablePubs()
        {
            return VisiblePubs;
        }
        public PublicationsController()
        {
            VisiblePubs = new ObservableCollection<Publication>();
        }
        //get list of publications
        public List<Publication> LoadPublicationsFor(Researcher researcher)
        {
            researcher.Publications = DBAdapter.fetchBasicPublicationsDetails(researcher);
            return researcher.Publications;
        }

        //get detail of given publication
        public Publication getPubDetails(Publication pub)
        {
            return (DBAdapter.completePublicationDetails(pub));
        }
        public Researcher SortPubOrder(Researcher res, int order)
        {
            if (order == 0)
            {
                //descneding
                res.Publications = res.Publications.OrderByDescending(p => p.Year).ToList();
            }
            else
            {
                res.Publications = res.Publications.OrderBy(p => p.Year).ToList();
            }
            return res;
        }
        public void Display()
        {


        }
    }

}
