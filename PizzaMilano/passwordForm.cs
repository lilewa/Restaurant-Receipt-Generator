using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PizzaMilano
{
    public partial class passwordForm : Form
    {
        public string factor,cash;
        public passwordForm(string Factor,string Cash)
        {
            InitializeComponent();
            this.factor = Factor;
            this.cash = Cash;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

          

                if (txtPass.Text == "amind")
                {
                    Properties.Settings.Default.fileCounter = Convert.ToInt64(factor);
                    Properties.Settings.Default.TotalCost = Convert.ToInt64(cash);

                    Properties.Settings.Default.Save();
                    MessageBox.Show("Done!!! :D");
                }
                else
                    MessageBox.Show("You're Fucked! :D");
            }
            catch (Exception)
            {
            }
           
        }
    }
}
