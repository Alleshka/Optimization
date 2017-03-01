﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParserAG;
using MartixVectorAG;

namespace Pauell
{
    public partial class Form1 : Form
    {
        private PauellA1 P1 = null;
        private PauellA2 P2 = null;
        private PauellA3 P3 = null;
        private Rossen Ros = null;


        private List<Vector> PosP1 = null;
        private List<Vector> PosP2 = null;
        private List<Vector> PosP3 = null;
        private List<Vector> PosRossen = null;

        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            comboBox1.Enabled = true;
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
            Start();
            pictureBox1.Refresh();
        }

        private void Start()
        {
            PosP1 = null;
            PosP2 = null;
            PosP3 = null;
            PosRossen = null;

            textBox3.Text = "";

            string _func;
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

            Vector X0 = new Vector(variable.ToArray());

            if (checkBox1.Checked == true)
            {
                P1 = new PauellA1(_func, X0, Math.Pow(10, -8));
                textBox3.Text += "П1" + Environment.NewLine + "Минимум в точке: " + P1.Start().printVector() + Environment.NewLine;
                PosP1 = P1.Position;
            }
            if (checkBox2.Checked == true)
            {
                P2 = new PauellA2(_func, X0, Math.Pow(10, -8));
                textBox3.Text += "П2" + Environment.NewLine + "Минимум в точке: " + P2.Start().printVector() + Environment.NewLine;
                PosP2 = P2.Position;
            }
            if (checkBox3.Checked == true)
            {
                P3 = new PauellA3(_func, X0, Math.Pow(10, -8));
                textBox3.Text += "П3" + Environment.NewLine + "Минимум в точке: " + P3.Start().printVector() + Environment.NewLine;
                PosP3 = P3.Position;
            }
            if (checkBox5.Checked == true)
            {
                Ros = new Rossen(_func, X0);
                textBox3.Text += "Розенброк " + Environment.NewLine + "Минимум в точке: " + Ros.Start().printVector() + Environment.NewLine;
                PosRossen = Ros.Position;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {


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
                g.DrawLine(System.Drawing.Pens.Purple, w, h, w + 50, h - 50);
                g.DrawLine(System.Drawing.Pens.Purple, w, h, w + 50, h);
                g.DrawLine(System.Drawing.Pens.Purple, w, h, w, h - 50);
            }

            // Рисуем позиции
            if ((PosP1 != null)&&(checkBox1.Checked))
            {
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosP1[0].ch[0]), Convert.ToSingle(h - 50 * PosP1[0].ch[1]), 3, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosP1.Last().ch[0]), Convert.ToSingle(h - 50 * PosP1.Last().ch[1]), 3, 3);
                }
                for (int i = 0; i < PosP1.Count - 1; i++)
                {
                    //textBox3.Text += ("V1: " + PosP1[i].printVector()) + "V2:  " + PosP1[i+1].printVector();
                    PrintVector(g, System.Drawing.Pens.Red, PosP1[i], PosP1[i + 1]);
                }
            }

            if ((PosP2 != null)&&(checkBox2.Checked))
            {
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosP2[0].ch[0]), Convert.ToSingle(h - 50 * PosP2[0].ch[1]), 3, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosP2.Last().ch[0]), Convert.ToSingle(h - 50 * PosP2.Last().ch[1]), 3, 3);
                }
                for (int i = 0; i < PosP2.Count - 1; i++)
                {
                    //textBox3.Text += ("V1: " + PosP1[i].printVector()) + "V2:  " + PosP1[i+1].printVector();
                    PrintVector(g, System.Drawing.Pens.Green, PosP2[i], PosP2[i + 1]);
                }
            }

            if ((PosP3 != null)&&(checkBox3.Checked))
            {
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosP3[0].ch[0]), Convert.ToSingle(h - 50 * PosP3[0].ch[1]), 5, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosP3.Last().ch[0]), Convert.ToSingle(h - 50 * PosP3.Last().ch[1]), 6, 3);
                }
                for (int i = 0; i < PosP3.Count - 1; i++)
                {
                    PrintVector(g, System.Drawing.Pens.Blue, PosP3[i], PosP3[i + 1]);
                }
            }
            if ((PosRossen != null)&&(checkBox5.Checked))
            {
                if (checkBox6.Checked == true)
                {
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosRossen[0].ch[0]), Convert.ToSingle(h - 50 * PosRossen[0].ch[1]), 3, 3);
                    g.FillRectangle(Brushes.Black, Convert.ToSingle(w + 50 * PosRossen.Last().ch[0]), Convert.ToSingle(h - 50 * PosRossen.Last().ch[1]), 3, 3);
                }
                for (int i = 0; i < PosRossen.Count - 1; i++)
                {
                    PrintVector(g, System.Drawing.Pens.DarkOrange, PosRossen[i], PosRossen[i + 1]);
                }
            }
        }

        private void PrintVector(Graphics e, System.Drawing.Pen p, Vector v1, Vector v2)
        {
            double w, h;

            // Центр
            w = pictureBox1.Width / 2;
            h = pictureBox1.Height / 2;

            float a1, b1, a2, b2;

            a1 = Convert.ToSingle(w + v1.ch[0] * 50);
            b1 = Convert.ToSingle(h - v1.ch[1] * 50);
            a2 = Convert.ToSingle(w + v2.ch[0] * 50);
            b2 = Convert.ToSingle(h - v2.ch[1] * 50);

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
    }
}