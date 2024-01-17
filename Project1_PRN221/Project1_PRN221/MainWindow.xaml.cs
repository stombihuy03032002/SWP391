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

namespace Project1_PRN221
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txt1.TextChanged += TextBox_TextChanged;
            txt2.TextChanged += TextBox_TextChanged;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(txt1.Text, out double weight) && double.TryParse(txt2.Text, out double height))
            {


                double bmi = weight / ((height / 100) * (height / 100));

                txt3.Text = bmi.ToString("F2");

                if (bmi < 18.5)
                {
                    txt4.Text = "Underweight";
                }
                else if (bmi >= 18.5 && bmi < 25)
                {
                    txt4.Text = "Normal";
                    
                }
                else if (bmi >= 25 && bmi < 30)
                {
                    txt4.Text = "Overweight";
                }
                else
                {
                    txt4.Text = "Obese";
                }
            }
            else
            {
                txt3.Text = "";
                txt4.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            txt4.Text = "";
        }

        private void txt3_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
    }
}
