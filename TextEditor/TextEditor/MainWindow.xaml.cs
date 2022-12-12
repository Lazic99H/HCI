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
using Contenti;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
//using System.Drawing;

namespace TextEditor
{
    public partial class MainWindow : Window
    {
        public List<Color> boje2 = new List<Color>() { Colors.Red,Colors.Black};
        List<string> boje = new List<string>() { "Red", "Blue", "Black", "Green", "Yellow", "Purple", "Brown", "Aqua", "Lime", "Cyan" };
        public List<Double> fontVelicina = new List<Double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        
        String imageLocation = "";
        public int sNumber = 0;
        public int rez = 0;
        public int rtf_fajl =1  ;
        public bool provjera = false;
        

        public MainWindow()
        {

            InitializeComponent();
    
            textBoxName.Text = "Enter name";
            textBoxName.Foreground = Brushes.LightSlateGray;

            richtextdio.Text = "Enter Bio";
            richtextdio.Foreground = Brushes.LightSlateGray;

            textBoxNumber.Text = "Enter number";
            textBoxNumber.Foreground = Brushes.LightSlateGray;

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbColorFamily.ItemsSource = boje; //  typeof(Color).GetProperties();   //boje;//SystemColors.;
            cmbSizeFamily.ItemsSource = fontVelicina;
            cmbDay.ItemsSource = Contenti.Constants.dani;
            cmbMonth.ItemsSource = Contenti.Constants.mjeseci;
            cmbYear.ItemsSource = Contenti.Constants.godine;
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            
        }
        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbColorFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (cmbColorFamily.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(ForegroundProperty, cmbColorFamily.SelectedItem);
            }

        }

