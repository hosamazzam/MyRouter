using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Media;

namespace MyRouter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(this.Form1_FormClosed);
        }

        int status = -1;
        
        
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private void statusinfo()
        {
            RunCMD("cls", 0);
            RunCMD("cd C:\\Windows\\System32", 0);
            RunCMD("netsh wlan show hostednetwork", 1);
        }

        private void RunCMD(string filename , int num)
        {
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            procInfo.CreateNoWindow = true;
            procInfo.RedirectStandardOutput = true;
            procInfo.RedirectStandardInput = true;
            procInfo.UseShellExecute = false;
            
            var proc = Process.Start(procInfo);
          
           

            if (num == 1) // Show status of online or of line 
            {
                proc.StandardInput.WriteLine(filename);
                proc.StandardInput.WriteLine("exit");

                string result = proc.StandardOutput.ReadToEnd();
                string data = getBetween(result, "Status", "\n");
              
                if (data != "")
                {
                    string output = getBetween(data, ":", "d");
                    string outputfail = getBetween(data, ":", "e");
                    if (output == " Starte")
                    {
                        status = 1;
                        start_stop.Text = "Stop";
                        Service_notafiction_nenu.Text = "Stop";
                        Status.Text = "    Started";
                        Status.BackColor = System.Drawing.Color.Lime;
                    }
                    
                    if (output == " Not starte")
                    {
                        status =0;
                        start_stop.Text = "Start";
                        Service_notafiction_nenu.Text = "Start";
                        Status.Text = "       Stop";
                        Status.BackColor = System.Drawing.Color.Red;
                    }
                    if (outputfail == " Not availabl")
                    {
                        status = 0;
                        start_stop.Text = "Start";
                        Service_notafiction_nenu.Text = "Start";
                        Status.Text = "       Stop";
                        Status.BackColor = System.Drawing.Color.Red;
                        RunCMD("cd C:\\Windows\\System32", 0);
                        RunCMD("netsh wlan set hostednetwork mode=allow ssid=H-router" +
                            " key=123456789" + "  keyUsage=persistent", 4);
                    }
                }
                if (data == "")
                {
                    status = -1;
                    Status.Text = " Not support";
                    Service_notafiction_nenu.Text = "Not support";
                    Status.BackColor = System.Drawing.Color.Silver;

                    MessageBox.Show("Your machine doesn't support this program.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
               //     Application.Exit();
                    Properties.Settings.Default.Save();

                }
               
                
               
            }


            else if (num == 2) //start host
            {
                proc.StandardInput.WriteLine(filename);
                proc.StandardInput.WriteLine("exit");

                string result = proc.StandardOutput.ReadToEnd();


                string data = getBetween(result, "hostednetwork", ".");

                string first_run = getBetween(result, "administrator", ".");


                if (first_run == " privilege")
                {

                    MessageBox.Show("You must run this program as administrator privilege\n application will be Close", "First Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                    Properties.Settings.Default.Save();

                }
                else
                {
                    MessageBox.Show(data, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Status.Text = "    Started";
                    Status.BackColor = System.Drawing.Color.Lime;
                }
            }


            else if (num == 3) //stop host
            {
                proc.StandardInput.WriteLine(filename);
                proc.StandardInput.WriteLine("exit");

                string result = proc.StandardOutput.ReadToEnd();


                string data = getBetween(result, "hostednetwork", ".");
                SystemSounds.Asterisk.Play();
                MessageBox.Show(data, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Status.Text = "    Started";
                Status.BackColor = System.Drawing.Color.Lime;
            }

            else if (num == 4) //change host name & password
            {
                proc.StandardInput.WriteLine(filename);
                proc.StandardInput.WriteLine("exit");

                string result = proc.StandardOutput.ReadToEnd();

                string first_run = getBetween(result, "administrator", ".");


                if (first_run == " privilege")
                {

                    MessageBox.Show("You must run this program as administrator privilege\n application will be Close", "First Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                    Properties.Settings.Default.Save();

                }
                else
                {
                    string data = getBetween(result, "keyUsage=persistent", ".");
                    data += getBetween(result, "allow.", ".");
                    data += getBetween(result, "successfully changed.", ".");
                    data += "\nPlease click start to Sart the host.";
                    textBox1.Text = textBox1_name.Text;
                    textBox2.Text = textBox2_password.Text;
                    MessageBox.Show(data, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else if (num == 5) //configration host
            {

                proc.StandardInput.WriteLine("netsh wlan show hostednetwork");
                proc.StandardInput.WriteLine(filename);
                proc.StandardInput.WriteLine("exit");

                string result = proc.StandardOutput.ReadToEnd();

                string slash = "\"";

                string data = getBetween(result, "SSID name              : " + slash, slash);

                textBox1_name.Text = data;
                textBox1.Text = data;


                data = getBetween(result, "User security key      : ", "\n");

                textBox2_password.Text = data;
                textBox2.Text = data;


            }

            else if (num == 6) //information host
            {


                proc.StandardInput.WriteLine(filename);
                proc.StandardInput.WriteLine("netsh wlan show drivers");
                proc.StandardInput.WriteLine("exit");

                string result = proc.StandardOutput.ReadToEnd();

                string data = "Interface" + getBetween(result, "Interface", "Connection");
                data += "Connection\n----------------------------------------------\n";
                data += "    Driver" + getBetween(result, "Driver", "Authentication");

                data += "\n    ----------------------------------------------------------------\n\n";
                data += "Hosted" + getBetween(result, "Hosted", "Hosted network status");


                MessageBox.Show(data, "INFO");



            }
            else

                proc.StandardInput.WriteLine(filename);
                proc.StandardInput.WriteLine("exit");


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RunCMD("cls", 0);
            RunCMD("cd C:\\Windows\\System32", 0);
            RunCMD("netsh wlan show hostednetwork", 1);
            RunCMD("netsh wlan show hostednetwork setting=security", 5);
            notifyIcon1.ShowBalloonTip(1000, "H-Router", "Welcome Back", ToolTipIcon.Info);
            this.BackColor = Properties.Settings.Default.formthems;





            if (Properties.Settings.Default.thems =="white")
            {
                WhiteToolStripMenuItem.Checked = true;
            }

            if (Properties.Settings.Default.thems == "sliver")
            {
                silverToolStripMenuItem.Checked = true;
            }


            if (Properties.Settings.Default.thems == "black")
            {
               blackToolStripMenuItem.Checked = true;
            }







            if (Properties.Settings.Default.formbackground == "Red")
            {
              this.BackgroundImage = global::MyRouter.Properties.Resources.Red;
              redToolStripMenuItem.Checked = true;
            }

            if (Properties.Settings.Default.formbackground == "blue")
            {
                this.BackgroundImage = global::MyRouter.Properties.Resources.blue;
                blueToolStripMenuItem.Checked = true;
            }

            if (Properties.Settings.Default.formbackground == "white")
            {
                this.BackgroundImage = global::MyRouter.Properties.Resources.white;
                whiteToolStripMenuItem1.Checked = true;
            }

            if (Properties.Settings.Default.formbackground == "yellow")
            {
                this.BackgroundImage = global::MyRouter.Properties.Resources.yellow;
                yellowToolStripMenuItem.Checked = true;
            }

            if (Properties.Settings.Default.formbackground == "green")
            {
                this.BackgroundImage = global::MyRouter.Properties.Resources.green;
                greenToolStripMenuItem.Checked = true;
            }

            if (Properties.Settings.Default.formbackground == "brown")
            {
                this.BackgroundImage = global::MyRouter.Properties.Resources.brown;
                brownToolStripMenuItem.Checked = true;
            }

            if (Properties.Settings.Default.formbackground == "black")
            {
                this.BackgroundImage = global::MyRouter.Properties.Resources.black;
                blackToolStripMenuItem1.Checked = true;
            }

            if (Properties.Settings.Default.formbackground == "none")
            {
                this.BackgroundImage =null;
                noneToolStripMenuItem.Checked = true;
            }

        }

        private void start_stop_Click(object sender, EventArgs e)
        {
            if (status == 0)
            {
                
                RunCMD("cd C:\\Windows\\System32", 0);
                RunCMD("netsh wlan start hostednetwork", 2);
                RunCMD("netsh wlan show hostednetwork", 1);
                start_stop.Text = "Stop";
                Service_notafiction_nenu.Text = "Stop Serviceop";
                startToolStripMenuItem.Text = "Stop Service";
            }
            else if (status == 1)
            {
              
                RunCMD("cd C:\\Windows\\System32", 0);
                RunCMD("netsh wlan stop hostednetwork", 3);
                RunCMD("netsh wlan show hostednetwork", 1);
                start_stop.Text = "Start";
                Service_notafiction_nenu.Text = "Start Service";
                startToolStripMenuItem.Text = "Start Service";
            }
        }

        private void Configration_Click(object sender, EventArgs e)
        {
            if (Change.Visible == true)
            {
                textBox1_name.Visible = false;
                textBox2_password.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                Change.Visible = false;
            }
            else
            {
                textBox1_name.Visible = true;
                textBox2_password.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                Change.Visible = true;
                RunCMD("netsh wlan show hostednetwork setting=security", 5);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
                RunCMD("cd C:\\Windows\\System32", 0);
                RunCMD("netsh wlan set hostednetwork mode=allow ssid=" + textBox1_name.Text +
                    " key=" + textBox2_password.Text + "  keyUsage=persistent", 4);
                textBox1_name.Visible = false;
                textBox2_password.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                Change.Visible = false;
                statusinfo();
           
        }

        private void button2_Click(object sender, EventArgs e) //هinformation
        {
            RunCMD("cd C:\\Windows\\System32", 0);
            RunCMD("netsh wlan show hostednetwork", 6);
        }

        private void Form1_FormClosed(object sender, CancelEventArgs e)
        {
            if (status == 1)
            {
               DialogResult exit = MessageBox.Show("The hotspot will be stopped Are you sure ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
               if (exit == DialogResult.Yes)
               {
                   e.Cancel = false;

                   RunCMD("cd C:\\Windows\\System32", 0);
                   RunCMD("netsh wlan stop hostednetwork", 0);
                   Application.Exit();
                   Properties.Settings.Default.Save();
               }
               else
                   e.Cancel = true ;
            }
            else
            {
                Application.Exit();
                Properties.Settings.Default.Save();
            }
                
        }


        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.ShowBalloonTip(2000, "Still Running ", "H-Router is still running \nDouble click to open ", ToolTipIcon.Info);
                this.Hide();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                DialogResult exit = MessageBox.Show("The hotspot will be stopped Are you sure ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (exit == DialogResult.Yes)
                {
                   

                    RunCMD("cd C:\\Windows\\System32", 0);
                    RunCMD("netsh wlan stop hostednetwork", 0);
                    Application.Exit();
                    Properties.Settings.Default.Save();
                }
                
                   
            }
            else
            {
                Application.Exit();
                Properties.Settings.Default.Save();
            }
        }
       
        private void information_notafiction_nenu_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void Help_notafiction_nenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is simple program if you don't have WiFi Router"+
                " but have labtop you can use this program to convert your"+
                " labtop to WiFi Router to share your network to other devices", "Help");
        
        }

        private void Service_notafiction_nenu_Click(object sender, EventArgs e)
        {
            start_stop_Click(sender, e);
        }

        private void show_notafiction_nenu_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void configrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configration_Click(sender, e);
        }

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                DialogResult exit = MessageBox.Show("The hotspot will be stopped Are you sure ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (exit == DialogResult.Yes)
                {
                    RunCMD("cd C:\\Windows\\System32", 0);
                    RunCMD("netsh wlan stop hostednetwork", 0);
                    Application.Exit();
                    Properties.Settings.Default.Save();
                }
               
            }
            else
            {
                Application.Exit();
                Properties.Settings.Default.Save();
            }
           
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help_notafiction_nenu_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f2 = new about();
            f2.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_name_TextChanged(object sender, EventArgs e)
        {

        }





        private void silverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.Silver;
            Properties.Settings.Default.formthems = System.Drawing.Color.Silver;
            Properties.Settings.Default.thems="sliver";

            silverToolStripMenuItem.Checked = true;
            WhiteToolStripMenuItem.Checked = false;
            blackToolStripMenuItem.Checked = false;
            label1.ForeColor = System.Drawing.Color.Black;
            label2.ForeColor = System.Drawing.Color.Black;
            label3.ForeColor = System.Drawing.Color.Black;
            label4.ForeColor = System.Drawing.Color.Black;
            label6.ForeColor = System.Drawing.Color.Black;
            menuStrip1.ForeColor = System.Drawing.Color.Black;


        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.Black;
            Properties.Settings.Default.formthems = System.Drawing.Color.Black;
            Properties.Settings.Default.thems = "black";

            silverToolStripMenuItem.Checked =false;
            WhiteToolStripMenuItem.Checked = false;
            blackToolStripMenuItem.Checked = true;
            label1.ForeColor = System.Drawing.Color.White;
            label2.ForeColor = System.Drawing.Color.White;
            label3.ForeColor = System.Drawing.Color.White;
            label4.ForeColor = System.Drawing.Color.White;
            label6.ForeColor = System.Drawing.Color.White;
            menuStrip1.ForeColor = System.Drawing.Color.White;
           
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.White;
            Properties.Settings.Default.formthems = System.Drawing.Color.White;
            Properties.Settings.Default.thems = "white";

            silverToolStripMenuItem.Checked = false;
            WhiteToolStripMenuItem.Checked = true;
            blackToolStripMenuItem.Checked = false;
            label1.ForeColor = System.Drawing.Color.Black;
            label2.ForeColor = System.Drawing.Color.Black;
            label3.ForeColor = System.Drawing.Color.Black;
            label4.ForeColor = System.Drawing.Color.Black;
            label6.ForeColor = System.Drawing.Color.Black;
            menuStrip1.ForeColor = System.Drawing.Color.Black;
        }




        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = global::MyRouter.Properties.Resources.Red;
            Properties.Settings.Default.formbackground = "Red";

            redToolStripMenuItem.Checked = true;
            whiteToolStripMenuItem1.Checked = false;
            blackToolStripMenuItem1.Checked = false;
            blueToolStripMenuItem.Checked = false;
            noneToolStripMenuItem.Checked = false;
            greenToolStripMenuItem.Checked = false;
            brownToolStripMenuItem.Checked = false;
            yellowToolStripMenuItem.Checked = false;
            if (blackToolStripMenuItem.Checked == true)
            {
                label1.ForeColor = System.Drawing.Color.White;
                label2.ForeColor = System.Drawing.Color.White;
                label3.ForeColor = System.Drawing.Color.White;
                label4.ForeColor = System.Drawing.Color.White;
                label6.ForeColor = System.Drawing.Color.White;
                menuStrip1.ForeColor = System.Drawing.Color.White;
            }
            
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage =null;
            Properties.Settings.Default.formbackground = "null";

            redToolStripMenuItem.Checked = false;
            whiteToolStripMenuItem1.Checked = false;
            blackToolStripMenuItem1.Checked = false;
            blueToolStripMenuItem.Checked = false;
            noneToolStripMenuItem.Checked = true;
            greenToolStripMenuItem.Checked = false;
            brownToolStripMenuItem.Checked = false;
            yellowToolStripMenuItem.Checked = false;
            if (blackToolStripMenuItem.Checked == false)
            {
                label1.ForeColor = System.Drawing.Color.Black;
                label2.ForeColor = System.Drawing.Color.Black;
                label3.ForeColor = System.Drawing.Color.Black;
                label4.ForeColor = System.Drawing.Color.Black;
                label6.ForeColor = System.Drawing.Color.Black;
                menuStrip1.ForeColor = System.Drawing.Color.Black;
            }
            else
                label1.ForeColor = System.Drawing.Color.White;
            label2.ForeColor = System.Drawing.Color.White;
            label3.ForeColor = System.Drawing.Color.White;
            label4.ForeColor = System.Drawing.Color.White;
            label6.ForeColor = System.Drawing.Color.White;
            menuStrip1.ForeColor = System.Drawing.Color.White;
        }

        private void whiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = global::MyRouter.Properties.Resources.white;
            Properties.Settings.Default.formbackground = "white";

            redToolStripMenuItem.Checked = false;
            whiteToolStripMenuItem1.Checked = true;
            blackToolStripMenuItem1.Checked = false;
            blueToolStripMenuItem.Checked = false;
            noneToolStripMenuItem.Checked = false;
            greenToolStripMenuItem.Checked = false;
            brownToolStripMenuItem.Checked = false;
            yellowToolStripMenuItem.Checked = false;
            label1.ForeColor = System.Drawing.Color.Black;
            label2.ForeColor = System.Drawing.Color.Black;
            label3.ForeColor = System.Drawing.Color.Black;
            label4.ForeColor = System.Drawing.Color.Black;
            label6.ForeColor = System.Drawing.Color.Black;
            menuStrip1.ForeColor = System.Drawing.Color.Black;
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
        this.BackgroundImage = global::MyRouter.Properties.Resources.yellow;
        Properties.Settings.Default.formbackground = "yellow";

        redToolStripMenuItem.Checked = false;
        whiteToolStripMenuItem1.Checked = false;
        blackToolStripMenuItem1.Checked = false;
        blueToolStripMenuItem.Checked = false;
        noneToolStripMenuItem.Checked = false;
        greenToolStripMenuItem.Checked = false;
        brownToolStripMenuItem.Checked = false;
        yellowToolStripMenuItem.Checked = true;

        label1.ForeColor = System.Drawing.Color.Black;
        label2.ForeColor = System.Drawing.Color.Black;
        label3.ForeColor = System.Drawing.Color.Black;
        label4.ForeColor = System.Drawing.Color.Black;
        label6.ForeColor = System.Drawing.Color.Black;
        menuStrip1.ForeColor = System.Drawing.Color.Black;
        }
        

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
           this.BackgroundImage = global::MyRouter.Properties.Resources.green;
           Properties.Settings.Default.formbackground = "green";

           redToolStripMenuItem.Checked = false;
           whiteToolStripMenuItem1.Checked = false;
           blackToolStripMenuItem1.Checked = false;
           blueToolStripMenuItem.Checked = false;
           noneToolStripMenuItem.Checked = false;
           greenToolStripMenuItem.Checked = true;
           brownToolStripMenuItem.Checked = false;
           yellowToolStripMenuItem.Checked = false;
           if (blackToolStripMenuItem.Checked == true)
           {
               label1.ForeColor = System.Drawing.Color.White;
               label2.ForeColor = System.Drawing.Color.White;
               label3.ForeColor = System.Drawing.Color.White;
               label4.ForeColor = System.Drawing.Color.White;
               label6.ForeColor = System.Drawing.Color.White;
               menuStrip1.ForeColor = System.Drawing.Color.White;
           }
        }
        

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
        this.BackgroundImage = global::MyRouter.Properties.Resources.blue;
        Properties.Settings.Default.formbackground = "blue";

        redToolStripMenuItem.Checked = false;
        whiteToolStripMenuItem1.Checked = false;
        blackToolStripMenuItem1.Checked = false;
        blueToolStripMenuItem.Checked = true;
        noneToolStripMenuItem.Checked = false;
        greenToolStripMenuItem.Checked = false;
        brownToolStripMenuItem.Checked = false;
        yellowToolStripMenuItem.Checked = false;
        if (blackToolStripMenuItem.Checked == true)
        {
            label1.ForeColor = System.Drawing.Color.White;
            label2.ForeColor = System.Drawing.Color.White;
            label3.ForeColor = System.Drawing.Color.White;
            label4.ForeColor = System.Drawing.Color.White;
            label6.ForeColor = System.Drawing.Color.White;
            menuStrip1.ForeColor = System.Drawing.Color.White;
        }
        
        }
        

        private void brownToolStripMenuItem_Click(object sender, EventArgs e)
        {
        this.BackgroundImage = global::MyRouter.Properties.Resources.brown;
        Properties.Settings.Default.formbackground = "brown";

        redToolStripMenuItem.Checked = false;
        whiteToolStripMenuItem1.Checked = false;
        blackToolStripMenuItem1.Checked = false;
        blueToolStripMenuItem.Checked = false;
        noneToolStripMenuItem.Checked = false;
        greenToolStripMenuItem.Checked = false;
        brownToolStripMenuItem.Checked = true;
        yellowToolStripMenuItem.Checked = false;
        if (blackToolStripMenuItem.Checked == true)
        {
            label1.ForeColor = System.Drawing.Color.White;
            label2.ForeColor = System.Drawing.Color.White;
            label3.ForeColor = System.Drawing.Color.White;
            label4.ForeColor = System.Drawing.Color.White;
            label6.ForeColor = System.Drawing.Color.White;
            menuStrip1.ForeColor = System.Drawing.Color.White;
        }

        }
        

        private void blackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = global::MyRouter.Properties.Resources.black;
            Properties.Settings.Default.formbackground = "black";

            redToolStripMenuItem.Checked = false;
            whiteToolStripMenuItem1.Checked = false;
            blackToolStripMenuItem1.Checked = true;
            blueToolStripMenuItem.Checked = false;
            noneToolStripMenuItem.Checked = false;
            greenToolStripMenuItem.Checked = false;
            brownToolStripMenuItem.Checked = false;
            yellowToolStripMenuItem.Checked = false;
            if (blackToolStripMenuItem.Checked == true)
            {
                label1.ForeColor = System.Drawing.Color.White;
                label2.ForeColor = System.Drawing.Color.White;
                label3.ForeColor = System.Drawing.Color.White;
                label4.ForeColor = System.Drawing.Color.White;
                label6.ForeColor = System.Drawing.Color.White;
                menuStrip1.ForeColor = System.Drawing.Color.White;
            }

        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start_stop_Click(sender, e);
        }

      
        
        }

        

       

        
       
       

    }

