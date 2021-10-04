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

namespace ValorantInstaPickHelper_b1
{
    public partial class cnfgr : Form
    {

      public static  RectangleF lockBar;
        private  Dictionary<String, RectangleF> chDic = new Dictionary<string, RectangleF>();
        public static  PointF CP1 = PointF.Empty, CP2 = PointF.Empty;

        public cnfgr()
        {
            InitializeComponent();
        }

        public static PointF[] GetValues()
        {
            PointF[] pf = { CP1, CP2 };
            return pf;
        }

        private void cnfgr_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            if (Form1.isEng == true)
            {
                label1.Text = "Choose a character to lock!";
                label2.Text = "Showing only \n configured characters!";
                label5.Text = "Choose a character to configure";
                button1.Text = "Configure";
                button2.Text = "Configure Lock Button";
            }
            else
            {
                label1.Text = "Kilitlenecek karakteri seçin!";
                label2.Text = "Sadece yapılandırılmış \n karakterler gösterilir!";
                label5.Text = "Yapılandırılacak karakter seçin";
                button1.Text = "Yapılandır";
                button2.Text = "Kilitleme Butonunu Yapılandır";
            }

            if (!File.Exists(Application.StartupPath + "\\" + "lbpos.txt"))
            {
                var j = File.CreateText(Application.StartupPath + "\\" + "lbpos.txt");
                j.Close();
            }

            if (!File.Exists(Application.StartupPath + "\\" + "chpos.txt"))
            {
                var j = File.CreateText(Application.StartupPath + "\\" + "chpos.txt");
                j.Close();
            }

            //Load configured characters

            chDic.Clear();

            try
            {
                float x, y, w, h;
                string rStr = null;
                if (File.Exists(Application.StartupPath + "\\" + "lbpos.txt"))
                {
                    if (!string.IsNullOrEmpty(File.ReadAllText(Application.StartupPath + "\\" + "lbpos.txt")))
                    {
                        rStr = (File.ReadAllLines(Application.StartupPath + "\\" + "lbpos.txt")[0]);
                        char[] splitChar = ":".ToCharArray();
                        x = float.Parse(rStr.Split(splitChar)[0]);
                        y = float.Parse(rStr.Split(splitChar)[1]);
                        w = float.Parse(rStr.Split(splitChar)[2]);
                        h = float.Parse(rStr.Split(splitChar)[3]);

                        lockBar = new RectangleF(x, y, w, h);
                    }

                }

            }
            catch
            {
                MessageBox.Show(Form1.isEng == true ? "Invalid Configuration" : "Hatalı Yapılandırma", Form1.isEng == true ? "Error!" : "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

 

            try
            {
                float x, y, w, h;
                string chname;
                string[] rStr;
                if (File.Exists(Application.StartupPath + "\\" + "chpos.txt"))
                {
                    if (!string.IsNullOrEmpty(File.ReadAllText(Application.StartupPath + "\\" + "chpos.txt")))
                    {
                        rStr = File.ReadAllLines(Application.StartupPath + "\\" + "chpos.txt");
                        for (int i = 0; i <= rStr.Length - 1; i++)
                        {
                            char[] splitChar = ":".ToCharArray();
                            if (rStr[i].Split(splitChar).Length > 0)
                            {
                                chname = rStr[i].Split(splitChar)[0];
                                x = float.Parse(rStr[i].Split(splitChar)[1]);
                                y = float.Parse(rStr[i].Split(splitChar)[2]);
                                w = float.Parse(rStr[i].Split(splitChar)[3]);
                                h = float.Parse(rStr[i].Split(splitChar)[4]);

                                if (comboBox2.Items.Contains(chname))
                                {
                                    chDic.Add(chname, new RectangleF(x, y, w, h));
                                }
                            }
                           
                        }
                    }
                }
            }  catch(Exception exe)  {
                MessageBox.Show(Form1.isEng == true ? "Invalid Configuration (Ch)" : "Hatalı Yapılandırma (Ch)", Form1.isEng == true ? "Error!" : "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = 0; i <= chDic.Count - 1; i++)
            {
                comboBox1.Items.Add(chDic.ElementAt(i).Key);
            }

        }
      
        private void refresh()
        {

        }

            private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                if (MessageBox.Show(Form1.isEng == true ? "To configure the character, please resize and drag the pop-up window to the position of the character selection button 'in-game'." : "Karakterin yapılandırmasını yapmak için lütfen açılan pencereyi yeniden boyutlandırarek ve sürükleyerek 'oyun içerisindeki' karakterin seçme butonunun konumuna getirin.", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ct = SelectionForm.CallType.Charachter;
                    pName = comboBox2.SelectedItem.ToString();
                    SelectionForm sf = new SelectionForm(pName, ct);
                    sf.Show();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            textBox1.Text = "";
            label3.Text = comboBox2.SelectedItem.ToString();
            if (chDic.ContainsKey(comboBox2.SelectedItem.ToString()))
            {
                RectangleF rect;
                chDic.TryGetValue(comboBox2.SelectedItem.ToString(), out rect);
                textBox1.Text = Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height + " --> " + rect.X.ToString() + "," + rect.Y.ToString() + " ==> " + rect.Width.ToString() + "," + rect.Height.ToString() + " ~LB" + lockBar.X.ToString() + "," + lockBar.Y.ToString();

            } 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Form1.isEng == true ? "To configure the lock button, please resize and drag the pop-up window to the position of the lock button 'in-game'.": "Kilitleme butonunun yapılandırmasını yapmak için lütfen açılan pencereyi yeniden boyutlandırarek ve sürükleyerek 'oyun içerisindeki' kilitle butonunun konumuna getirin.", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
               ct = 0;
              pName = Form1.isEng == true ? "Lock Button" : "Kilitleme Butonu";
                SelectionForm sf = new SelectionForm(pName, ct);
                sf.Show();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CP1= new PointF(lockBar.X + lockBar.Width / 2, lockBar.Y + lockBar.Height / 2);
            RectangleF tempRect;
                chDic.TryGetValue(comboBox1.SelectedItem.ToString(), out tempRect);
            CP2 = new PointF(tempRect.X  + tempRect .Width / 2, tempRect.Y + tempRect.Height / 2);
        }

     public static SelectionForm.CallType ct;  
public static string pName;  

    }
}
