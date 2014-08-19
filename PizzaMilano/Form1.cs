using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Media;
using System.IO;
using System.Diagnostics;
using System.Data.OleDb;
using System.Threading;


namespace PizzaMilano
{
    public partial class Form1 : Form
    {
        Bitmap screenshot, newbmp;
        SoundPlayer player;
        TextWriter tw;
        StreamWriter sw;
        List<Order> myOrders = new List<Order>();
        int counter,pizzaCount,sandwitchCount,otherCount,customerCount,price,m;
        long fileCount,tCost;
        string deskNum,path;
        public Form1()
        {
            InitializeComponent();
            counter = 0;
            m = 0;
            customerCount = 100;
            sandwitchCount = 0;
            otherCount = 0;
            deskNum = "";
            myOrders.Add(new Order());
            player = new SoundPlayer();
            fileCount = Properties.Settings.Default.fileCounter;
            path = Directory.GetCurrentDirectory() + "\\Factors\\Factor" + fileCount.ToString() + ".htm" ;
            webBrowser1.Url =new Uri(path);
            webBrowser2.Url = new Uri(path);
            txtCustomerNo.Text = Properties.Settings.Default.currentNumber;
            tCost = Properties.Settings.Default.TotalCost;
            txtFactorNumber.Text = fileCount.ToString();
            txtSetFactorNum.Text = txtFactorNumber.Text;
            txtTotalCost.Text = tCost.ToString();
            txtSetCashNum.Text = txtTotalCost.Text;
            
        }
        myDatabaseDataSetTableAdapters.PriceTableAdapter priceAdapter = new myDatabaseDataSetTableAdapters.PriceTableAdapter();
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'myDatabaseDataSet.Price' table. You can move, or remove it, as needed.
            this.priceTableAdapter.Fill(this.myDatabaseDataSet.Price);
            // TODO: This line of code loads data into the 'myDatabaseDataSet.Foods' table. You can move, or remove it, as needed.
            this.foodsTableAdapter.Fill(this.myDatabaseDataSet.Foods);

            listBoxNumOrder.SelectedIndex = 0;
        }

        private void PizzaClick(object sender, MouseEventArgs e)
        {
            myOrders[counter].myPizzas.Add(new Pizza(listBoxPizza.Text, Convert.ToInt32(listBoxNumOrder.Text)));
            listBoxOrder.Items.Add("پیتزا "+myOrders[counter].myPizzas[pizzaCount].name+" "+myOrders[counter].myPizzas[pizzaCount].numOrder.ToString());
            pizzaCount++;
            listBoxNumOrder.SelectedIndex = 0;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
          
            string s = listBoxOrder.Text;
           
                if (s.Contains("پیتزا"))
                {
                    Pizza toBeDeleted = myOrders[counter].myPizzas.Find(delegate(Pizza o) { return o.name == s.Split(' ')[1]; });
                    myOrders[counter].myPizzas.Remove(toBeDeleted);
                    pizzaCount--;
                }
                if (s.Contains("ساندویچ"))
                {
                    Sandwitch toBeDeleted = myOrders[counter].mySandwicthes.Find(delegate(Sandwitch o) { return o.name.Contains(s.Split(' ')[1]); });
                    myOrders[counter].mySandwicthes.Remove(toBeDeleted);
                    sandwitchCount--;
                }
                if (s.Contains("غیره"))
                {
                    Other toBeDeleted = myOrders[counter].myOthers.Find(delegate(Other o) { return o.name.Contains(s.Split(' ')[1]); });
                    myOrders[counter].myOthers.Remove(toBeDeleted);
                    otherCount--;
                }
                try
                {
                    listBoxOrder.Items.RemoveAt(listBoxOrder.SelectedIndex);

                }
                catch (Exception)
                {
                  
                }
                listBoxOrder.SelectedIndex = listBoxOrder.Items.Count-1;
           
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void listBoxSandwitch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            myOrders[counter].mySandwicthes.Add(new Sandwitch(listBoxSandwitch.Text, Convert.ToInt32(listBoxNumOrder.Text)));
            listBoxOrder.Items.Add("ساندویچ " + myOrders[counter].mySandwicthes[sandwitchCount].name + " " + myOrders[counter].mySandwicthes[sandwitchCount].numOrder.ToString());
            sandwitchCount++;
            listBoxNumOrder.SelectedIndex = 0;

        }