        private void cmbSizeFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            if (cmbSizeFamily.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbSizeFamily.SelectedItem);
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if ( (string)AddButton.Content == "Add")
            {
                Contenti.FootballPlayer newPlayer;

                if (validate())
                {
                    int day = int.Parse(cmbDay.SelectedValue.ToString());
                    int month = int.Parse(cmbMonth.SelectedValue.ToString());
                    int year = int.Parse(cmbYear.SelectedValue.ToString());

                    DateTime bDay = new DateTime(year, month, day);
                 
                    
                    string path = "rtf_file" + DateTime.Now.ToFileTime() + ".rtf";

                    TextRange textRange;
                    System.IO.FileStream fileStream;

                   
                        textRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                        using (fileStream = new FileStream(path, FileMode.OpenOrCreate))
                        {
                            textRange.Save(fileStream, System.Windows.DataFormats.Rtf);
                        }
                   

                    newPlayer = new Contenti.FootballPlayer(sNumber, textBoxName.Text, bDay, imageLocation, path); 
                    JuventusSquad.Team.Add(newPlayer);
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Podaci nisu dobro popunjeni", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if ((string)AddButton.Content =="Update")
            {
                Contenti.FootballPlayer newPlayer;

                if (validate())
                {
                    int day = int.Parse(cmbDay.SelectedValue.ToString());
                    int month = int.Parse(cmbMonth.SelectedValue.ToString());
                    int year = int.Parse(cmbYear.SelectedValue.ToString());

                    DateTime bDay = new DateTime(year, month, day);
                    if(imageLocation == "")
                    {
                        imageLocation =imageFot.Source.ToString();
                    }

                    TextRange textRange;
                    System.IO.FileStream fileStream;
                    
                        textRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                        using (fileStream = new FileStream(Constants.path_for_update, FileMode.OpenOrCreate))
                        {
                            textRange.Save(fileStream, System.Windows.DataFormats.Rtf);
                        }
                   

                    //validacija samo provjerima je li ovaj imageLocation prazan easy
                    newPlayer = new Contenti.FootballPlayer(sNumber, textBoxName.Text, bDay, imageLocation, Constants.path_for_update);
                    JuventusSquad.Team[Constants.index] = newPlayer; 
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Podaci nisu dobro popunjeni", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        //validacija 
        private bool validate()
        {
            bool result = true;

            if (textBoxName.Text.Trim().Equals("") || textBoxName.Text.Trim().Equals("Enter name"))
            {
                result = false;
                textBoxName.BorderBrush = Brushes.Red;
                textBoxName.BorderThickness = new Thickness(1);
                NameError.Content = "False entery!";
                NameError.Foreground = Brushes.Red;
            }
            else
            {
                textBoxName.BorderBrush = Brushes.Green;
                NameError.Content = string.Empty;
            }

           TextRange textRange = new TextRange(rtbEditor.Document.ContentStart,rtbEditor.Document.ContentEnd);
            
            if (!textRange.Text.Trim().Equals("") && !richtextdio.Text.Trim().Equals("Enter Bio"))
            {
                rtbEditor.BorderBrush = Brushes.Green;
                rtbError.Content = string.Empty;
            }
            else
            {

                if (richtextdio.Text.Trim().Equals("") || richtextdio.Text.Trim().Equals("Enter Bio"))
                {
                    result = false;
                    rtbEditor.BorderBrush = Brushes.Red;
                    rtbEditor.BorderThickness = new Thickness(1);
                    rtbError.Content = "False entery!";
                    rtbError.Foreground = Brushes.Red;
                }
                else
                {
                    rtbEditor.BorderBrush = Brushes.Green;
                    rtbError.Content = string.Empty;
                }
            }
            if(cmbDay.SelectedItem==null || cmbMonth.SelectedItem == null || cmbYear.SelectedItem ==null)
            {
                result = false;
                if (cmbDay.SelectedItem == null)
                {
                    cmbDay.BorderBrush = Brushes.Red;
                    cmbDay.BorderThickness = new Thickness(1);
                }
                else
                {
                    cmbDay.BorderBrush = Brushes.Green;
                }
                if (cmbMonth.SelectedItem == null)
                {
                    cmbMonth.BorderBrush = Brushes.Red;
                    cmbMonth.BorderThickness = new Thickness(1);
                }
                else
                {
                    cmbMonth.BorderBrush = Brushes.Green;

                }
                if (cmbYear.SelectedItem == null)
                {
                    cmbYear.BorderBrush = Brushes.Red;
                    cmbYear.BorderThickness = new Thickness(1);
                }
                else
                {
                    cmbYear.BorderBrush = Brushes.Green;

                }
                BirthError.Content = "False entery!";
                BirthError.Foreground = Brushes.Red;
            }
            else
            {
                cmbDay.BorderBrush = Brushes.Green;
                cmbMonth.BorderBrush = Brushes.Green;
                cmbYear.BorderBrush = Brushes.Green;
                BirthError.Content = string.Empty;
            }

            if(textBoxNumber.Text.Trim().Equals(""))
            {
                result = false;
                textBoxNumber.BorderBrush = Brushes.Red;
                textBoxNumber.BorderThickness = new Thickness(1);
                NumberError.Content = "False entery!";
                NumberError.Foreground = Brushes.Red;

            }
            else
            {
                textBoxNumber.BorderBrush = Brushes.Black;
                try
                {
                    sNumber = Int32.Parse(textBoxNumber.Text.Trim());
                    NumberError.Content = "";
                }
                catch (Exception exc)
                {
                    result = false;
                    textBoxNumber.BorderBrush = Brushes.Red;
                    textBoxNumber.BorderThickness = new Thickness(1);
                    NumberError.Content = "False entery!";
                    NumberError.Foreground = Brushes.Red;
                    result = false;
                }
                

            }

            if((string)AddButton.Content == "Add")
            {
                if(imageLocation == "")
                {
                    result = false;
                    ImgError.Content = "No photo added!";
                    ImgError.Foreground = Brushes.Red;
                }
                else
                {
                    ImgError.Content = "";
                }
                    

            }

            return result;
        }

        //za ime
        private void textBoxName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxName.Text.Trim().Equals("Enter name"))
            {
                textBoxName.Text = "";
                textBoxName.Foreground = Brushes.Black;
            }
        }

        private void textBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxName.Text.Trim().Equals(""))
            {
                textBoxName.Text = "Enter name";
                textBoxName.Foreground = Brushes.LightSlateGray;
            }
        }
        //za RichTextBox
        private void rtbEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (richtextdio.Text.Trim().Equals("Enter Bio"))
            {
                richtextdio.Text = "";
                richtextdio.Foreground = Brushes.Black;
            }
        }

        private void rtbEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxName.Text.Trim().Equals(""))
            {
                textBoxName.Text = "Enter Bio";
                textBoxName.Foreground = Brushes.LightSlateGray;
            }
        }

        //za Number
        private void textBoxNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxNumber.Text.Trim().Equals("Enter number"))
            {
                textBoxNumber.Text = "";
                textBoxNumber.Foreground = Brushes.Black;
            }
        }

        private void textBoxNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxNumber.Text.Trim().Equals(""))
            {
                textBoxNumber.Text = "Enter number";
                textBoxNumber.Foreground = Brushes.LightSlateGray;
            }
        }

        //browser slika i odma cuvam lokaciju u imageLocation
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";
                
                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    imageFot.Source = new BitmapImage(new Uri(imageLocation));
              
                }
                    
            }
            catch(Exception)
            {
                System.Windows.Forms.MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //ovo mi je brojac za richbox text
        private void rtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                TextRange textRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);

                //string s = richtextdio.Text; uh 
                string s = textRange.Text;
                s = s.TrimEnd();
                if (!String.IsNullOrEmpty(s))
                {
                    int count = 0;
                    bool lastWasWordChar = false;
                    foreach (char c in s)
                    {
                        if (Char.IsLetterOrDigit(c) || c == '_' || c == '\'' || c == '-')
                        {
                            lastWasWordChar = true;
                            continue;
                        }
                        if (lastWasWordChar)
                        {
                            lastWasWordChar = false;
                            count++;
                        }
                    }
                    if (!lastWasWordChar) count--;
                    rez = count + 1;
                }

                Counting.Content = rez.ToString();

            }

            catch (Exception ex)
            {
              //  System.Windows.Forms.MessageBox.Show(ex.Message.ToString());
            }
        }

        private void cmbDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
   
}
