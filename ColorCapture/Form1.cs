using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ColorCapture
{
    public partial class MainForm : Form
    {
        private readonly List<Form> _forms = new List<Form>();
        private Color _color;
        private const string HEX = "HEX :";
        private const string RGB = "RGB :";
        public MainForm()
        {
            InitializeComponent();
            InitBackgroundForm();
        }

        private void InitBackgroundForm()
        {
            foreach (Screen s in Screen.AllScreens)
            {
                Rectangle r = s.Bounds;
                Form form = new Form
                {
                    Top = r.Top,
                    Left = r.Left,
                    Width = r.Width,
                    Height = r.Height,
                    TopMost = true,
                    BackColor = Color.Azure,
                    FormBorderStyle = FormBorderStyle.None,
                    Opacity = 0.3d,
                    StartPosition = FormStartPosition.Manual
                };
                form.MouseClick += (sender, args) =>
                {
                    _forms.ForEach(f => f.Hide());
                    using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
                    {
                        Point point = new Point(args.X, args.Y);
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(form.Left, form.Top, 0, 0, bitmap.Size);
                        }
                        _color = bitmap.GetPixel(args.X, args.Y);
                        txtColor.Text = ColorToString(_color);
                    }
                };
                _forms.Add(form);
            }
        }
        private void btnPicker_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog.ShowDialog(this);
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                _color = colorDialog.Color;
                txtColor.Text = ColorToString(_color);
            }
        }

        private string ColorToString(Color c)
        {
            return btnFormat.Text == HEX ? $"#{c.R:X2}{c.G:X2}{c.B:X2}" : $"RGB({c.R},{c.G},{c.B})";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtColor.Text);
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            _forms.ForEach(f => f.Show());
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            btnFormat.Text = btnFormat.Text == HEX ? RGB : HEX;
            txtColor.Text = ColorToString(_color);
        }
    }
}
