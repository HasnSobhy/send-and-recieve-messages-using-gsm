using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;




namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        SerialPort port = new SerialPort("com1", 9600, Parity.None, 8, StopBits.One);

        String number;
        String message, m;
        char ctrl_z = (char)26;
        char CR = (char)10;
        int remain;
        int n = 0;
        int k = 0;
        int i = 0;
        int z = 0;
        int a = 0;

        String command1, command2, command3;
        TextBox tb;
        String index, index1, index2;
        int index3;
        string[] arr = new string[100];
        
        public Form1()
        {
            
            InitializeComponent();

            if (!(port.IsOpen))
                port.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Recived_Read");
            comboBox1.Items.Add("Recived_Unread");
            comboBox1.Items.Add("Stored_Sent");
            comboBox1.Items.Add("Stored_Unsent");
            comboBox1.Items.Add("All_Messages");
            comboBox1.Items.Add("By Index");

            comboBox2.Items.Add("By index");
            comboBox2.Items.Add("Recived_Read");
            comboBox2.Items.Add("Recived_Read/Stored_Sent");
            comboBox2.Items.Add("Recived_Read/Stored_Sent/Stored_Unsent");
            comboBox2.Items.Add("All_Messages");
        }



        private void button2_Click(object sender, EventArgs e)
        {
          
            tb = new TextBox();
            k++;
            //tb.Name = "Num"+k;
            tb.Name = "num";
            Point p = new Point(230 + (n * 110), 16 + (25 * i));
            tb.Location = p;
            tb.BackColor = Color.LightBlue;
            this.Controls.Add(tb);
            n++;
            if (n == 2)
            {
                i++;
                n = 0;
            }


        }



        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            remain = 160 - textBox3.TextLength;
            label5.Text = remain.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "By Index")
            {
                tb = new TextBox();
                tb.Name = "txt1";
                Point p = new Point(727, 170);
                tb.Location = p;
                this.tb.Size = new System.Drawing.Size(45, 20);
                tb.BackColor = Color.LightBlue;
                this.Controls.Add(tb);


            }



        }



        private void button3_Click(object sender, EventArgs e)
        {
            port.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);

            // int p = 1;
            // number = tb.Text;
            message = textBox3.Text;
            z = 0;

            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Name == "num")
                    {
                        TextBox tb1 = (TextBox)item;
                        arr[z] = tb1.Text;
                        z++;
                       // command2 = "AT+CMGW=" + @"""" + item.Text + @"""" + CR + message + ctrl_z;

                    }


                }
            }

            command1 = "AT+CMGF=1";
            command2 = "AT+CMGW=" + @"""" +num.Text + @"""" + CR + message + ctrl_z;

           // command3 = "AT+CMSS=" + index2 + "," + @"""" + arr[i] + @"""" + CR;

            port.WriteLine(command1);
            Thread.Sleep(300);
            port.WriteLine(command2);
            Thread.Sleep(5000);

           





        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void DataRecievedHandler(object sender, SerialDataReceivedEventArgs e)
        {
           
            port = (SerialPort)sender;
            string w = port.ReadLine();

            if (w.Contains("+CMGW:"))
            {  /*
                string split = w.Substring((w.IndexOf('+') + 6), 2);
                Int32.TryParse(split, out index3);
                */
                index2 = w.Substring(7, 2);

                for (a = 0; a < z; a++)
                {
                    port.WriteLine("AT+CMSS=" + index2 + "," + @"""" + arr[a] + @"""" + CR);
                    Thread.Sleep(2000);
                }

                // index2 = w.Substring(w.IndexOf("+CMGW:") + 1, 2);
                // Int32.TryParse(index2, out index3);


            }
            Invoke(new Action(() => richTextBox1.AppendText(w)));
            Invoke(new Action(() => richTextBox1.ScrollToCaret()));
          


        }



        private void button4_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are You Want To Delete ??", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                String dcommand = "AT+CMGD=" + index1 + "," + 4 + CR;    //dcommand="AT+CMGDA" +CR;
                if (comboBox2.Text == "By index")
                {
                    foreach (Control c in this.Controls)
                    {
                        if (c is TextBox)
                        {
                            if (c.Name == "txt2")
                            {
                                index1 = c.Text;
                                dcommand = "AT+CMGD=" + index1 + CR;
                            }
                        }
                    }


                }
                else if (comboBox2.Text == "Recived_Read")
                {
                    dcommand = "AT+CMGD=" + 1 + "," + 1;      //T+CMGD=INDEX,1;
                }
                else if (comboBox2.Text == "Recived_Read/Stored_Sent")
                {
                    dcommand = "AT+CMGD=" + 1 + "," + 2;
                }
                else if (comboBox2.Text == "Recived_Read/Stored_Sent/Stored_Unsent")
                {
                    dcommand = "AT+CMGD=" + 1 + "," + 3;
                }

                else if (comboBox2.Text == "All_Messages")
                {
                    dcommand = "AT+CMGDA" + CR;          //dcommand="AT+CMGD=" + index1 + "," + 4; 
                }
                port.WriteLine(dcommand);
                port.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }


             

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.Text == "By index")
            {
                tb = new TextBox();
                tb.Name = "txt2";
                Point p = new Point(727, 207);
                tb.Location = p;
                this.tb.Size = new System.Drawing.Size(45, 20);
                tb.BackColor = Color.LightBlue;
                this.Controls.Add(tb);


            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

      
              /*  private void DataRecieved(object sender, SerialDataReceivedEventArgs e)
                {
                    port = (SerialPort)sender;
                    m = port.ReadExisting();


                    if (m != "")
                    {
                        Invoke(new Action(() => richTextBox1.AppendText(m)));
                        Invoke(new Action(() => richTextBox1.ScrollToCaret()));

                    }


                }*/




                private void button5_Click(object sender, EventArgs e)
                {


                    String rcommand = "AT+CMGL=\"ALL\"";
                    // String rcommand="AT+CMGL"+@""""+"ALL"+@""""+CR;


                    if (comboBox1.SelectedIndex == 0)
                    {
                        rcommand = "AT+CMGL=\"REC READ\"";

                    }

                    else if (comboBox1.SelectedIndex == 1)
                    {
                        rcommand = "AT+CMGL=\"REC UNREAD\"";
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        rcommand = "AT+CMGL=\"STO SENT\"";
                    }
                    else if (comboBox1.SelectedIndex == 3)
                    {
                        rcommand = "AT+CMGL=\"STO UNSENT\"";
                    }
                    else if (comboBox1.SelectedIndex == 4)
                    {
                        rcommand = "AT+CMGL=\"ALL\"";
                    }
                    else if (comboBox1.SelectedIndex == 5)
                    {
                        foreach (Control x in this.Controls)
                        {
                            if (x is TextBox)
                            {
                                if (x.Name == "txt1")
                                {
                                    TextBox tb = (TextBox)x;
                                    index = x.Text;
                                    rcommand = "AT+CMGR=" + index + CR;
                                }
                            }
                        }


                    }

                    port.WriteLine(rcommand);
                    port.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);



                }




            }
        }
    











       