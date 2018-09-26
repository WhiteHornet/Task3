﻿//Rah5i  Mitchell Dreyer 18000499

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GADE_POE
{
    public partial class Form1 : Form
    {
        Map map = new Map(20, 20, 20);
        const int START_X = 20;
        const int START_Y = 20;
        const int SPACING = 10;
        const int SIZE = 20;
        Random R = new Random();
        int turn = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DisplayMap()
        {
            groupBox1.Controls.Clear();
            foreach (Unit u in map.Units)
            {
                if (u.GetType() == typeof(MeleeUnit))
                {
                    int start_x, start_y;
                    start_x = groupBox1.Location.X;
                    start_y = groupBox1.Location.Y;
                    MeleeUnit m = (MeleeUnit)u;
                    Button b = new Button();
                    b.Size = new Size(SIZE, SIZE);
                    b.Location = new Point(start_x + (m.XPos * SIZE), start_y + (m.YPos * SIZE));
                    b.Text = m.Symbol;
                    if (m.Faction == 1)
                    {
                        b.ForeColor = Color.Blue;
                    }
                    else
                    {
                        b.ForeColor = Color.Orange;
                    }

                    if(m.IsDead())
                    {
                        b.ForeColor = Color.Black;
                    }
                    b.Click += new EventHandler(buttn_click);
                    groupBox1.Controls.Add(b);
                }
                foreach (Unit Y in map.Units)
                {if (Y.GetType() == typeof(RangedUnit))
                    {
                        int start_x, start_y;
                        start_x = groupBox1.Location.X;
                        start_y = groupBox1.Location.Y;
                        RangedUnit r = (RangedUnit)Y;
                        Button b = new Button();
                        b.Size = new Size(SIZE, SIZE);
                        b.Location = new Point(start_x + (r.XPos * SIZE), start_y + (r.YPos * SIZE));
                        b.Text = r.Symbol;
                        if (r.Faction == 1)
                        {
                            b.ForeColor = Color.Blue;
                        }
                        else
                        {
                            b.ForeColor = Color.Orange;
                        }

                        if (r.IsDead())
                        {
                            b.ForeColor = Color.Black;
                        }
                        b.Click += new EventHandler(buttn_click);
                        groupBox1.Controls.Add(b);
                    }
                }
               
            }
        }

        private void UpdateMap()
        {
            foreach (Unit u in map.Units)
            {

                if (u.GetType() == typeof(MeleeUnit))
                {
                    MeleeUnit m = (MeleeUnit)u;
                    if (m.Health < 25)// running away
                    {
                        switch (R.Next(0, 4))
                        {
                            case 0: m.Move(Direction.North); break;
                            case 1: m.Move(Direction.East); break;
                            case 2: m.Move(Direction.South); break;
                            case 3: m.Move(Direction.West); break;
                        }
                    }
                    else // in combat or moving toward
                    {
                        bool inCombat = false;
                        foreach (Unit e in map.Units)
                        {

                            if (u.inRange(e)) // in combat
                            {
                                u.Combat(e);
                                inCombat = true;
                            }
                        }

                        if(inCombat)
                        {
                            Unit c = m.Closest(map.Units);
                            m.Move(m.DirectionTo(c));
                        }
                    }
                }
                foreach (Unit Y in map.Units)
                {
                    if (Y.GetType() == typeof(RangedUnit))
                    {
                        RangedUnit r = (RangedUnit)Y;
                        if (r.Health < 25)// running away
                        {
                            switch (R.Next(0, 4))
                            {
                                case 0: r.Move(Direction.North); break;
                                case 1: r.Move(Direction.East); break;
                                case 2: r.Move(Direction.South); break;
                                case 3: r.Move(Direction.West); break;
                            }
                        }
                        else // in combat or moving toward
                        {
                            bool inCombat = false;
                            foreach (Unit e in map.Units)
                            {

                                if (Y.inRange(e)) // in combat
                                {
                                    Y.Combat(e);
                                    inCombat = true;
                                }
                            }

                            if (inCombat)
                            {
                                Unit c = r.Closest(map.Units);
                                r.Move(r.DirectionTo(c));
                            }
                        }
                    }
                }
            }
        }
            private void timer1_Tick(object sender, EventArgs e)
            {
                UpdateMap();
                DisplayMap();
            txtTurn.Text = (++turn).ToString();
            }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void buttn_click(object sender, EventArgs e)
        {
            int x = (((Button)sender).Location.X - groupBox1.Location.X) / SIZE;
            int y = (((Button)sender).Location.Y - groupBox1.Location.Y) / SIZE;
            txtInfo.Text = x + " " + y;
            foreach (Unit u in map.Units)
            {
                if (u.GetType() == typeof(MeleeUnit))
                {
                    MeleeUnit m = (MeleeUnit)u;
                    if (m.XPos == x && m.YPos == y)
                    {
                        txtInfo.Text = "Button Clicked at: " + m.ToString();
                    }
                }

                foreach (Unit Y in map.Units)
                { if (Y.GetType() == typeof(RangedUnit))
                    {
                        RangedUnit r = (RangedUnit)u;
                        if (r.XPos == x && r.YPos == y)
                        {
                            txtInfo.Text = "Button Clicked at: " + r.ToString();
                        }
                    }
                }
                
            }
        }
    }
}
