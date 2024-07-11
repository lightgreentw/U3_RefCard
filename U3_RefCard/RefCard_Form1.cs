using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace U3_RefCard
{
    public partial class RefCard_Form1 : Form
    {
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
            HORZRES = 8,
            DESKTOPHORZRES = 118
        }

        public string iniFileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".ini");

        private PictureBox pictureBox;
        private Image image;
        private Point mouseDownLocation;
        private int originalWidth, originalHeight;
        private float initialAspectRatio;
        private double originalOpacity;
        private float scalingFactorX;
        private float scalingFactorY;

        public bool resizing = false;
        public bool adjustingOpacity = false;

        public void Save_INI()
        {
            bool success = IniFile.Write(this.Text, "Top", Convert.ToString(this.Top), iniFileName);
            success = success & IniFile.Write(this.Text, "Left", Convert.ToString(this.Left), iniFileName);
            success = success & IniFile.Write(this.Text, "Height", Convert.ToString(this.Height), iniFileName);
            success = success & IniFile.Write(this.Text, "Width", Convert.ToString(this.Width), iniFileName);
            success = success & IniFile.Write(this.Text, "Opacity", Convert.ToString(this.Opacity), iniFileName);
            success = success & IniFile.Write(this.Text, "InvertColor", Convert.ToString(this.pictureBox.Tag), iniFileName);

            if (success == false)
                MessageBox.Show("INI Write failed.");
        }

        private void GetScalingFactors()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);

            // 獲取邏輯 DPI
            int logicalScreenWidth = GetDeviceCaps(hdc, (int)DeviceCap.HORZRES);
            int logicalScreenHeight = GetDeviceCaps(hdc, (int)DeviceCap.VERTRES);

            // 獲取物理 DPI
            int physicalScreenWidth = GetDeviceCaps(hdc, (int)DeviceCap.DESKTOPHORZRES);
            int physicalScreenHeight = GetDeviceCaps(hdc, (int)DeviceCap.DESKTOPVERTRES);

            // 如果物理 DPI 為 0，則使用邏輯 DPI，這適用於較舊版本的 Windows
            if (physicalScreenWidth == 0) 
                physicalScreenWidth = logicalScreenWidth;
            if (physicalScreenHeight == 0) 
                physicalScreenHeight = logicalScreenHeight;

            // 計算縮放比例
            scalingFactorX = (float)physicalScreenWidth / logicalScreenWidth;
            scalingFactorY = (float)physicalScreenHeight / logicalScreenHeight;

            ReleaseDC(IntPtr.Zero, hdc);
        }

        private Image InvertColors(Bitmap original)
        {
            Bitmap inverted = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(255 - originalColor.R, 255 - originalColor.G, 255 - originalColor.B);
                    inverted.SetPixel(x, y, invertedColor);
                }
            }

            return inverted;
        }

        public RefCard_Form1(String sTitle, String sFile, int iTop, int iLeft, int iHeight, int iWidth, Single iOpacity, Boolean bInvertColor)
        {
            //            InitializeComponent();

            // 載入圖片
            image = Image.FromFile(sFile);

            //取得縮放比例
            GetScalingFactors();

            this.Text = sTitle;

            // 設定視窗樣式
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = image.Size;
            this.BackColor = Color.Fuchsia;
            this.TransparencyKey = Color.Fuchsia;
            //this.Size = new Size((int)(image.Width * scalingFactorX), (int)(image.Height * scalingFactorY));

            // 設定原始大小和透明度
            originalWidth = image.Width;
            originalHeight = image.Height;
            initialAspectRatio = (float)image.Width / image.Height;
            originalOpacity = this.Opacity;

            // 創建PictureBox並設定
            pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(pictureBox);

            //載入INI儲存的設定
            if (iTop != 0 && iLeft != 0 && iHeight != 0 && iWidth != 0)
            {
                this.Top = iTop;
                this.Left = iLeft;
                this.Height = iHeight;
                this.Width = iWidth;
                this.Opacity = iOpacity;

                if (bInvertColor == true)
                {
                    this.pictureBox.Tag = Convert.ToString(ChangeColor(false));
                }
                else
                {
                    this.pictureBox.Tag = false;
                }
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            // 事件處理
            this.MouseDown += RefCard_Form1_MouseDown;
            this.MouseMove += RefCard_Form1_MouseMove;
            this.MouseUp += RefCard_Form1_MouseUp;
            this.MouseDoubleClick += RefCard_Form1_MouseDoubleClick;
            this.pictureBox.MouseDown += RefCard_Form1_MouseDown;
            this.pictureBox.MouseMove += RefCard_Form1_MouseMove;
            this.pictureBox.MouseUp += RefCard_Form1_MouseUp;
            this.pictureBox.MouseDoubleClick += RefCard_Form1_MouseDoubleClick;

            // 隨著視窗大小改變圖片大小
            this.Resize += RefCard_Form1_Resize;
        }

        private void RefCard_Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
            {
                resizing = true;
            }
            else if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Alt)
            {
                adjustingOpacity = true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                resizing = false;
                adjustingOpacity = false;
            }

            mouseDownLocation = e.Location;
        }

        private void RefCard_Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
            {
                this.Cursor = Cursors.SizeNWSE;

                // 計算新的大小，保持比例
                int newWidth = this.Location.X + (int)((e.X * scalingFactorX) - mouseDownLocation.X);
                int newHeight = this.Location.Y + (int)((e.Y * scalingFactorY) - mouseDownLocation.Y);

                if (newWidth / initialAspectRatio > newHeight)
                {
                    newHeight = (int)(newWidth / initialAspectRatio);
                }
                else
                {
                    newWidth = (int)(newHeight * initialAspectRatio);
                }

                this.Size = new Size(newWidth, newHeight);
            }
            else if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Alt)
            {
                this.Cursor = Cursors.SizeWE;

                int deltaX = e.X - mouseDownLocation.X;
               double newOpacity = originalOpacity + deltaX / 500.0f;
               newOpacity = Math.Max(0.1f, Math.Min(1.0f, newOpacity));
               this.Opacity = newOpacity;
            }
            else if (e.Button == MouseButtons.Left)
            {
               this.Left = e.X + this.Left - mouseDownLocation.X;
               this.Top = e.Y + this.Top - mouseDownLocation.Y;
            }
            else
            {
               this.Cursor = Cursors.Default;
            }
        }

        private void RefCard_Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
            {
                resizing = false;
            }
            else if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Alt)
            {
                adjustingOpacity = false;
                originalOpacity = this.Opacity;
            }
        }

        private void RefCard_Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ModifierKeys == Keys.Shift)
            {
                this.Size = new Size(originalWidth, originalHeight);
                this.Opacity = 1;
                this.CenterToScreen();
            }
            else if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Alt)
            {
                this.pictureBox.Tag = Convert.ToString(ChangeColor(Convert.ToBoolean(this.pictureBox.Tag)));
            }

        }

        private void RefCard_Form1_Resize(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private void RefCard_Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private Boolean ChangeColor(Boolean bInputValue)
        {
            Image ximage = InvertColors((Bitmap)this.pictureBox.Image);
            this.pictureBox.Image = ximage;

            return bInputValue != true;
        }
    }
}
