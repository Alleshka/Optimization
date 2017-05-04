using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using System.Threading;
using MartixVectorAG;
using ParserAG;
using System.Windows.Input;

namespace GenAlgV1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        GenAlgWorck temp = null;
        AnswerClass answer = null;

        string Functon = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start()
        {
            NumIterText.Text = "0";

            Functon = "";
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

            temp = new GenAlgWorck();

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
            else argsgen.Add(argsgen[0] * 2);
            // Добавляем количество итераций для уплотнения
            if (CompactCheck.IsChecked == true)
            {
                argsgen.Add(Convert.ToDouble(CompactIterText.Text));
            }
            else
            {
                argsgen.Add(argsgen[0] * 2);
            }
            argsgen.Add(Convert.ToDouble(CompactCountText.Text)); // Количество разрядов

            if (AltExit.IsChecked == true) argsgen.Add(1);
            else argsgen.Add(0);

            temp.SetAlgPar(argsgen);

            answer = temp.StartWorck();

            NumIterBox.Items.Clear();
            for (int i = 0; i < answer.log.Count; i++)
            {
                NumIterBox.Items.Add(i);
            }
            NumIterBox.SelectedIndex = 0;


            RefreshAnswer();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow1.Cursor = Cursors.Wait;
                Start();
                PrintGraph();
                MainWindow1.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MainWindow1.Cursor = Cursors.Arrow;
                MessageBox.Show("Ошибка ввода данных");
            }
        }

        private void RefreshAnswer()
        {
            if (answer != null)
            {
                PointMinText.Text = "(";
                foreach (double t in answer.minPoint)
                {
                    PointMinText.Text += t + "; ";
                }
                PointMinText.Text += ")";

                FuncMinText.Text = Convert.ToString(answer.MinF);

                if ((NumIterText.Text != ""))
                {
                    int index = Convert.ToInt32(NumIterText.Text);
                    answer.log[index].PrintLog((bool)OrderCheck.IsChecked, Convert.ToInt32(AroundText.Text));

                    ArgLogText.Text = "Average: " + answer.log[index].Average + Environment.NewLine;
                    ArgLogText.Text += "Min: " + answer.log[index].MinF + Environment.NewLine;
                    ArgLogText.Text += Environment.NewLine;

                    ArgLogText.Text += answer.log[index].RealLog;

                    CodeLogText.Text = "Average: " + answer.log[index].Average + Environment.NewLine;
                    CodeLogText.Text += "Min: " + answer.log[index].MinF + Environment.NewLine;
                    CodeLogText.Text += Environment.NewLine;
                    CodeLogText.Text += answer.log[index].CodeLog;
                }
            }
        }

        private void OrderCheck_Click(object sender, RoutedEventArgs e)
        {
            RefreshAnswer();
        }

        private void AroundText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AroundText.Text != "") RefreshAnswer();
        }

        private void PrintGraph()
        {
            if (answer != null)
            {
                List<double> Min = new List<double>();
                List<double> Average = new List<double>();

                DataContext = null;

                foreach (LogClass k in answer.log)
                {
                    Min.Add(k.MinF);
                    Average.Add(k.Average);
                }

                SeriesCollection = new SeriesCollection();

                if (MinGraphCheck.IsChecked == true)
                {
                    SeriesCollection.Add(new LineSeries
                    {
                        Title = "Min",
                        Values = new ChartValues<double>(Min),
                        PointForeground = Brushes.Gray
                    });
                }

                if (AverageGraphCheck.IsChecked == true)
                {
                    SeriesCollection.Add(new LineSeries
                    {
                        Title = "Average",
                        Values = new ChartValues<double>(Average),
                        PointForeground = Brushes.Orange
                    });
                }

                YFormatter = value => value.ToString();

                DataContext = this;
            }
        }

        private void NumIterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (NumIterText.Text != "") RefreshAnswer();
            }
            catch (Exception ex)
            {
                NumIterText.Text = "0";
            }
        }

        private void NumIterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NumIterText.Text = Convert.ToString(NumIterBox.SelectedIndex);
        }

        private void MinGraphCheck_Click(object sender, RoutedEventArgs e)
        {
            PrintGraph();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MartixVectorAG.Vector answ = null;
                double[] temp = answer.minPoint.ToArray();
                Parser parse = new Parser();

                if (HDRadio.IsChecked == true)
                {
                    Huck hd = new Huck(Functon, new MartixVectorAG.Vector(temp), Math.Pow(10, -5));
                    answ = hd.Start();
                }

                if (RosenRadio.IsChecked == true)
                {
                    Rossen ros = new Rossen(Functon, new MartixVectorAG.Vector(temp));
                    answ = ros.Start();
                }

                if (Pauel1Radio.IsChecked == true)
                {
                    PauellA1 p = new PauellA1(Functon, new MartixVectorAG.Vector(temp), Math.Pow(10, -5));
                    answ = p.Start();
                }

                if (Pauel2Radio.IsChecked == true)
                {
                    PauellA2 p = new PauellA2(Functon, new MartixVectorAG.Vector(temp), Math.Pow(10, -5));
                    answ = p.Start();
                }

                if (Pauel3Radio.IsChecked == true)
                {
                    PauellA3 p = new PauellA3(Functon, new MartixVectorAG.Vector(temp), Math.Pow(10, -5));
                    answ = p.Start();
                }

                MinMetodPoint.Text = answ.printVector();
                MinFMetod.Text = parse.Parse(this.Functon, answ.ch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Необходимо выбрать метод");
            }
        }

        private void PointMinText_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(PointMinText.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (answer != null)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename, false))
                    {
                        file.Write("Минимум в точке: (");
                        foreach (double m in answer.minPoint)
                        {
                            file.Write(m + "; ");
                        }
                        file.WriteLine(");");

                        file.WriteLine("Значение в минимуме: " + answer.MinF); file.WriteLine();

                        for (int i = 0; i < answer.log.Count; i++)
                        {
                            file.WriteLine("Итерация " + i);
                            file.WriteLine("Минимум: " + answer.log[i].MinF);
                            file.WriteLine("Среднее: " + answer.log[i].Average);
                            file.WriteLine();

                            file.WriteLine("Популяция: ");
                            file.WriteLine(answer.log[i].RealLog);

                            file.WriteLine("Закодированные значения: ");
                            file.WriteLine(answer.log[i].CodeLog);

                            file.WriteLine(); file.WriteLine("==========================================================");
                        }
                    }
                }
            }
        }
    }
}
