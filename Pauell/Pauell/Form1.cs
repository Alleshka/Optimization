﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using ParserAG;
using MartixVectorAG;
using System.IO;

namespace Pauell
{
    public partial class Form1 : Form
    {
        private PauellA1 P1 = null;
        private PauellA2 P2 = null;
        private PauellA3 P3 = null;
        private Rossen Ros = null;

        private Huck huk = null;


        private List<Vector> PosP1 = null;
        private List<Vector> PosP2 = null;
        private List<Vector> PosP3 = null;
        private List<Vector> PosRossen = null;
        private List<Vector> PosHuck = null;

        private Thread _ThreadPaul;
        private Thread _ThreadRoss;
        private Thread _ThreadHD;

        private string _func;
        private Vector X0;

        private int livetime;

        private List<string> answers;

        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            comboBox1.Enabled = true;

            label4.Text = Convert.ToString(trackBar1.Value);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = true;
                comboBox1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Minimum = 0;
                livetime = Convert.ToInt32(textBox4.Text);
                progressBar1.Maximum = livetime;

                Start();
                pictureBox1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Start()
        {
            answers = new List<string>();

            PosP1 = null;
            PosP2 = null;
            PosP3 = null;
            PosRossen = null;
            PosHuck = null;

            pictureBox1.Refresh();

            textBox3.Text = "";

            if (radioButton1.Checked == false) _func = textBox1.Text;
            else _func = comboBox1.Text;

            string tempX = textBox2.Text;
            string[] temp = tempX.Split(';');

            List<double> variable = new List<double>();

            Parser tempParse = new Parser();

            int count = Convert.ToInt32(tempParse.CheckParse(_func));
            label3.Text = Convert.ToString(count);

            for (int i = 0; i < Convert.ToDouble(tempParse.CheckParse(_func)); i++)
            {
                variable.Add(Convert.ToDouble(temp[i]));
            }

            X0 = new Vector(variable.ToArray());

            Thread starttime = new Thread(StartTime);
            starttime.Start();

            _ThreadPaul = new Thread(PaulsStart);
            _ThreadRoss = new Thread(RossenbrokStart);
            _ThreadHD = new Thread(HDStart);
           

            _ThreadRoss.Start();
            _ThreadHD.Start();
            _ThreadPaul.Start();
           
            starttime.Join();

            PrintText();
        }

        private void PaulsStart()
        {
            P1 = new PauellA1(_func, X0, Math.Pow(10, -8));
            P1.Start(); answers.Add(P1.PrintAnswer());
            PosP1 = P1.Position;


            P2 = new PauellA2(_func, X0, Math.Pow(10, -8));
            P2.Start(); answers.Add(P2.PrintAnswer());
            PosP2 = P2.Position;


            P3 = new PauellA3(_func, X0, Math.Pow(10, -8));
            P3.Start(); answers.Add(P3.PrintAnswer());
            PosP3 = P3.Position;

            //pictureBox1.Refresh();
            PrintText();
        }

        private void RossenbrokStart()
        {
            Ros = new Rossen(_func, X0);
            Ros.Start(); answers.Add(Ros.PrintAnswer());            
            PosRossen = Ros.Position;

            PrintText();
        }

        private void HDStart()
        {
            huk = new Huck(_func, X0, Math.Pow(10, -8));
            huk.Start();
            answers.Add(huk.PrintAnswer());
            PosHuck = huk.Position;

            PrintText();
        }

        private void StartTime()
        {
            for (int i = 0; i <= livetime; i++)
            {

                if (answers.Count >= 5)
                {
                    progressBar1.Value = livetime;
                    answers.Add("Все потоки завершили свою работу");
                    return;
                }
                Thread.Sleep(1000);
                progressBar1.Value = i;
            }

            answers.Add("Время кончилось");

            _ThreadHD.Abort();
            _ThreadPaul.Abort();
            _ThreadRoss.Abort();
        }

