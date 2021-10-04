using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValorantInstaPickHelper_b1
{
    public partial class ShowPointsInSelRec : Form
    {
        Dictionary<string, RectangleF> rectArray_ = new Dictionary<string, RectangleF>();
        public ShowPointsInSelRec(Dictionary<string, RectangleF> rectArray)
        {
            InitializeComponent();
            rectArray_ = rectArray;
        }

        private void ShowPointsInSelRec_Load(object sender, EventArgs e)
        {
this.Paint += new PaintEventHandler(this.ShowPointsInSelRec_Draw);
        }

        private void ShowPointsInSelRec_Draw(object sender, PaintEventArgs e)
        {
            for (int i = 0; i <= rectArray_.Count - 1; i++)
            {
                Form1.DrawSelectionRectangle(e.Graphics, rectArray_., new Pen())
            }
        }
    }
}
