using Contenti;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace TextEditor
{
    
    /// <summary>
    /// Interaction logic for JuventusSquad.xaml
    /// </summary>
    public partial class JuventusSquad : Window
    {
        private DataIO serializer = new DataIO();

        public static BindingList<Contenti.FootballPlayer> Team
        {
            get; set;
            
        }

       

        public JuventusSquad()
        {
            Team = serializer.DeSerializeObject<BindingList<FootballPlayer>>("fudbaleri.xml");
            if (Team == null)
            {
                Team = new BindingList<FootballPlayer>();
            }

            InitializeComponent();
            DataContext = this;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            serializer.SerializeObject<BindingList<FootballPlayer>>(Team, "fudbaleri.xml");
        }

        private void AddPlayer(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.ShowDialog();
        }

        private void nameImg_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            
        }

        private void ButtonDelate_Click(object sender, RoutedEventArgs e)
        {
            DialogResult ds;
            ds = System.Windows.Forms.MessageBox.Show("Are you sure you want to remove this player?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (ds == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    FootballPlayer fd =(FootballPlayer) (((System.Windows.Controls.Button)e.Source).DataContext);

                    File.Delete(fd.Text);

                    Team.Remove(fd);

                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ButtonRead_Click(object sender, RoutedEventArgs e)
        {
            ReadWindow newReadWindow = new ReadWindow();
            FootballPlayer fd = (FootballPlayer)(((System.Windows.Controls.Button)e.Source).DataContext);
            newReadWindow.PlayerNameWillBeHere.Content = fd.Name;

            TextRange textRange;
            System.IO.FileStream fileStream;

            if (System.IO.File.Exists(fd.Text))
            {
               
                textRange = new TextRange(newReadWindow.RichBio.Document.ContentStart, newReadWindow.RichBio.Document.ContentEnd);
                using (fileStream = new FileStream(fd.Text, FileMode.OpenOrCreate))
                {
                    textRange.Load(fileStream, System.Windows.DataFormats.Rtf);
                }
            }

             
            newReadWindow.image.Source = new BitmapImage(new Uri(fd.PicturePath));
            newReadWindow.ShowDialog();
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newChangeWindow = new MainWindow();

            FootballPlayer fd = (FootballPlayer)(((System.Windows.Controls.Button)e.Source).DataContext); //ovo je taj otvoreni,ovo sam dugo trazio xd
            Constants.index = Team.IndexOf(fd);
            Constants.path_for_update = fd.Text;

            TextRange textRange;
            System.IO.FileStream fileStream;

            if(System.IO.File.Exists(fd.Text))
            {
                //TextRange = new TextRange()rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd
                try
                {
                    textRange = new TextRange(newChangeWindow.rtbEditor.Document.ContentStart, newChangeWindow.rtbEditor.Document.ContentEnd);
                  //  textRange = new TextRange(newChangeWindow.richtextdio.ContentStart, newChangeWindow.richtextdio.ContentEnd);
                    using (fileStream = new FileStream(fd.Text, FileMode.OpenOrCreate))
                    {
                        textRange.Load(fileStream, System.Windows.DataFormats.Rtf);
                        
                    }
                }
                catch
                {

                }
            }

           

            newChangeWindow.AddButton.Content = "Update";
            newChangeWindow.textBoxName.Text = fd.Name;
            newChangeWindow.textBoxName.Foreground = Brushes.Black;
            //       newChangeWindow.richtextdio.Text = fd.Text;
            newChangeWindow.textBoxNumber.Text = fd.Number.ToString();
            newChangeWindow.textBoxNumber.Foreground = Brushes.Black;
            newChangeWindow.imageFot.Source = new BitmapImage(new Uri(fd.PicturePath));
            newChangeWindow.cmbDay.SelectedValue = fd.DateOfBirth.Day;
            newChangeWindow.cmbMonth.SelectedValue = fd.DateOfBirth.Month;
            newChangeWindow.cmbYear.SelectedValue = fd.DateOfBirth.Year;
            newChangeWindow.ShowDialog();

        }

        private void Window_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
