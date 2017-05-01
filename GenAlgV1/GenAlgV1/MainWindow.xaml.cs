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

namespace GenAlgV1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<double> RealNum = new List<double>();
        private List<string> CodeNum = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartCode_Click(object sender, RoutedEventArgs e)
        {
            Person temp = new Person();
            List<double> doubles = new List<double>();

            for (int i = 0; i < ListReal.Items.Count; i++)
            {
                doubles.Add(Convert.ToDouble(ListReal.Items[i]));
            }

            RealNum = doubles;

            if (TypeCodeCheck.IsChecked == false) temp.SetNumber(RealNum);
            else
            {
                List<double> args = new List<double>();
                args.Add(Convert.ToDouble(Min.Text));
                args.Add(Convert.ToDouble(Max.Text));
                args.Add(Convert.ToDouble(Count.Text));

                temp.SetNumber(args, RealNum);
            }

            CodeNum = temp._codeNumbers;

            RefreshList();
        }

        private void RefreshList()
        {
            ListReal.Items.Clear();
            foreach (double t in RealNum)
            {
                ListReal.Items.Add(t);
            }


            ListCode.Items.Clear();
            foreach (string s in CodeNum)
            {
                ListCode.Items.Add(s);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            RealNum.Add(Convert.ToDouble(Number.Text));
            Number.Text = "";
            RefreshList();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            RealNum = new List<double>();
            CodeNum = new List<string>();

            RefreshList();
        }

        private void DeCode_Click(object sender, RoutedEventArgs e)
        {
            Person temp = new Person();
            List<string> doubles = new List<string>();

            for (int i = 0; i < ListCode.Items.Count; i++)
            {
                doubles.Add(Convert.ToString(ListCode.Items[i]));
            }

            CodeNum = doubles;

            if (TypeCodeCheck.IsChecked == false) temp.SetCodeNumber(CodeNum);
            else
            {
                List<double> args = new List<double>();
                args.Add(Convert.ToDouble(Min.Text));
                args.Add(Convert.ToDouble(Max.Text));
                args.Add(Convert.ToDouble(Count.Text));

                temp.SetCodeNumber(args, CodeNum);
            }

            RealNum = temp._numbers;

            RefreshList();
        }
    }
}
