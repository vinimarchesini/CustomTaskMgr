using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BootTaskmgr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refreshlistbox();
            StartTimer();
        }

        private void initthread()
        {
            System.Threading.Thread rTh = new System.Threading.Thread(new System.Threading.ThreadStart(refreshlistbox));
            rTh.Start();
        }

        private void refreshlistbox()
        {

                if (this.InvokeRequired)

                {

                    this.Invoke(new MethodInvoker(refreshlistbox));

                    return;

                }

                listBox1.Items.Clear();
                Process[] processes = Process.GetProcesses();

                foreach (Process p in processes)
                {
                    if (!String.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        listBox1.Items.Add(p.MainWindowTitle);
                    }
                }
            
        }

        private void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            initthread();
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();


            foreach (Process p in processes)
            {
                try {
                    if (p.MainWindowTitle.Contains(listBox1.SelectedItem.ToString()))
                    {
                        DialogResult dialogResult = MessageBox.Show("Tem certeza que deseja encerrar o processo do " + listBox1.SelectedItem.ToString() + "?", "Atenção!", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            p.Kill();
                            return;
                        }
                        if (dialogResult == DialogResult.No) {
                            return;
                        }
                    }

                } catch (Exception)
                {
                    MessageBox.Show("Não foi possivel localizar o processo. Por favor tente novamente.");
                    return;
                }
            }
        }


    }
}
