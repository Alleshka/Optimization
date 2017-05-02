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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Functon = "";
            // Если выбран ручной ввод функции
            if (ManualFuncRadio.IsChecked == true)
            {
                Functon = ManualFuncText.Text;
            }
            if (SaveFuncRadio.IsChecked == true)
            {
                Functon = SaveFuncCombo.Text;
            }

            List<double> args = new List<double>(); // Аргументы границы/кодирования
            args.Add(Convert.ToDouble(MinText.Text)); args.Add(Convert.ToDouble(MaxText.Text)); // Имеем границы

            // Если целочисленное кодирование
            if (IntCodeCheck.IsChecked == true)
            {
                args.Add(Convert.ToDouble(CountBitText.Text));
            }

            GenAlgWorck temp = new GenAlgWorck();

            // Если целочисленное кодирование
            if (args.Count == 3)
            {
                temp.SetGr(args);
            }
            else // Если вещественное
            {
                temp.SetGr(args[0], args[1]);
            }

            temp.SetFunc(Functon);

            List<double> argsgen = new List<double>(); // Параметры ГА
            argsgen.Add(Convert.ToDouble(CountPersonText.Text)); // Размер популяции
            argsgen.Add(Convert.ToDouble(CountIterText.Text)); // Количество итераци
            argsgen.Add(Convert.ToDouble(TournirText.Text)); // Турнир
            argsgen.Add(Convert.ToDouble(ChanceCrossText.Text)); // Шанс скрещивания
            argsgen.Add(Convert.ToDouble(ChanceMutationText.Text)); // Шанс мутации
            argsgen.Add(Convert.ToDouble(ChanceInversText.Text)); // Шанс инверсии
            argsgen.Add(Math.Pow(10, Convert.ToDouble(SpikeEps.Text))); // Разность
            // Итераций для вымирания
            if (SpikeCheckBox.IsChecked == true)
            {
                argsgen.Add(Convert.ToDouble(CpikeCount.Text));
            }
            else argsgen.Add(argsgen[0]*2);



            temp.SetAlgPar(argsgen);
            AnswerClass answer = temp.StartWorck();

            PointMinText.Text = "";
            foreach (double k in answer.minPoint)
            {
                PointMinText.Text += k + ";";
            }
            FuncMinText.Text = "F = " + answer.MinF;

            ArgLog.Text = answer.log[Convert.ToInt32(NumIter.Text)].RealLog;
            CodeLog.Text = answer.log[Convert.ToInt32(NumIter.Text)].CodeLog;
        }
    }
}
