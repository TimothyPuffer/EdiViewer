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
                        select new EdiFileLine(i++, "*", ss);

            FileInfo infod = FileInfo.Load(aasdf);
            this.DataContext = infod;

        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel pan = sender as StackPanel;
            if (pan == null)
                return;
            
            PropertyProviderInfo line = pan.DataContext as PropertyProviderInfo;
            if (line == null)
                return;

            RawLineCollection rlc = icRawText.ItemsSource as RawLineCollection;
            if (rlc == null)
                return;
            rlc.SetSelected(line.ValueMonad.Line, line.Position);

        }


        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            RawLineCollection rlc = icRawText.ItemsSource as RawLineCollection;
            if (rlc == null)
                return;
            rlc.ClearSelected();
        }

        private void lbClaim_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = sender as ListBox;
            if (box == null)
                return;

            ClaimPaymentInformation cpi = box.SelectedItem as ClaimPaymentInformation;
            if (cpi == null)
                return;

            icRawText.ItemsSource = new RawLineCollection(cpi.Lines);


        }

    }

    public class HighLightConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FileLineHighLight temp;
            if (!Enum.TryParse<FileLineHighLight>(value.ToString(), out temp))
            {
                temp = FileLineHighLight.None;
            }

            switch (temp)
            {
                case FileLineHighLight.Yellow:
                    return new SolidColorBrush(Colors.Yellow);
                default:
                    return new SolidColorBrush(Colors.White);

            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
