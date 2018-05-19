using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyCube_Undermove
{
    public partial class LeaderBoardForm : Form
    {
        int _currentScore = 0;

        public LeaderBoardForm(int currentScore)
        {
            InitializeComponent();
            _currentScore = currentScore;
            try
            {
                textBox1.Text = File.ReadAllText("leaders.lol");
            }
            catch (FileNotFoundException)
            {
                File.Create("leaders.lol");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.AppendText(textBox2.Text + 
                ":" + 
                _currentScore.ToString() + 
                Environment.NewLine);
        }

        private void LeaderBoardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText("leaders.lol", textBox1.Text);
        }
    }
}