        private void listBoxOther_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            myOrders[counter].myOthers.Add(new Other(listBoxOther.Text, Convert.ToInt32(listBoxNumOrder.Text)));
            listBoxOrder.Items.Add("غیره " + myOrders[counter].myOthers[otherCount].name + " " + myOrders[counter].myOthers[otherCount].numOrder.ToString());
            otherCount++;
            listBoxNumOrder.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            customerCount = Convert.ToInt32(txtCustomerNo.Text);
            deskNum = comboBox1.Text;
            tw = new StreamWriter(path);
            price = 0;
            int temp = 0;
            #region HTML
            tw.WriteLine("<head> <style>");
            tw.WriteLine("td {");
            tw.WriteLine("text-align: center;");
            tw.WriteLine("}</head> </style>");
            tw.WriteLine("<img src=\"milano.jpg\" height=\"120\" width=\"350\" />");
            tw.WriteLine("<table>");
            tw.WriteLine("<tr>");
            tw.WriteLine("<td>");
            tw.WriteLine(customerCount.ToString());
            tw.WriteLine("</td>");
            tw.WriteLine("<td>");
            tw.WriteLine("شماره");
            tw.WriteLine("</td>");
            tw.WriteLine("</tr>");

            tw.WriteLine("<tr>");
            tw.WriteLine("<td>");
            tw.WriteLine(deskNum);
            tw.WriteLine("</td>");
            tw.WriteLine("<td>");
            tw.WriteLine("شماره میز");
            tw.WriteLine("</td>");
            tw.WriteLine("</tr>");

            tw.WriteLine("</table>");
            #region PRICE TABLE
            tw.WriteLine("<table  border=\"2\"    width=\"350\">");
           
           
            
            tw.WriteLine("<tr>");

            tw.WriteLine("<td>");
            tw.WriteLine("قیمت");
            tw.WriteLine("</td>");

            tw.WriteLine("<td>");
            tw.WriteLine("فی");
            tw.WriteLine("</td>");

            tw.WriteLine("<td>");
            tw.WriteLine("نام کالا");
            tw.WriteLine("</td>");

            tw.WriteLine("<td>");
            tw.WriteLine("مقدار");
            tw.WriteLine("</td>");


            tw.WriteLine("</tr>");


           
            for (int i = 0; i < myOrders[counter].myPizzas.Count; i++)
            {
                temp= myOrders[counter].myPizzas[i].numOrder * Convert.ToInt32(priceAdapter.getPrice(myOrders[counter].myPizzas[i].name));
                price += temp;


                tw.WriteLine("<tr>");

                tw.WriteLine("<td>");
                tw.WriteLine(temp.ToString());
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(priceAdapter.getPrice(myOrders[counter].myPizzas[i].name));
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(myOrders[counter].myPizzas[i].name);
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(myOrders[counter].myPizzas[i].numOrder.ToString());
                tw.WriteLine("</td>");

                tw.WriteLine("</tr>");
            }

            for (int i = 0; i < myOrders[counter].mySandwicthes.Count; i++)
            {
                temp = myOrders[counter].mySandwicthes[i].numOrder * Convert.ToInt32(priceAdapter.getPrice(myOrders[counter].mySandwicthes[i].name));
                price += temp;


                tw.WriteLine("<tr>");

                tw.WriteLine("<td>");
                tw.WriteLine(temp.ToString());
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(priceAdapter.getPrice(myOrders[counter].mySandwicthes[i].name));
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(myOrders[counter].mySandwicthes[i].name);
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(myOrders[counter].mySandwicthes[i].numOrder.ToString());
                tw.WriteLine("</td>");

                tw.WriteLine("</tr>");
            }
            for (int i = 0; i < myOrders[counter].myOthers.Count; i++)
            {
                temp = myOrders[counter].myOthers[i].numOrder * Convert.ToInt32(priceAdapter.getPrice(myOrders[counter].myOthers[i].name));
                price += temp;


                tw.WriteLine("<tr>");

                tw.WriteLine("<td>");
                tw.WriteLine(temp.ToString());
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(priceAdapter.getPrice(myOrders[counter].myOthers[i].name));
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(myOrders[counter].myOthers[i].name);
                tw.WriteLine("</td>");

                tw.WriteLine("<td>");
                tw.WriteLine(myOrders[counter].myOthers[i].numOrder.ToString());
                tw.WriteLine("</td>");

                tw.WriteLine("</tr>");
            }
           
            tw.WriteLine("</table>");
            #endregion

            tw.WriteLine("<br><hr><table  border=\"2\"    width=\"350\" >");
            tw.WriteLine("<tr>");
            tw.WriteLine("<td>");
            tw.WriteLine(price.ToString());
            tw.WriteLine("</td>");
            
            
            tw.WriteLine("<td>");
            tw.WriteLine("مجموع");
            tw.WriteLine("</td>");
            tw.WriteLine("</tr>");
            tw.WriteLine("</table>");
            tw.WriteLine(DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString());
            tw.Close();
            #endregion
            
            webBrowser1.Refresh();
            webBrowser2.Refresh();
            listBoxNumOrder.SelectedIndex = 1;
            buttonPrint.Enabled = true;
           
            screenshot = new Bitmap(760, 2200);
            NativeMethods.GetImage(webBrowser2.ActiveXInstance, screenshot, Color.White);

            pictureBox1.Image = (Image)screenshot;
          

        }
       
