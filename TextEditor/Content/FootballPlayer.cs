//using System;
////using System.Drawing;
//using System.Windows.Controls;
//using System.Linq;
//using System.Net.Mime;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Media.Imaging;

using System;
//using System.Drawing;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Shapes;

namespace Contenti
{
    [Serializable]
    public class FootballPlayer
    {
        int number;
        string name;
        DateTime dateOfBirth;
        string picturePath;// valjda sam dobro skontao da treba putanja slike
        string text;  // valjda sam dobro skontao da treba putanja rtf fajla
        

        public int Number { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PicturePath { get; set; }
        public string Text { get; set; }

        public FootballPlayer()
        {

        }
        
        public FootballPlayer(int number, string name, DateTime dateofBirth,string picturePath,string bio)
        {
            Number = number;
            Name = name;
            DateOfBirth = dateofBirth;
            PicturePath = picturePath;
            Text = bio;
        }
       

    }

}
