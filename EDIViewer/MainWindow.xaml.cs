using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EDIViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileName = Resource1.Remit.Split("~".ToCharArray());

            int i = 0;

            var aasdf = from s in fileName
                        let ss = s.Trim().Trim("~".ToCharArray())
                        select new EdiFileLine(i++, "*".ToCharArray(), ss);

            FileInfo infod = FileInfo.Load(aasdf);
            this.DataContext = infod;

        }
    }
}