private void btnPrint_Click(object sender, EventArgs e)
{
    customerCount++;
    txtCustomerNo.Text = customerCount.ToString();
recordDoc.DocumentName ="Customer Receip";
//recordDoc.PrintPage += new PrintPageEventHandler(this.PrintReceiptPage);

// Preview document
dlgPreview.Document = recordDoc;
//dlgPreview.ShowDialog();

screenshot = new Bitmap(760, 2200);


NativeMethods.GetImage(webBrowser2.ActiveXInstance, screenshot, Color.White);

recordDoc.Print();
recordDoc.Print();
recordDoc.Print();
    // Dispose of document when done printing
recordDoc.Dispose();
listBoxOrder.Items.Clear();


Properties.Settings.Default.currentNumber = txtCustomerNo.Text;
Properties.Settings.Default.Save();
buttonPrint.Enabled = false;
myOrders.Add(new Order());
pizzaCount = 0;
sandwitchCount = 0;
otherCount = 0; 
counter++;
fileCount++;
Properties.Settings.Default.fileCounter = fileCount;
Properties.Settings.Default.Save();
path=Directory.GetCurrentDirectory() + "\\Factors\\Factor" + fileCount.ToString() + ".htm";
webBrowser1.Url = new Uri(path);
webBrowser2.Url = new Uri(path);

tCost += price;
Properties.Settings.Default.TotalCost = tCost;
Properties.Settings.Default.Save();
txtTotalCost.Text = tCost.ToString();
txtFactorNumber.Text = fileCount.ToString();

}

private void PrintReceiptPage(object sender, PrintPageEventArgs e)

{
string message;
int y;
// Print receipt
Font myFont = new Font("Arial", 12, FontStyle.Bold);


y = e.MarginBounds.Y;
e.Graphics.DrawImage((Image)screenshot, 0, 0);
   
}

private void tabControl1_TabIndexChanged(object sender, EventArgs e)
{
    

}



private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
{
    //create a DataTable to hold the query results
   // this.myDatabaseDataSet.Foods = (myDatabaseDataSet.FoodsDataTable)foodsBindingSource3.DataSource;
}

private void button2_Click(object sender, EventArgs e)
{
    customerCount = Convert.ToInt32(txtCustomerNo.Text);

}

private void tabPage1_Click(object sender, EventArgs e)
{

}

private void deskNum_Event(object sender, EventArgs e)
{
    deskNum = comboBox1.Text;
}

private void دربارهنرمافزارToolStripMenuItem_Click(object sender, EventArgs e)
{
    MessageBox.Show("wRiTTen bY: aminD\n\n E-mail: amin.dorost@gmail.com");
}

private void بروزرسانیToolStripMenuItem_Click(object sender, EventArgs e)
{
    //this.foodsTableAdapter.Update(dataGridViewFood.t);
}

private void hToolStripMenuItem_Click(object sender, EventArgs e)
{
    dataGridViewFood.EndEdit();
    dataGridViewPrice.EndEdit();
    foodsTableAdapter.Update((PizzaMilano.myDatabaseDataSet.FoodsDataTable)myDatabaseDataSet.Tables[0]);
    priceTableAdapter.Update((PizzaMilano.myDatabaseDataSet.PriceDataTable)myDatabaseDataSet.Tables[1]);
    // TODO: This line of code loads data into the 'myDatabaseDataSet.Price' table. You can move, or remove it, as needed.
    this.priceTableAdapter.Fill(this.myDatabaseDataSet.Price);
    // TODO: This line of code loads data into the 'myDatabaseDataSet.Foods' table. You can move, or remove it, as needed.
    this.foodsTableAdapter.Fill(this.myDatabaseDataSet.Foods);

   // bSource.DataSource=
    //this.foodsTableAdapter.Update(dataGridViewFood.Rows);
}

private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
{
  
}

private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
{

}

private void resetToolStripMenuItem_Click(object sender, EventArgs e)
{
    Properties.Settings.Default.TotalCost = 0;
    Properties.Settings.Default.fileCounter = 0;
    Properties.Settings.Default.Save();
}

private void dataGridViewFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
{

}

private void listBoxOrder_SelectedIndexChanged(object sender, EventArgs e)
{

}

private void button3_Click(object sender, EventArgs e)
{
    try
    {
        player.SoundLocation = Directory.GetCurrentDirectory() + "\\Sounds\\" + numToCall.Value.ToString() + ".wav";
        player.Play();
    }
    catch (Exception)
    { }
}

private void button5_Click(object sender, EventArgs e)
{
    passwordForm passForm = new passwordForm(txtSetFactorNum.Text,txtSetCashNum.Text);
    passForm.ShowDialog();
   
}

private void button6_Click(object sender, EventArgs e)
{
    Properties.Settings.Default.TotalCost = Convert.ToInt64(txtSetCashNum.Text);
    Properties.Settings.Default.Save();
}


      
    }
}
