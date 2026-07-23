using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Collections;

namespace Hwasung_Windows_SDK
{
    public partial class Form1 : Form
    {
        [DllImport("HwaUSB.dll")]
        public static extern int UsbOpen(string ModelName);
        [DllImport("HwaUSB.dll")]
        public static extern int PrintStr(string Str);
        [DllImport("HwaUSB.dll")]
        public static extern int PrintCmd(short data);
        [DllImport("HwaUSB.dll")]
        public static extern int NewRealRead();
        [DllImport("HwaUSB.dll")]
        public static extern int DummyRealRead();
        [DllImport("HwaUSB.dll")]
        public static extern void UsbClose();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("HMK-060");
            comboBox1.Items.Add("HMK-080");
            comboBox1.Items.Add("HMK-081");
            comboBox1.Items.Add("HMK-825");
            comboBox1.Items.Add("HMK-830");
            comboBox1.Items.Add("HMK-072");
            comboBox1.Items.Add("HMK-056");
            comboBox1.Items.Add("HMK-054");
            comboBox1.Items.Add("HP-083");
            comboBox1.Items.Add("HP-283");
            comboBox1.SelectedIndex = 0;


            comboBox2.Items.Add("USB");
            comboBox2.Items.Add("COM1");
            comboBox2.Items.Add("COM2");
            comboBox2.Items.Add("COM3");
            comboBox2.Items.Add("COM4");
            comboBox2.Items.Add("COM5");
            comboBox2.Items.Add("COM6");
            comboBox2.Items.Add("COM7");
            comboBox2.SelectedIndex = 0;

            comboBox3.Items.Add("9600");
            comboBox3.Items.Add("19200");
            comboBox3.Items.Add("38400");
            comboBox3.Items.Add("57600");
            comboBox3.Items.Add("115200");
            comboBox3.SelectedIndex = 0;

            comboBox4.Items.Add("UPC-E");
            comboBox4.Items.Add("EAN13");
            comboBox4.Items.Add("EAN8");
            comboBox4.Items.Add("CODE39");
            comboBox4.Items.Add("ITF(I of 2/5)");
            comboBox4.Items.Add("CODABAR");
            comboBox4.Items.Add("CODE128 A");
            comboBox4.Items.Add("CODE128 B");
            comboBox4.Items.Add("CODE128 C");
            comboBox4.SelectedIndex = 0;

            comboBox5.Items.Add("Version 1");
            comboBox5.Items.Add("Version 3");
            comboBox5.Items.Add("Version 5");
            comboBox5.Items.Add("Version 9");
            comboBox5.SelectedIndex = 0;

            label5.Text = "";
            textBox1.Text = "1234567890";
            textBox2.Text = "1234567890";
        }
        private int port_open(object port,int baudrate)
        {
            int port_open = 20;
            int a;
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();        //Serialport close
            }
            if (port.Equals("USB"))
            {
                a = UsbOpen(comboBox1.SelectedItem.ToString());
                port_open = a;
            }
            else
            {
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();        //Serialport close
                }

