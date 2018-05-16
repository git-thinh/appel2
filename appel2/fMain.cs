using FarsiLibrary.Win;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace appel
{
    public class fMain : Form, IFORM
    {
        #region [ VARIABLE ]

        private IconButton btn_exit;

        private FATabStrip m_tab;
        private FATabStripItem m_tab_Search;
        private FATabStripItem m_tab_Tag;
        private FATabStripItem m_tab_Listen;
        private FATabStripItem m_tab_Speaking;
        private FATabStripItem m_tab_Word;
        private FATabStripItem m_tab_Grammar;
        private FATabStripItem m_tab_Text;

        private TextBox txt_Search;
        private Panel m_search_Result;

        #endregion

        public fMain() {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Shown += (se, ev) =>
            {
                this.Width = 550;
                this.Height = 600;

                btn_exit.Location = new Point(this.Width - 18, 0);
                btn_exit.BringToFront();
                btn_exit.Height = 20;
            };
            
            #region [ TAB ]

            btn_exit = new IconButton(20) { IconType = IconType.close_circled };
            btn_exit.Click += (se, ev) => { app.Exit(); };
            this.Controls.Add(btn_exit);

            m_tab = new FATabStrip() {
                Dock = DockStyle.Fill,
                AlwaysShowClose = false,
                AlwaysShowMenuGlyph = false,
            };
            m_tab_Search = new FATabStripItem()
            {
                CanClose = false,
                Title = "Search",
            };
            m_tab_Listen = new FATabStripItem()
            {
                CanClose = false,
                Title = "Listen",
            };
            m_tab_Tag = new FATabStripItem()
            {
                CanClose = false,
                Title = "Tag",
            };
            m_tab_Grammar = new FATabStripItem()
            {
                CanClose = false,
                Title = "Grammar",
            };
            m_tab_Word = new FATabStripItem()
            {
                CanClose = false,
                Title = "Word",
            };
            m_tab_Speaking = new FATabStripItem()
            {
                CanClose = false,
                Title = "Speak",
            };
            m_tab_Text = new FATabStripItem()
            {
                CanClose = false,
                Title = "Text",
            };

            m_tab.Items.AddRange(new FATabStripItem[] {
                m_tab_Search,
                m_tab_Tag,
                m_tab_Listen,
                m_tab_Speaking,
                m_tab_Grammar,
                m_tab_Word,
                m_tab_Text,
            });
            m_tab.MouseMove += f_form_move_MouseDown;

            this.Controls.Add(m_tab);

            #endregion

            #region [ SEARCH ]

            m_search_Result = new Panel() {
                AutoScroll = true,
                BackColor = Color.Blue,
                Dock = DockStyle.Fill,
            };

            txt_Search = new TextBox() {
                Dock = DockStyle.Right,
            };

            var header = new Panel() {
                Height = 39,
                Dock = DockStyle.Top,
                BackColor = Color.Orange,
                Padding = new Padding(9),
            };

            header.Controls.Add(txt_Search);
            m_tab_Search.Controls.AddRange(new Control[] {
                header,
                m_search_Result,
            });

            #endregion
        }

        #region [ FORM MOVE ]

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void f_form_move_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion


        public void api_initMsg(msg m)
        {
        }

        public void api_responseMsg(object sender, threadMsgEventArgs e)
        {
        }
    }
}