        private void PrintText()
        {
            textBox3.Text = "";

            for (int i = 0; i < answers.Count; i++)
            {
                textBox3.Text += answers[i];
            }

            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            float mash = trackBar1.Value;

            // Рисуем Оси
            Graphics g = e.Graphics;

            g.DrawLine(System.Drawing.Pens.Black, 0, pictureBox1.Height / 2, pictureBox1.Width, pictureBox1.Height / 2);
            g.DrawLine(System.Drawing.Pens.Black, pictureBox1.Width / 2, 0, pictureBox1.Width / 2, pictureBox1.Height);


            float w, h;

            // Центр
            w = pictureBox1.Width / 2;
            h = pictureBox1.Height / 2;

            if (checkBox4.Checked == true)
            {
                g.DrawLine(System.Drawing.Pens.Purple, w, h, w + mash, h - mash);
                g.DrawLine(System.Drawing.Pens.Purple, w, h, w + mash, h);
                g.DrawLine(System.Drawing.Pens.Purple, w, h, w, h - mash);
            }

            // Рисуем позиции
            if ((PosP1 != null) && (checkBox1.Checked))
            {
                checkBox1.ForeColor = Color.Red;
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosP1[0].ch[0]), Convert.ToSingle(h - mash * PosP1[0].ch[1]), 3, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosP1.Last().ch[0]), Convert.ToSingle(h - mash * PosP1.Last().ch[1]), 3, 3);
                }
                for (int i = 0; i < PosP1.Count - 1; i++)
                {
                    //textBox3.Text += ("V1: " + PosP1[i].printVector()) + "V2:  " + PosP1[i+1].printVector();
                    PrintVector(g, System.Drawing.Pens.Red, PosP1[i], PosP1[i + 1]);
                }
            }
            else checkBox1.ForeColor = Color.Black;

            if ((PosP2 != null) && (checkBox2.Checked))
            {
                checkBox2.ForeColor = Color.Green;
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosP2[0].ch[0]), Convert.ToSingle(h - mash * PosP2[0].ch[1]), 3, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosP2.Last().ch[0]), Convert.ToSingle(h - mash * PosP2.Last().ch[1]), 3, 3);
                }
                for (int i = 0; i < PosP2.Count - 1; i++)
                {
                    //textBox3.Text += ("V1: " + PosP1[i].printVector()) + "V2:  " + PosP1[i+1].printVector();
                    PrintVector(g, System.Drawing.Pens.Green, PosP2[i], PosP2[i + 1]);
                }
            }
            else checkBox2.ForeColor = Color.Black;

            if ((PosP3 != null) && (checkBox3.Checked))
            {
                checkBox3.ForeColor = Color.Blue;
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosP3[0].ch[0]), Convert.ToSingle(h - mash * PosP3[0].ch[1]), 5, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosP3.Last().ch[0]), Convert.ToSingle(h - mash * PosP3.Last().ch[1]), 6, 3);
                }
                for (int i = 0; i < PosP3.Count - 1; i++)
                {
                    PrintVector(g, System.Drawing.Pens.Blue, PosP3[i], PosP3[i + 1]);
                }
            }
            else checkBox3.ForeColor = Color.Black;

            if ((PosRossen != null) && (checkBox5.Checked))
            {
                checkBox5.ForeColor = Color.DarkOrange;
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosRossen[0].ch[0]), Convert.ToSingle(h - mash * PosRossen[0].ch[1]), 3, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosRossen.Last().ch[0]), Convert.ToSingle(h - mash * PosRossen.Last().ch[1]), 3, 3);
                }
                for (int i = 0; i < PosRossen.Count - 1; i++)
                {
                    PrintVector(g, System.Drawing.Pens.DarkOrange, PosRossen[i], PosRossen[i + 1]);
                }
            }
            else checkBox5.ForeColor = Color.Black;

            if ((PosHuck != null) && (checkBox8.Checked))
            {
                checkBox8.ForeColor = Color.DarkKhaki;
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosHuck[0].ch[0]), Convert.ToSingle(h - mash * PosHuck[0].ch[1]), 3, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + mash * PosHuck.Last().ch[0]), Convert.ToSingle(h - mash * PosHuck.Last().ch[1]), 3, 3);
                }
                for (int i = 0; i < PosHuck.Count - 1; i++)
                {
                    PrintVector(g, System.Drawing.Pens.DarkKhaki, PosHuck[i], PosHuck[i + 1]);
                }
            }
            else checkBox8.ForeColor = Color.Black;
        }
        private void PrintVector(Graphics e, System.Drawing.Pen p, Vector v1, Vector v2)
        {

            float mash = trackBar1.Value;

            double w, h;

            // Центр
            w = pictureBox1.Width / 2;
            h = pictureBox1.Height / 2;

            float a1, b1, a2, b2;

            a1 = Convert.ToSingle(w + v1.ch[0] * mash);
            b1 = Convert.ToSingle(h - v1.ch[1] * mash);
            a2 = Convert.ToSingle(w + v2.ch[0] * mash);
            b2 = Convert.ToSingle(h - v2.ch[1] * mash);

            e.DrawLine(p, a1, b1, a2, b2);
            if(checkBox7.Checked==true) e.FillRectangle(Brushes.Black, a2, b2, 3, 3);
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            string _func;
            if (radioButton1.Checked == false) _func = textBox1.Text;
            else _func = comboBox1.Text;

            Parser tempParse = new Parser();

            int count = Convert.ToInt32(tempParse.CheckParse(_func));
        }
        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label4.Text = Convert.ToString(trackBar1.Value);
            pictureBox1.Refresh();
        }
        private void сделатьОтчётToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter temp = new StreamWriter(File.Open("tmptext", FileMode.Append));

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                comboBox1.SelectedIndex = i;

                Start();

                temp.WriteLine("Func = " + comboBox1.Text);
                temp.WriteLine(textBox3.Text);

                temp.WriteLine("________________________________________" + Environment.NewLine + Environment.NewLine);
            }

            temp.Close();
        }
        private void контрольныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader temp = new StreamReader(File.OpenRead("D:\\Project\\Sem10\\Optimization\\Pauell\\Pauell\\FuncEasy.txt"));

            comboBox1.Items.Clear();
            while (!temp.EndOfStream)
            {
                comboBox1.Items.Add(temp.ReadLine());
            }
        }
        private void практическиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader temp = new StreamReader(File.OpenRead("D:\\Project\\Sem10\\Optimization\\Pauell\\Pauell\\FuncHard.txt"));

            comboBox1.Items.Clear();
            while (!temp.EndOfStream)
            {
                comboBox1.Items.Add(temp.ReadLine());
            }
        }

        
    }
}
