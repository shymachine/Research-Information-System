using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RAP2.Control;
using RAP2.Research;

namespace RAP2.View
{
    //View or Window Class for front-end
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Researcher> showlist = new List<Researcher>();
        private ResearcherController controller;
        private PublicationsController pcontroller = new PublicationsController();
        private int pubOrder = 0; //Descending
        public MainWindow()
        {
            InitializeComponent();
            controller = (ResearcherController)(Application.Current.FindResource("rList") as ObjectDataProvider).ObjectInstance;
            if (controller == null)
            {
                MessageBox.Show("Error to initialize controller, restart again");
                Close();
            }

            pcontroller = (PublicationsController)(Application.Current.FindResource("pList") as ObjectDataProvider).ObjectInstance;
            if (pcontroller == null)
            {
                MessageBox.Show("Error to initialize pcontroller, restart again");
                Close();
            }

            FilterCombo.ItemsSource = Enum.GetValues(typeof(EmploymentLevel));
            pubTextBlock.Text = "";
            supervisionbox.Visibility = Visibility.Hidden;
            supervisionlabel.Visibility = Visibility.Hidden;
            supervisionslabel.Visibility = Visibility.Hidden;
            supervisionscontent.Visibility = Visibility.Hidden;
            SupervisionsButton.Visibility = Visibility.Hidden;
            avglabel.Visibility = Visibility.Hidden;
            avgtext.Visibility = Visibility.Hidden;
            degreelabel.Visibility = Visibility.Visible;
            degreecontent.Visibility = Visibility.Visible;
            perflabel.Visibility = Visibility.Hidden;
            perflabel2.Visibility = Visibility.Hidden;
            perftext.Visibility = Visibility.Hidden;
            supervisorcontent.Visibility = Visibility.Visible;
            supervisorlabel.Visibility = Visibility.Visible;
            poslabel.Visibility = Visibility.Hidden;
            positionList.Visibility = Visibility.Hidden;
            pubTextBlock.Visibility = Visibility.Hidden;
            CumulativeLabel.Visibility = Visibility.Hidden;
            CumulativeList.Visibility = Visibility.Hidden;
            CountCumulativeButton.Visibility = Visibility.Hidden;
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            supervisionbox.Visibility = Visibility.Hidden;
            supervisionlabel.Visibility = Visibility.Hidden;
            supervisionslabel.Visibility = Visibility.Hidden;
            supervisionscontent.Visibility = Visibility.Hidden;
            SupervisionsButton.Visibility = Visibility.Hidden;
            avglabel.Visibility = Visibility.Hidden;
            avgtext.Visibility = Visibility.Hidden;
            degreelabel.Visibility = Visibility.Visible;
            degreecontent.Visibility = Visibility.Visible;
            perflabel.Visibility = Visibility.Hidden;
            perflabel2.Visibility = Visibility.Hidden;
            perftext.Visibility = Visibility.Hidden;
            supervisorcontent.Visibility = Visibility.Visible;
            supervisorlabel.Visibility = Visibility.Visible;
            poslabel.Visibility = Visibility.Hidden;
            positionList.Visibility = Visibility.Hidden;
            pubTextBlock.Visibility = Visibility.Hidden;
            CumulativeLabel.Visibility = Visibility.Hidden;
            CumulativeList.Visibility = Visibility.Hidden;

            if (e.AddedItems.Count > 0)
            {
                pubTextBlock.Text = "";
                Researcher temp = e.AddedItems[0] as Researcher;

                Researcher r = controller.LoadResearcherDetails(temp.id);
                if (r == null)
                {
                    MessageBox.Show("Error while loading researcher details, restart the program and try again");
                    return;
                }
                if (r.PublicationCount > 0)
                {
                    CountCumulativeButton.Visibility = Visibility.Visible;
                }
                
                if (r.Level != EmploymentLevel.Student)
                {
                    supervisionslabel.Visibility = Visibility.Visible;
                    supervisionscontent.Visibility = Visibility.Visible;

                    staff s = (staff)r;
                    if (s.SupervisionCount > 0)
                    {
                        SupervisionsButton.Visibility = Visibility.Visible;
                    }
                    if (s.PreviousPositions.Count > 0)
                    {
                        poslabel.Visibility = Visibility.Visible;
                        positionList.Visibility = Visibility.Visible;
                    }
                    avglabel.Visibility = Visibility.Visible;
                    avgtext.Visibility = Visibility.Visible;

                    degreelabel.Visibility = Visibility.Hidden;
                    degreecontent.Visibility = Visibility.Hidden;

                    perflabel.Visibility = Visibility.Visible;
                    perftext.Visibility = Visibility.Visible;
                    perflabel2.Visibility = Visibility.Visible;
                    supervisorcontent.Visibility = Visibility.Hidden;
                    supervisorlabel.Visibility = Visibility.Hidden;

                }
                else
                {
                    supervisionslabel.Visibility = Visibility.Hidden;
                    supervisionscontent.Visibility = Visibility.Hidden;
                    SupervisionsButton.Visibility = Visibility.Hidden;
                    avglabel.Visibility = Visibility.Hidden;
                    avgtext.Visibility = Visibility.Hidden;
                    degreelabel.Visibility = Visibility.Visible;
                    degreecontent.Visibility = Visibility.Visible;
                    perflabel.Visibility = Visibility.Hidden;
                    perftext.Visibility = Visibility.Hidden;
                    supervisorcontent.Visibility = Visibility.Visible;
                    supervisorlabel.Visibility = Visibility.Visible;
                    poslabel.Visibility = Visibility.Hidden;
                    positionList.Visibility = Visibility.Hidden;
                    perflabel2.Visibility = Visibility.Hidden;
                }

                if (pcontroller != null)
                {
                    pcontroller.VisiblePubs.Clear();
                    foreach (Publication tmp in r.Publications)
                    {

                        Publication pub = new Publication();
                        pub.DOI = tmp.DOI;
                        pub.Title = tmp.Title;
                        pub.Year = tmp.Year;
                        pcontroller.VisiblePubs.Add(pub);
                    }
                }
                DetailPanel.DataContext = e.AddedItems[0];
                PublicationBox.DataContext = e.AddedItems[0];
                positionList.DataContext = e.AddedItems[0];
                supervisionbox.DataContext = e.AddedItems[0];
                CumulativeList.DataContext = e.AddedItems[0];
                
                //Load Photo
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = r.Photo;
                image.EndInit();
                Photo.Source = image;
            }
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            supervisionbox.Visibility = Visibility.Hidden;
            supervisionlabel.Visibility = Visibility.Hidden;
            supervisionbox.Visibility = Visibility.Hidden;
            supervisionlabel.Visibility = Visibility.Hidden;
            supervisionslabel.Visibility = Visibility.Hidden;
            supervisionscontent.Visibility = Visibility.Hidden;
            SupervisionsButton.Visibility = Visibility.Hidden;
            avglabel.Visibility = Visibility.Hidden;
            avgtext.Visibility = Visibility.Hidden;
            degreelabel.Visibility = Visibility.Visible;
            degreecontent.Visibility = Visibility.Visible;
            perflabel.Visibility = Visibility.Hidden;
            perftext.Visibility = Visibility.Hidden;
            perflabel2.Visibility = Visibility.Hidden;
            supervisorcontent.Visibility = Visibility.Visible;
            supervisorlabel.Visibility = Visibility.Visible;
            poslabel.Visibility = Visibility.Hidden;
            positionList.Visibility = Visibility.Hidden;
            pubTextBlock.Visibility = Visibility.Hidden;
            pubTextBlock.Text = "";
            controller.FilterBy((EmploymentLevel)(FilterCombo.SelectedItem));
        }

