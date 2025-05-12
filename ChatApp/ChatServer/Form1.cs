using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Open the ActiveServer form when the button is clicked, to view server activity.
            ActiveServer myActiveServer = new ActiveServer();
            myActiveServer.Show();
            this.Hide(); //Hide the current form to show the next form.
        }
    }
}
