using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluePuzzle
{
    public partial class Form1 : Form
    {
        #region Panels
        Point positionOfPanel;

        // When the main Form has been resized, this function will adjust the mainPanel's position
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.mainPannel.Location = new Point((this.Size.Width - this.mainPannel.Width) / 2, this.positionOfPanel.Y);
            }
            else if (WindowState == FormWindowState.Normal)
            {
                this.mainPannel.Location = this.positionOfPanel;
            }
        }
        #endregion

        #region Labels
        string contentOfLabel1 = "当前分数为：{0}";
        string contentOfLabel2 = "当前步数为：{0}";
        string contentOfLabel3 = "所有步数为：{0}";

        #endregion


        #region Buttons
        Button[,] buttons;


        // Create all the buttons
        private void createAndLayButtons()
        {
            for(int row = 0;row < Level; row++)
            {
                for(int col = 0; col < Level; col++)
                {
                    var button = createButton();
                    button.Tag = new Point{ Y = row, X = col };
                    var point = new Point();
                    point.Y = row * (button.Height + 1);
                    point.X = col * (button.Width + 1);
                    button.Location = point;
                    button.MouseClick += Button_MouseClick;
                    buttons[row,col] = button;   
                }

            }
            foreach (var button in buttons)
            {
                this.mainPannel.Controls.Add(button);
            }

        }


        private void Button_MouseClick(object sender, MouseEventArgs e)
        {

            var button = sender as Button;
            if (button == null) return;

            this.ReverseColor(button);
            var point = (Point)button.Tag;

            bool[] URDL = { true, true, true, true }; // Up,Right,Down,Left
            if (point.X == 0) URDL[3] = false;
            if (point.X == Level - 1) URDL[1] = false;
            if (point.Y == 0) URDL[0] = false;
            if (point.Y == Level - 1) URDL[2] = false;

            if (URDL[0]) this.ReverseColor(buttons[point.Y - 1, point.X]);
            if (URDL[1]) this.ReverseColor(buttons[point.Y, point.X + 1]);
            if (URDL[2]) this.ReverseColor(buttons[point.Y + 1, point.X]);
            if (URDL[3]) this.ReverseColor(buttons[point.Y, point.X - 1]);

            this.CurrentStep++;
            this.TotalStep++;
            Label2.Text=contentOfLabel2.Replace("{0}", this.CurrentStep.ToString());
            Label3.Text = contentOfLabel3.Replace("{0}",this.TotalStep.ToString());

            if (this.checkSuccess())
            {
                this.Level++;
                this.Label1.Text = contentOfLabel1.Replace("{0}", this.Level.ToString());
                this.CurrentStep = 0;
                ResetButtons();
            }
        }

        private void ResetButtons()
        {
            buttons = new Button[Level, Level];
            this.mainPannel.Controls.Clear();
            this.createAndLayButtons();
        }

        // Reverse the color
        void ReverseColor(Button button)
        {
            if(button.BackColor == Color.Blue)
            {
                button.BackColor = Color.Orange;
            }
            else
            {
                button.BackColor = Color.Blue;
            }

        }

        // Get a button with the given size.
        private Button createButton()
        {
            
            Button button = new Button();
            button.BackColor = Color.Orange;
            button.Height = (this.mainPannel.Height - Level + 1) / Level;
            button.Width = (this.mainPannel.Width - Level + 1) / Level;
            button.Margin = new Padding(0);
            button.Padding = new Padding(0);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;

            return button;

        }


        // Check if I win.
        private bool checkSuccess()
        {
            foreach(var button in buttons)
            {
                if(button.BackColor == Color.Orange)
                {
                    return false;
                }
            }


            return true;
        }
        #endregion

        #region Data
        int Level = 1;
        int TotalStep = 0;
        int CurrentStep = 0;

        #endregion






        public Form1()
        {
            InitializeComponent();
        }

        // This function does some initial behaviors, such as adding some buttons.
        private void Form1_Load(object sender, EventArgs e)
        {

            positionOfPanel = this.mainPannel.Location;
            this.Label1.Text = contentOfLabel1.Replace("{0}", this.Level.ToString());
            this.Label2.Text = contentOfLabel2.Replace("{0}", this.CurrentStep.ToString());
            this.Label3.Text = contentOfLabel3.Replace("{0}", this.TotalStep.ToString());
            buttons = new Button[Level, Level];
            this.createAndLayButtons();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.R)
            {
                Control[] c = new Control[this.Level * this.Level]; ;
                this.mainPannel.Controls.CopyTo(c, 0);
                this.createAndLayButtons();
                foreach (var i in c) i.Dispose();

                //this.mainPannel.Controls.Clear();
                //this.createAndLayButtons();

                this.TotalStep -= this.CurrentStep;
                this.CurrentStep = 0;
                Label2.Text = contentOfLabel2.Replace("{0}", this.CurrentStep.ToString());
                Label3.Text = contentOfLabel3.Replace("{0}", this.TotalStep.ToString());

            }
        }
    }
}