        private void NameSearchButton_Click(object sender, RoutedEventArgs e)
        {
            supervisionbox.Visibility = Visibility.Hidden;
            supervisionlabel.Visibility = Visibility.Hidden;
            supervisionslabel.Visibility = Visibility.Hidden;
            supervisionscontent.Visibility = Visibility.Hidden;
            SupervisionsButton.Visibility = Visibility.Hidden;
            avglabel.Visibility = Visibility.Hidden;
            avgtext.Visibility = Visibility.Hidden;
            degreelabel.Visibility = Visibility.Visible;
            degreecontent.Visibility = Visibility.Visible;
            perflabel.Visibility = Visibility.Hidden;
            perflabel2.Visibility = Visibility.Hidden;
            perftext.Visibility = Visibility.Hidden;
            supervisorcontent.Visibility = Visibility.Visible;
            supervisorlabel.Visibility = Visibility.Visible;
            poslabel.Visibility = Visibility.Hidden;
            positionList.Visibility = Visibility.Hidden;
            pubTextBlock.Visibility = Visibility.Hidden;
            supervisionbox.Visibility = Visibility.Hidden;
            supervisionlabel.Visibility = Visibility.Hidden;
            pubTextBlock.Text = "";
            controller.FilterByName(SearchBox.Text);
        }

        private void PublicationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //supervisionbox.Visibility = Visibility.Hidden;
            //supervisionlabel.Visibility = Visibility.Hidden;
            pubTextBlock.Visibility = Visibility.Hidden;
            Publication publication;
            if (e.AddedItems.Count > 0)
            {
                pubTextBlock.Text = "";
                publication = e.AddedItems[0] as Publication;
                pcontroller.getPubDetails(publication);
                pubTextBlock.Text = String.Format("DOI: {0}\n\n " +
                    "Title: {1} \n\n Authors: {2} \n\n Year: " +
                    "{3}\n\n CiteAs: {4}\n\n Available: {5}\n\n Type: " +
                    "{6}\n\n Age: {7}\n\n",
               publication.DOI, publication.Title, publication.Authors,
               publication.Year, publication.CiteAS, publication.Available.ToShortDateString(),
               publication.Type, publication.Age);
                pubTextBlock.Visibility = Visibility.Visible;
            }

            //private void ListBox_Loaded(object sender, RoutedEventArgs e)
            //{
            //Database.DBAdapter a = new Database.DBAdapter();
            //Database.DBAdapter.Connect();
            //showlist = a.FetchBasicResearcherlist();
            //  Lia.ItemsSource = showlist;
            //}
        }

        private void SupervisionsButton_Click(object sender, RoutedEventArgs e)
        {
            supervisionbox.Visibility = Visibility.Visible;
            supervisionlabel.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CumulativeLabel.Visibility = Visibility.Visible;
            CumulativeList.Visibility = Visibility.Visible;
        }

        //Reverse order of Publication List 
        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            Researcher res = Lia.SelectedItem as Researcher;
            if (res == null) return;
            if (pubOrder == 0)
            {
                pubOrder = 1; //ascending by Year
            }
            else
            {
                pubOrder = 0; //descending by year
            }

            DetailPanel.DataContext = pcontroller.SortPubOrder(res, pubOrder);
            if (pcontroller != null)
            {
                pcontroller.VisiblePubs.Clear();
                foreach (Publication tmp in res.Publications)
                {
                    Publication pub = new Publication();
                    pub.DOI = tmp.DOI;
                    pub.Title = tmp.Title;
                    pub.Year = tmp.Year;
                    pcontroller.VisiblePubs.Add(pub);
                }
            }
        }
    }

}
