using Gma.System.MouseKeyHook;
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
using System.Threading.Tasks;

namespace ValorantInstaPickHelper_b1
{

    public partial class SelectionForm : Form
    {
    

        private  CallType ctype;
        private  bool isWorking = false;
        private  string chName = null;


        public SelectionForm(string name, CallType ctype1)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            ctype = ctype1;
            chName = name;
        }


        public enum CallType : int
        {
            LockBar = 0,
            Charachter = 1
        }

        private RectangleF rectDraw;

  
        

        private void SelectionForm_Move(object sender, EventArgs e)
        {
            this.Text = chName + "  (X:" + this.Location.X + " Y:" + this.Location.Y + ")";
        }

        //private void SelectionForm_ReSize(object sender, EventArgs e)
        //{
        //    rectDraw = new RectangleF(5, 5, this.Size.Width - 70,this.Size.Height - 90);
        //    this.panel1.Paint += new PaintEventHandler(this.Panel1_Draw);
        //    this.Refresh();
        //}

        //private void Panel1_Draw(object sender, PaintEventArgs e)
        //{
        //    Form1.DrawSelectionRectangle(e.Graphics, rectDraw, new Pen(Color.FromArgb(192, 0, 0), 3.5F));
        //}

        private void SelectionForm_Load(object sender, EventArgs e)
        {
            if (ctype != null && chName != null)
            {
                isWorking = true;
            this.Text = chName;
            }
            this.ShowInTaskbar = true;

        }

        private void SelectionForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Return")
            {
                if (isWorking == true & ctype == CallType.Charachter || ctype == CallType.LockBar)
                    SaveValues(ctype, new RectangleF(this.Location, this.Size), chName);
            }
        }

        private void SaveValues(CallType ct, RectangleF rect, string name = null)
        {
        var msg = MessageBox.Show(Form1.isEng == true ? "Are you sure you want to overwrite/add this value to config?" : "Bu veriyi yapılandırma dosyasının üstüne yazmak/eklemek istiyor musunuz?", "????", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msg == DialogResult.Yes)
            {
                if (ct == 0)
                {
                    if (File.Exists(Application.StartupPath + "\\" + "lbpos.txt"))
                    {
                        string s = rect.X + ":" + rect.Y + ":" + rect.Width + ":" + rect.Height;
                        File.WriteAllText(Application.StartupPath + "\\" + "lbpos.txt", s);
                        this.Close();
                    }
                   
                } else
                {
                    if (File.Exists(Application.StartupPath + "\\" + "chpos.txt"))
                    {
                        string[] currentLines = File.ReadAllLines(Application.StartupPath + "\\" + "chpos.txt");
                        List<string> sblist = new List<string>();
  
                        for (int i = 0; i <= currentLines.Length - 1; i++)
                        {
                            if (currentLines[i].Split(":".ToCharArray())[0] != name)
                            {
                              sblist.Add( currentLines[i]);
                            }
                        }

                        string sep = ":";
                       sblist.Add( name + sep + rect.X + sep + rect.Y + sep + rect.Width + sep + rect.Height);
                        string str = "";
                        string[] path = { "chpos.txt" };
                        for (int i = 0; i <= sblist.Count - 1; i++) 
                        {
                            if (str.Length > 0 )
                            {
                                str += "\n" + sblist[i];
                            } else
                            {
                                str += sblist[i];
                            }                   
                        }
                        SV2(path[0], str);
                    }
                }
            }
        }


        private void SV2(string path, string sb)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                File.WriteAllText(path, sb.ToString());
                this.Close();
                Form frm = new cnfgr();
                frm.Close();
            }
          
        }
    }
}
