using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace U3_RefCard
{
    public partial class MainControl : Form
    {
        private static AboutForm pfAboutForm;

        public string iniFileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".ini");
        public string ApplicationPathWithFileName = Application.ExecutablePath;

        public static RefCard_Form1 pRefCard1;
        public static RefCard_Form1 pRefCard2;

        public RefCard_Form1[] _RefCard_Forms = new RefCard_Form1[0];
        private String[][] _INIList = new String[0][];

        private Int32 Load_RefCardIndex(String sIniFile)
        {
            return Convert.ToInt32(IniFile.Read("RefCardIndex", "Count", "1", sIniFile));
        }

        private bool Load_INI()
        {
            if (File.Exists(iniFileName))
            {
                String sIndexTitle;
                Int32 iCount = Load_RefCardIndex(iniFileName);

                //重置視窗陣列及INI陣列
                Array.Clear(_RefCard_Forms, 0, _RefCard_Forms.Length);
                Array.Clear(_INIList, 0, _INIList.Length);
                Array.Resize(ref _RefCard_Forms, 0);
                Array.Resize(ref _INIList, 0);

                Array.Resize(ref _RefCard_Forms, iCount);
                Array.Resize(ref _INIList, iCount);

                cms_Menu.Items.Clear();

                //存檔內容由1起算
                for (int i = 0; i < iCount; i++)
                {
                    sIndexTitle = "RefCard" + Convert.ToString(i + 1);
                    _INIList[i] = new string[9];
                    _INIList[i][0] = IniFile.Read(sIndexTitle, "Title", "", iniFileName);
                    _INIList[i][1] = IniFile.Read(sIndexTitle, "RefFile", "", iniFileName);
                    _INIList[i][2] = IniFile.Read(sIndexTitle, "Top", "0", iniFileName);
                    _INIList[i][3] = IniFile.Read(sIndexTitle, "Left", "0", iniFileName);
                    _INIList[i][4] = IniFile.Read(sIndexTitle, "Height", "0", iniFileName);
                    _INIList[i][5] = IniFile.Read(sIndexTitle, "Width", "0", iniFileName);
                    _INIList[i][6] = IniFile.Read(sIndexTitle, "Opacity", "1", iniFileName);
                    _INIList[i][7] = IniFile.Read(sIndexTitle, "InvertColor", "False", iniFileName);
                    _INIList[i][8] = IniFile.Read(sIndexTitle, "DefaultOn", "False", iniFileName);

                    _INIList[i][7] = Convert.ToString(_INIList[i][7].Equals("true", StringComparison.OrdinalIgnoreCase));
                    _INIList[i][8] = Convert.ToString(_INIList[i][8].Equals("true", StringComparison.OrdinalIgnoreCase));

                    this._RefCard_Forms[i] = new RefCard_Form1(sIndexTitle, 
                                                                _INIList[i][1],
                                                                Convert.ToInt32(_INIList[i][2]),
                                                                Convert.ToInt32(_INIList[i][3]),
                                                                Convert.ToInt32(_INIList[i][4]),
                                                                Convert.ToInt32(_INIList[i][5]),
                                                                Convert.ToSingle(_INIList[i][6]),
                                                                Convert.ToBoolean(_INIList[i][7]));

                    ToolStripMenuItem xMenuItem = new System.Windows.Forms.ToolStripMenuItem(); // 滑鼠右鍵選單選項
                    xMenuItem.Text = _INIList[i][0];
                    xMenuItem.Tag = i;
                    xMenuItem.Click += referenceCardToolStripMenuItem_Click;
                    cms_Menu.Items.Add(xMenuItem); 
                }

                ToolStripSeparator xSeparator = new System.Windows.Forms.ToolStripSeparator(); // 右鍵選單選分隔線
                cms_Menu.Items.Add(xSeparator);

                ToolStripMenuItem xAboutMenuItem = new System.Windows.Forms.ToolStripMenuItem(); // 右鍵選單選關於程式選項
                xAboutMenuItem.Text = "About";
                xAboutMenuItem.Click += aboutToolStripMenuItem_Click;
                cms_Menu.Items.Add(xAboutMenuItem);

                ToolStripMenuItem xExitMenuItem = new System.Windows.Forms.ToolStripMenuItem(); // 右鍵選單選關閉程式選項
                xExitMenuItem.Text = "Exit";
                xExitMenuItem.Click += exitToolStripMenuItem_Click;
                cms_Menu.Items.Add(xExitMenuItem);

                return true;
            }
            else
                return false;  //

        }

        public MainControl()
        {
            InitializeComponent();

            Load_INI();

            //預設開啟
            for (int i = 0; i < _INIList.Length; i++)
            {
                if (Convert.ToBoolean(_INIList[i][8]) == true) 
                    cms_Menu.Items[i].PerformClick();
            }
        }

        private void referenceCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem xMenuItem = (ToolStripMenuItem)sender;
            int iFormIndex = Convert.ToInt32(xMenuItem.Tag);

            if (_RefCard_Forms[iFormIndex] != null)
            {
                if (xMenuItem.Checked == false )
                {
                    _RefCard_Forms[iFormIndex].Show();
                    xMenuItem.Checked = true;
                }
                else
                {
                    _RefCard_Forms[iFormIndex].Hide(); // .Close();
                    xMenuItem.Checked = false;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i =0;i < _RefCard_Forms.Length; i++)
            {
                _RefCard_Forms[i].Save_INI();
            }

            this.Close();
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pfAboutForm == null)
            {
                pfAboutForm = new AboutForm();
                pfAboutForm.TopMost = true;
                pfAboutForm.ShowDialog();

                pfAboutForm = null;
            }
            else
                pfAboutForm.BringToFront();

        }
    }
}