                serialPort1.PortName = (string)port;          //serial port
                serialPort1.DataBits = (int)8;                                  //set databit
                serialPort1.StopBits = StopBits.One;                            //set stopbit
                serialPort1.Parity = Parity.None;                               //set parity
                serialPort1.Handshake = Handshake.RequestToSend;                //set handshake RTS
                serialPort1.RtsEnable = true;
                serialPort1.Encoding = System.Text.Encoding.Default;            //set encoding
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                switch (baudrate)                                //select baudrate
                {
                    case 0:
                        serialPort1.BaudRate = (int)9600;
                        break;
                    case 1:
                        serialPort1.BaudRate = (int)19200;
                        break;
                    case 2:
                        serialPort1.BaudRate = (int)38400;
                        break;
                    case 3:
                        serialPort1.BaudRate = (int)57600;
                        break;
                    case 4:
                        serialPort1.BaudRate = (int)115200;
                        break;
                }
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();     //Serial Port open
                    port_open = 10;
                }
            }            
            return port_open;
        }
        private void button1_Click(object sender, EventArgs e)
        {   //printer status
            int prt_result;
            prt_result = port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex);
            if (prt_result == 10)  //serial
            {
                byte[] send = new byte[] { 0x10, 0x04, 0x02 };
                serialPort1.Write(send, 0, send.Length);
            }
            else if (prt_result == 0)//usb
            {
                prt_status((byte)NewRealRead());
            }
            else
            {
                MessageBox.Show("error : " + prt_result);
            }
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort sp = (SerialPort)sender;
            if (sp.IsOpen == false) sp.Open();
            string str = sp.ReadExisting();
            byte[] data = new byte[str.Length];
            data = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
            if (str.Length == 1)
            {
                prt_status(data[0]);
            }
            else if(str.Length == 4)
            {
                glover = "";                    
                glover = Encoding.ASCII.GetString(data);
                CSafeSetText((Control)textBox3, glover);
            }
        }
        
        delegate void CrossThreadSafetySetText(Control ctl, String text);
        private void CSafeSetText(Control ctl, String text)
        {
            if (ctl.InvokeRequired)
                ctl.Invoke(new CrossThreadSafetySetText(CSafeSetText), ctl, text);
            else
                ctl.Text = text;
        }
        string glopost;
        string glover;
       
        private void prt_status(byte status)
        {
            var bits = new BitArray(new byte[] { status });
            glopost = "";
            
            if (status == 0) glopost = glopost +"Normal Status";

            if (bits[0] == true)
            {
                glopost = glopost + "Paper out";
            }
          
            if (bits[1] == true)
            {
                glopost = glopost + "  Head open";
            }
           
            if (bits[2] == true)
            {
                glopost = glopost + "  Paper jam";
            }
            
            if (bits[3] == true)
            {
                glopost = glopost + "  Near end";
            }
           
            if (bits[4] == true)
            {
                glopost = glopost + " Running";
            }
           
            if (bits[5] == true)
            {
                glopost = glopost + " Cutter jam";
            }
            if (bits[7] == true)
            {
                glopost = glopost + " Paper not taken";
            }

            CSafeSetText((Control)label5,glopost);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                if (comboBox4.SelectedIndex+1 >= 7)
                {
                    serialPort1.Write((char)(0x1d) + "k" + (char)7);
                    switch(comboBox4.SelectedIndex+1 ){
                        case 7:
                            serialPort1.Write("g"+textBox1.Text +(char)0x00);
                            break;
                        case 8:
                            serialPort1.Write("h" + textBox1.Text + (char)0x00);
                            break;
                        case 9:
                            serialPort1.Write("i" + textBox1.Text + (char)0x00);
                            break;
                    }
                }
                else
                {
                    if (comboBox4.SelectedIndex + 1 == 1 || comboBox4.SelectedIndex + 1 == 3)
                    {
                        if (textBox1.TextLength < 7)
                        {
                            textBox1.Text = "0123456";
                        }
                        serialPort1.Write((char)(0x1d) + "k" + (char)(comboBox4.SelectedIndex + 1) + textBox1.Text.Substring(0,7) + (char)0x00);
                    }
                    else if (comboBox4.SelectedIndex + 1 == 2)
                    {
                        if (textBox1.TextLength < 12)
                        {
                            textBox1.Text = "012345678901";
                        }
                        serialPort1.Write((char)(0x1d) + "k" + (char)(comboBox4.SelectedIndex + 1) + textBox1.Text.Substring(0, 12) + (char)0x00);
                    }
                    else if (comboBox4.SelectedIndex + 1 == 5)
                    {
                        if (textBox1.TextLength % 2 == 0)
                        {
                            serialPort1.Write((char)(0x1d) + "k" + (char)(comboBox4.SelectedIndex + 1) + textBox1.Text + (char)0x00);
                        }
                        else
                        {
                            serialPort1.Write((char)(0x1d) + "k" + (char)(comboBox4.SelectedIndex + 1) + textBox1.Text + "0" + (char)0x00);
                        }
                    }
                    else
                    {
                        serialPort1.Write((char)(0x1d) + "k" + (char)(comboBox4.SelectedIndex + 1) + textBox1.Text + (char)0x00);
                    }
                }
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;

                if (comboBox4.SelectedIndex + 1 >= 7)
                {
                    a = PrintCmd(0x1d);
                    a = PrintStr("k");
                    a = PrintCmd(0x07);
                    switch (comboBox4.SelectedIndex + 1)
                    {
                        case 7:
                            a = PrintStr("g");
                            a = PrintStr(textBox1.Text);
                            a = PrintCmd(0x00);
                            break;
                        case 8:
                            a = PrintStr("h");
                            a = PrintStr(textBox1.Text);
                            a = PrintCmd(0x00);
                            break;
                        case 9:
                            a = PrintStr("i");
                            a = PrintStr(textBox1.Text);
                            a = PrintCmd(0x00);
                            break;
                    }
                }
                else
                {
                    if (comboBox4.SelectedIndex + 1 == 1 || comboBox4.SelectedIndex + 1 == 3)
                    {
                        if (textBox1.TextLength < 7)
                        {
                            textBox1.Text = "0123456";
                        }
                        a = PrintCmd(0x1d);
                        a = PrintStr("k");
                        a = PrintCmd((short)(comboBox4.SelectedIndex + 1));
                        a = PrintStr(textBox1.Text.Substring(0, 7));
                        a = PrintCmd(0x00);
                    }
                    else if (comboBox4.SelectedIndex + 1 == 2)
                    {
                        if (textBox1.TextLength < 12)
                        {
                            textBox1.Text = "012345678901";
                        }
                        a = PrintCmd(0x1d);
                        a = PrintStr("k");
                        a = PrintCmd((short)(comboBox4.SelectedIndex + 1));
                        a = PrintStr(textBox1.Text.Substring(0, 12));
                        a = PrintCmd(0x00);
                    }
                    else if (comboBox4.SelectedIndex + 1 == 5)
                    {
                        if (textBox1.TextLength % 2 == 0)
                        {
                            a = PrintCmd(0x1d);
                            a = PrintStr("k");
                            a = PrintCmd((short)(comboBox4.SelectedIndex + 1));
                            a = PrintStr(textBox1.Text);
                            a = PrintCmd(0x00);
                        }
                        else
                        {
                            a = PrintCmd(0x1d);
                            a = PrintStr("k");
                            a = PrintCmd((short)(comboBox4.SelectedIndex + 1));
                            a = PrintStr(textBox1.Text + "0");
                            a = PrintCmd(0x00);
                        }
                    }
                    else
                    {
                        a = PrintCmd(0x1d);
                        a = PrintStr("k");
                        a = PrintCmd((short)(comboBox4.SelectedIndex + 1));
                        a = PrintStr(textBox1.Text);
                        a = PrintCmd(0x00);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                serialPort1.Write((char)0x1B + "L");           // PAGE MODE
                serialPort1.Write((char)0x1B + "T" + (char)0x01);   // PAGE 방향
                // 좌표지정 및 문자열출력
                  
                serialPort1.Write((char)(0x1B) + "W" + "0010" + "1160");
                serialPort1.Write("Thermal Printer Ticket Sample");
  
                serialPort1.Write((char)(0x1B) + "W" + "0104" + "1160");
                serialPort1.Write("Hwasung System Thermal Printer");
  
                serialPort1.Write((char)(0x1B) + "W" + "0136" + "1160");
                serialPort1.Write("Sample Print");
   
                serialPort1.Write((char)(0x1D) + "!" + (char)(0x00));  // DOUBLE WIDTH SIZE
                serialPort1.Write((char)(0x1B) + "W" + "0216" + "1160");
                serialPort1.Write(DateTime.Now.ToString());
  
                serialPort1.Write((char)(0x1B) + "W" + "0280" + "1160");
                serialPort1.Write("Page Mode Ticket Sample");
      
                serialPort1.Write((char)(0x1D) + "!" + (char)(0x10));  // DOUBLE HEIGHT SIZE
                serialPort1.Write((char)(0x1B) + "W" + "0104" + "0304");
                serialPort1.Write("Page Mode");
    
                serialPort1.Write((char)(0x1D) + "!" + (char)(0x0));   //NORMAL SIZE
                serialPort1.Write((char)(0x1B) + "W" + "0168" + "0304");
                serialPort1.Write("Sample Print");
     
                serialPort1.Write((char)(0x1B) + "W" + "0416" + "1160");
                serialPort1.Write("Thermal Printer Ticket Sample");


                //--------- BARCODE --------------------------------
                serialPort1.Write( (char)(0x1B) + "W" + "0376" + "0688");
                serialPort1.Write( (char)(0x1D) + "h" +  (char)(40));   // barcode height
                serialPort1.Write( (char)(0x1D) + "k" +  (char)(5));   // barcode type
                serialPort1.Write( "010001200307311439" +  (char)(0));   // barcode data

               
                serialPort1.Write(""+ (char)(0x1B) + (char)(0x0C));   // PAGE AREA PRINT
                   
                serialPort1.Write( (char)(0x1B) + "S");       // PAGE AREA CLEAR AND TO STANDARD MODE


                // -----------------------
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;
                a = PrintCmd(0x1B);
                a = PrintStr("L");           // PAGE MODE SET
                a = PrintCmd(0x1B);
                a = PrintStr("T");
                a = PrintCmd(0x01);   // PAGE TOWARD

                // 좌표지정 및 문자열출력
                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0010");
                a = PrintStr("1160");
                a = PrintStr("Thermal Printer Ticket Sample");

                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0104");
                a = PrintStr("1160");
                a = PrintStr("Hwasung System Thermal Printer");

                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0136");
                a = PrintStr("1160");
                a = PrintStr("Sample Print");

                a = PrintCmd(0x1D);
                a = PrintStr("!");
                a = PrintCmd(0x00);

                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0216");
                a = PrintStr("1160");
                a = PrintStr(DateTime.Now.ToString());

                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0280");
                a = PrintStr("1160");
                a = PrintStr("Page Mode Ticket Sample");

                a = PrintCmd(0x1D);
                a = PrintStr("!");
                a = PrintCmd(0x10);

                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0104");
                a = PrintStr("0304");
                a = PrintStr("Page Mode");

                a = PrintCmd(0x1D);
                a = PrintStr("!");
                a = PrintCmd(0x00);

                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0168");
                a = PrintStr("0304");
                a = PrintStr("Sample Print");

                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0416");
                a = PrintStr("1160");
                a = PrintStr("Thermal Printer Ticket Sample");


                //--------- BARCODE --------------------------------
                a = PrintCmd(0x1B);
                a = PrintStr("W");
                a = PrintStr("0376");
                a = PrintStr("0688");
                a = PrintCmd(0x1D);// barcode height
                a = PrintStr("h");
                a = PrintCmd(0x40);
                a = PrintCmd(0x1D);// barcode type
                a = PrintStr("k");
                a = PrintCmd(0x05);
                a = PrintStr("010001200307311439" );
                a = PrintCmd(0x00);   // barcode data

                a = PrintCmd(0x1B);// PAGE AREA PRINT
                a = PrintCmd(0x0c);
               
                      // PAGE AREA CLEAR AND TO STANDARD MODE
                a = PrintCmd(0x1B);
                a = PrintStr("S");
            }            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                byte[] MyData;
                MyData = new byte[8];
                
                serialPort1.Write((char)0x1B + "L");           // PAGE MODE
                serialPort1.Write((char)0x1B + "T" + (char)0x00);   // PAGE TOWARD

                serialPort1.Write((char)0x1B + "W"); 
           
                MyData[0] = 0x00; //xL
                MyData[1] = 0x00; //xH
                MyData[2] = 0x00; //yL
                MyData[3] = 0x00; //yH
                MyData[4] = 0xA0; //dxL
                MyData[5] = 0x00; //dxH
                MyData[6] = 0xD9; //dyL
                MyData[7] = 0x00; //dyH
            
                //location x = 0mm      
                //location y = 0mm
                //dx =  20mm
                //dy =  27.125mm         
                serialPort1.Write(MyData,0,8); 
                serialPort1.Write("ABCDEFGHIJKLM"); 


                serialPort1.Write((char)0x1B + "W");
                MyData[0] = 0x70; //xL
                MyData[1] = 0x00; //xH
                MyData[2] = 0x90; //yL
                MyData[3] = 0x00; //yH
                MyData[4] = 0xA0; //dxL
                MyData[5] = 0x00; //dxH
                MyData[6] = 0xD9; //dyL
                MyData[7] = 0x00; //dyH


                //location x = 14mm
                //location y = 18mm
                //dx =  20mm
                //dy =  27.125mm         
                serialPort1.Write(MyData, 0, 8); 
                serialPort1.Write("1234567890123"); 


                serialPort1.Write((char)0x1B + "W");
                MyData[0] = 0xA0; //xL
                MyData[1] = 0x00; //xH
                MyData[2] = 0x60; //yL
                MyData[3] = 0x00; //yH
                MyData[4] = 0xA0; //dxL
                MyData[5] = 0x00; //dxH
                MyData[6] = 0xD9; //dyL
                MyData[7] = 0x00; //dyH

                //location x = 20mm
                //location y = 12mm
                //dx =  20mm
                //dy =  27.125mm      
                serialPort1.Write(MyData, 0, 8); 
                serialPort1.Write("123ABC456DEF7"); 

                serialPort1.Write(""+ (char)(0x1B) + (char)(0x0C));   // PAGE AREA PRINT
                   
                serialPort1.Write( (char)(0x1B) + "S");       // PAGE AREA CLEAR AND TO STANDARD MODE

            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;
                byte[] MyData;
                MyData = new byte[8];
                a = PrintCmd(0x1B );
                a = PrintStr("L");           // PAGE MODE

                a = PrintCmd(0x1B);
                a = PrintStr("T");  
                a = PrintCmd(0x00);   // PAGE TOWARD

                a = PrintCmd(0x1B);
                a = PrintStr("W"); 

                a = PrintCmd(0x00); //xL
                a = PrintCmd(0x00); //xH
                a = PrintCmd(0x00); //yL
                a = PrintCmd(0x00); //yH
                a = PrintCmd(0xA0); //dxL
                a = PrintCmd(0x00); //dxH
                a = PrintCmd(0xD9); //dyL
                a = PrintCmd(0x00); //dyH

                //location x = 0mm      
                //location y = 0mm
                //dx =  20mm
                //dy =  27.125mm         
                a = PrintStr("ABCDEFGHIJKLM");


                a = PrintCmd(0x1B);
                a = PrintStr("W"); 
                a = PrintCmd(0x70); //xL
                a = PrintCmd(0x00); //xH
                a = PrintCmd(0x90); //yL
                a = PrintCmd(0x00); //yH
                a = PrintCmd(0xA0); //dxL
                a = PrintCmd(0x00); //dxH
                a = PrintCmd(0xD9); //dyL
                a = PrintCmd(0x00); //dyH
                
                //location x = 14mm
                //location y = 18mm
                //dx =  20mm
                //dy =  27.125mm         
                a = PrintStr("1234567890123");


                a = PrintCmd(0x1B);
                a = PrintStr("W"); 
                a = PrintCmd(0xA0); //xL
                a = PrintCmd(0x00); //xH
                a = PrintCmd(0x60); //yL
                a = PrintCmd(0x00); //yH
                a = PrintCmd(0xA0); //dxL
                a = PrintCmd(0x00); //dxH
                a = PrintCmd(0xD9); //dyL
                a = PrintCmd(0x00); //dyH

                //location x = 20mm
                //location y = 12mm
                //dx =  20mm
                //dy =  27.125mm      
                a = PrintStr("123ABC456DEF7");

                a = PrintCmd(0x1B);
                a = PrintCmd(0x0c);         // PAGE AREA PRINT

                a = PrintCmd(0x1B);
                a = PrintStr("S");          // PAGE AREA CLEAR AND TO STANDARD MODE
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string str = comboBox5.SelectedItem.ToString();
            string val = str.Remove(0, str.Length - 1);
            int cnt;
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                cnt = textBox2.TextLength;
                serialPort1.Write((char)0x1B + "a" + (char)0x01);   // align center
                serialPort1.Write((char)0x1A + "B" + (char)0x02);
                serialPort1.Write("" + (char)cnt);
                switch (val){
                    case "1":
                        serialPort1.Write("" + (char)0x01);
                        break;
                    case "3":
                        serialPort1.Write("" + (char)0x03);
                        break;
                    case "5":
                        serialPort1.Write("" + (char)0x05);
                        break;
                    case "9":
                        serialPort1.Write("" + (char)0x09);
                        break;
                }                                   
                serialPort1.Write(textBox2.Text );
                serialPort1.Write((char)0x1B + "a" + (char)0x00);   // align left
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;
                cnt = textBox2.TextLength;
                //align center
                a = PrintCmd(0x1b);
                a = PrintStr("a");
                a = PrintCmd(0x01);

                a = PrintCmd(0x1A);
                a = PrintStr("B");
                a = PrintCmd(0x02);
                a = PrintCmd((short)cnt);
                switch (val)
                {
                    case "1":
                        a = PrintCmd(0x01);
                        break;
                    case "3":
                        a = PrintCmd(0x03);
                        break;
                    case "5":
                        a = PrintCmd(0x05);
                        break;
                    case "9":
                        a = PrintCmd(0x09);
                        break;
                }
                a = PrintStr(textBox2.Text);
                //align left
                a = PrintCmd(0x1b);
                a = PrintStr("a");
                a = PrintCmd(0x00);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                serialPort1.Write((char)0x1B +"i");
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;
                a = PrintCmd(0x1b);
                a = PrintStr("i");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                serialPort1.Write((char)0x1B + "m");
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;
                a = PrintCmd(0x1b);
                a = PrintStr("m");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                serialPort1.Write((char)0x13 + "i");
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;
                a = PrintCmd(0x13);
                a = PrintStr("i");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                byte[] MyData = new byte[1];
               
                serialPort1.Write((char)0x1B + "a" + (char)0x01);                       //Text Align center
                serialPort1.Write("Starbucks Coffee Germany" + (char)0x0A + (char)0x0A);
                serialPort1.Write("Frankfrut am Main" + (char)0x0A);
                serialPort1.Write("Kaiserstra");
                serialPort1.Write((char)0x1A + "x" + (char)0x01);                       //Extended ascii mode 
                MyData[0] = 225;
                serialPort1.Write(MyData,0,1);
                serialPort1.Write("e" + (char)0x0A);
                serialPort1.Write((char)0x1A + "x" + (char)0x00);
                serialPort1.Write("Tele:" + (char)0x0A);
                serialPort1.Write("VAT:  6417373R" + (char)0x0A + (char)0x0A);
                serialPort1.Write((char)0x1D + "L" + (char)0x12 + (char)0x00);
                serialPort1.Write(" 100003 Claire 0" + (char)0xA);
                serialPort1.Write("----------------------------------------" + (char)0x0A);
                serialPort1.Write("Chk 806             13Oct'16 15:48" + (char)0x0A);
                serialPort1.Write("----------------------------------------" + (char)0x0A);
                serialPort1.Write((char)0x1D + "L" + (char)0x32 + (char)0x00);
                serialPort1.Write((char)0x1D + "!" + (char)0x10);
                serialPort1.Write("To Go" + (char)0x0A);
                serialPort1.Write((char)0x1D + "!" + (char)0x00);
                serialPort1.Write("Gr Carml Macchiato                4.65" + (char)0x0A);
                serialPort1.Write("  Decaf" + (char)0x0A);
                serialPort1.Write("Gr Latte                          3.95" + (char)0x0A);
                serialPort1.Write("  Hazelnut                        0.05" + (char)0x0A);
                serialPort1.Write("Tl Chai Tea Latte                 3.45" + (char)0x0A);
                serialPort1.Write("Visa                             13.77" + (char)0x0A);
                serialPort1.Write("XXXXXXXXXXXX4258" + (char)0xA + (char)0xA);
                serialPort1.Write("Subtotal                    EURO 12.55" + (char)0x0A);
                serialPort1.Write("Tax 9.75%                   EURO  1.22" + (char)0x0A);
                serialPort1.Write("Total                       EURO 13.77" + (char)0x0A);
                serialPort1.Write("Change Due                  EURO  0.00" + (char)0x0A + (char)0x0A);
                serialPort1.Write("========================================" + (char)0x0A);
                serialPort1.Write("Thank you. Please visit us again" + (char)0x0A);
                serialPort1.Write("For more information visit" + (char)0x0A);
                serialPort1.Write("www.starbucks.de" + (char)0x0A + (char)0x0A + (char)0x0A + (char)0x0A + (char)0x0A+ (char)0x0A);

                serialPort1.Write( (char)0x1B + "i");                                       //Full Cut
                serialPort1.Write((char)0x1B + "a" + (char)0x00);                           //Text Align left
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                int a;
                a = PrintCmd(0x1A );
                a = PrintStr("x");
                a = PrintCmd(0x01);                       //Extended ascii mode 
                a = PrintCmd(0x1B);
                a = PrintStr("a");
                a = PrintCmd(0x01);                       //Text Align center
                a = PrintStr("Starbucks Coffee Germany" );
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintStr("Frankfrut am Main");
                a = PrintCmd(0x0A);
                a = PrintStr("Kaiserstra");
                a = PrintCmd(225);
                a = PrintStr("e" );
                a = PrintCmd(0x0A);
                a = PrintStr("Tele:");
                a = PrintCmd(0x0A);
                a = PrintStr("VAT:  6417373R");
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintCmd(0x1D);
                a = PrintStr("L"); 
                a = PrintCmd(0x12); 
                a = PrintCmd(0x00);
                a = PrintStr(" 100003 Claire 0");
                a = PrintCmd(0x0A);
                a = PrintStr("----------------------------------------");
                a = PrintCmd(0x0A);
                a = PrintStr("Chk 806             13Oct'16 15:48");
                a = PrintCmd(0x0A);
                a = PrintStr("----------------------------------------");
                a = PrintCmd(0x0A);
                a = PrintCmd(0x1D);
                a = PrintStr( "L");
                a = PrintCmd(0x32);
                a = PrintCmd(0x00);
                a = PrintCmd(0x1D );
                a = PrintStr("!"); 
                a = PrintCmd(0x10);
                a = PrintStr("To Go");
                a = PrintCmd(0x1D);
                a = PrintStr("!");
                a = PrintCmd(0x00);
                a = PrintCmd(0x0A);
                a = PrintStr("Gr Carml Macchiato                4.65");
                a = PrintCmd(0x0A);
                a = PrintStr("  Decaf");
                a = PrintCmd(0x0A);
                a = PrintStr("Gr Latte                          3.95");
                a = PrintCmd(0x0A);
                a = PrintStr("  Hazelnut                        0.05");
                a = PrintCmd(0x0A);
                a = PrintStr("Tl Chai Tea Latte                 3.45");
                a = PrintCmd(0x0A);
                a = PrintStr("Visa                             13.77");
                a = PrintCmd(0x0A);
                a = PrintStr("XXXXXXXXXXXX4258");
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintStr("Subtotal                    EURO 12.55");
                a = PrintCmd(0x0A);
                a = PrintStr("Tax 9.75%                   EURO  1.22");
                a = PrintCmd(0x0A);
                a = PrintStr("Total                       EURO 13.77");
                a = PrintCmd(0x0A);
                a = PrintStr("Change Due                  EURO  0.00");
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintStr("========================================");
                a = PrintCmd(0x0A);
                a = PrintStr("Thank you. Please visit us again"); 
                a = PrintCmd(0x0A);
                a = PrintStr("For more information visit");
                a = PrintCmd(0x0A);
                a = PrintStr("www.starbucks.de" );
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);
                a = PrintCmd(0x0A);

                a = PrintCmd(0x1B);
                a = PrintStr("i");                                       //Full Cut
                a = PrintCmd(0x1B);
                a = PrintStr("a");
                a = PrintCmd(0x00);                           //Text Align left
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int a = -1;
            string ver;

            if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 10)
            {
                serialPort1.Write((char)0x1D + "I" + "A");
            }
            else if (port_open(comboBox2.SelectedItem.ToString(), comboBox3.SelectedIndex) == 0)
            {
                Application.DoEvents();  
                PrintCmd(0x1D);    // GS
                PrintCmd(0x49);    // "I"
                PrintCmd(0x41);     // "A"
                System.Threading.Thread.Sleep(10);
                ver = char.ToString((char)DummyRealRead());
                System.Threading.Thread.Sleep(20);
                ver = ver + char.ToString((char)DummyRealRead());
                System.Threading.Thread.Sleep(20);
                ver = ver + char.ToString((char)DummyRealRead());
                System.Threading.Thread.Sleep(20);
                ver = ver + char.ToString((char)DummyRealRead());
                textBox3.Text = ver;
                UsbClose();                
            }        
        }
    }
}
