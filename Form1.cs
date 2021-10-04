using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SelectionRectangle;
using GlobalLowLevelHooks;
using System.Runtime.InteropServices;

namespace ValorantInstaPickHelper_b1
{
    public partial class Form1 : Form
    {
        public Dictionary<String, Control> langD = new Dictionary<String, Control>(); // I dont have time to make a language system using Dictionary so maybe another time :)
       public static Boolean isEng = true; 
        private string hotkey = null;
        KeyboardHook keyboardHook = new KeyboardHook();
        KeyboardHook kH1 = new KeyboardHook();
        KeyboardHook kH2 = new KeyboardHook();

        int msint = 200;

        public PointF CP1 = PointF.Empty, CP2 = PointF.Empty;

        private int sc = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //SelectionForm sf = new SelectionForm();
            //sf.Show();
            //sf.reCallType = SelectionForm.CallType.LockBar;
            //sf.reName = Form1.isEng == true ? "Lock Button" : "Kilitleme Butonu";

            comboBox1.SelectedIndex = 0;

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;


        }

        void ChangeLang()
        {
if (isEng != true)
            {
                //TR
                groupBox1.Text = "Dil";
                groupBox2.Text = "Kısayollar";
                label1.Text = "Aç / Kapat ;";
                label3.Text = "(Kaydet)";
            }
            else
            {
                //ENG
                groupBox1.Text = "Language";
                groupBox2.Text = "Hotkeys";
                label1.Text = "Toggle On / Off ;";
                label3.Text = "(Record)";
            }

        }
       
       public static void DrawSelectionRectangle(Graphics source, RectangleF rectangle, Pen pen, string text = null, Font textFont = null, Brush textBrush = null)
        {
            PointF Point = new PointF(rectangle.X, rectangle.Y);
            Size size = new Size((int)rectangle.Width, (int)rectangle.Height);
            Graphics e = source;
            PointF up_left = new PointF(Point.X, Point.Y);
            PointF up_right = new PointF(size.Width + Point.X, Point.Y);
            PointF down_right = new PointF(size.Width + Point.X, size.Height + Point.Y);
            PointF down_left = new PointF(Point.X, size.Height + Point.Y);

            Single xL = size.Width / 3;
            Single yL = size.Height / 3;

            Pen p = pen;

            List<PointF[]> PFaList = new List<PointF[]>();

            PointF[] pA1 = { new PointF(up_left.X, up_left.Y + yL), new PointF(up_left.X, up_left.Y), new PointF(up_left.X + xL, up_left.Y) };
            PointF[] pA2 = { new PointF(up_right.X + xL, up_right.Y + yL), new PointF(up_right.X + xL, up_right.Y), new PointF(up_right.X, up_right.Y) };

            PointF[] pA3 = { new PointF(down_right.X, down_right.Y + yL), new PointF(down_right.X + xL, down_right.Y + yL), new PointF(down_right.X + xL, down_right.Y) };
            PointF[] pA4 = { new PointF(down_left.X, down_left.Y), new PointF(down_left.X, down_left.Y + yL), new PointF(down_left.X + xL, down_left.Y + yL) };


            PFaList.Add(pA1);
            PFaList.Add(pA2);
            PFaList.Add(pA3);
            PFaList.Add(pA4);

            for (int i = 0; i <= PFaList.Count - 1; i++)
            {
                e.DrawLines(p, PFaList[i]);
            }

            if (!string.IsNullOrEmpty(text) && textFont != null)
            {
                SizeF StringSize = (SizeF)(e.MeasureString(text, textFont).ToSize());
                PointF pnt = new PointF(up_left.X + ((up_right.X - up_left.X) / 2), up_left.Y + 4);
                float xVal = StringSize.Width <= 30 ? pnt.X - 5 : pnt.X - StringSize.Width / 3.3F;
                e.DrawString(text, textFont, textBrush == null ? pen.Brush : textBrush, new PointF(xVal, pnt.Y - StringSize.Height - (float)Math.PI * 3));
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //  DrawSelectionRectangle(e.Graphics, new RectangleF(50, 50, 50, 50), new Pen(Color.Black, 2.0f), "KAY/O", new Font("Calibri", 100, FontStyle.Bold), new SolidBrush(Color.Red));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int lang = comboBox1.SelectedText.StartsWith("[TR]") == true ? 0 : 1; // TR=0, EN=1
            //for (int i = 0; i <= langD.Count - 1; i++)
            //{

            //}  

            if (comboBox1.SelectedIndex == 0)
            {
                isEng = true;
            }
            else
            {
                isEng = false;
            }
            ChangeLang();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           msint += msint < 2000 ? 10 : 0;
            string inter = msint.ToString();
            label4.Text = inter + " MS";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            msint -= msint > 10 ? 10 : 0;
            string inter = msint.ToString();
            label4.Text = inter + " MS";
        }

      

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Start();
            kH2.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook2_KeyDown);
            kH2.Install();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            kH2.KeyDown -= new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            kH2.Uninstall();
        }

        private void keyboardHook1_KeyDown(KeyboardHook.VKeys key)
        {
            if (key.ToString() == "END")
            {
                timer1.Enabled = false;
            }
            if (hotkey != null && key.ToString() == hotkey)
            {
                timer1.Enabled = timer1.Enabled == true ? false : true;
                if (timer1.Enabled == true)
                {
                    kH2.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook2_KeyDown);
                    kH2.Install();
                } else
                {
                    kH2.KeyDown -= new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
                    kH2.Uninstall();
                }
                         
            }

        }
         
    private void keyboardHook2_KeyDown(KeyboardHook.VKeys key)
    {
            if (key.ToString() == "END")
            {
                timer1.Enabled = false;
            }
        }
        private void keyboardHook_KeyDown(KeyboardHook.VKeys key)
        {
         
                if (key.ToString() != "Escape")
                {
                    hotkey = key.ToString();
                    label2.Text = key.ToString();

                kH1.KeyDown -= new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
                kH1.Uninstall();

                kH1.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook1_KeyDown);
                    kH1.Install();
                }

            keyboardHook.KeyDown -= new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            keyboardHook.Uninstall();
        }

        private void label3_Click(object sender, EventArgs e)
        {    
            keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            keyboardHook.Install();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CP1 = cnfgr.GetValues()[0];
            CP2 = cnfgr.GetValues()[1];
            //if (CP1 != PointF.Empty && CP2 != PointF.Empty)
            //{
            if (sc == 0)
                {
                   
                    Cursor.Position = new Point((int)CP1.X, (int)CP1.Y);
                System.Threading.Thread.Sleep(msint);
  lclick();
                sc = 1;
            }
            else
            {
                Cursor.Position = new Point((int)CP2.X, (int)CP2.Y);
                System.Threading.Thread.Sleep(msint);
                lclick();
                sc = 0;
                }
            //}
                   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form frm = new cnfgr();
            frm.Show();
        }



        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static void lclick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);  
        }

        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;

        private void button7_Click(object sender, EventArgs e)
        {
            timer1.Interval += timer1.Interval < 2000 ? 10 : 0;
            string inter = timer1.Interval.ToString();
            label5.Text = inter + " MS";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Interval -= timer1.Interval > 10 ? 10 : 0;
            string inter = timer1.Interval.ToString();
            label5.Text = inter + " MS";
        }

        private const int MOUSEEVENTF_RIGHTUP = 0x10;
    }
}
