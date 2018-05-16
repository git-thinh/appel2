using FarsiLibrary.Win;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using YoutubeExplode.Models;

namespace appel
{
    public class fMain : Form, IFORM
    {
        #region [ VARIABLE ] 
        private readonly Font font_Title = new Font("Arial", 11f, FontStyle.Regular);

        private IconButton btn_exit;
        private Label lbl_title;

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

        private Panel m_search_Header;

        #endregion

        public fMain()
        {
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

            btn_exit = new IconButton(20) { IconType = IconType.close_circled, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            btn_exit.Click += (se, ev) => { app.Exit(); };
            this.Controls.Add(btn_exit);

            m_tab = new FATabStrip()
            {
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

            lbl_title = new Label()
            {
                AutoSize = false,
                Text = "English Media",
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(9, 0, 0, 0),
            };
            lbl_title.MouseMove += f_form_move_MouseDown;

            this.Controls.AddRange(new Control[] { m_tab, lbl_title });

            #endregion

            #region [ SEARCH ]

            m_search_Result = new Panel()
            {
                AutoScroll = true,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
            };
            m_search_Result.MouseMove += f_form_move_MouseDown;

            txt_Search = new TextBox()
            {
                Dock = DockStyle.Right,
            };

            m_search_Header = new Panel()
            {
                Height = 39,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(9),
            };
            m_search_Header.MouseMove += f_form_move_MouseDown;
            m_search_Header.Controls.Add(txt_Search);
            m_tab_Search.Controls.AddRange(new Control[] {
                m_search_Result,
                m_search_Header,
            });

            #endregion

            f_search_Result();
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

        #region [ SEARCH ]

        void f_search_Result()
        {
            using (var file = File.OpenRead("videos.bin"))
            {
                const int margin_bottom = 9;
                const int margin_left = 9;
                const int tit_height = 45;
                int distance_tit = app.m_item_height - tit_height;

                var ls = Serializer.Deserialize<List<Video>>(file);

                int y = 0, y2 = 0, x = 0, row = 0;
                string tit = string.Empty;
                Control[] pics = new Control[30];
                Control[] tits = new Control[30];

                #region

                for (int i = 0; i < ls.Count; i++)
                {
                    if (i > 29) break;
                    if (i == 0 || i == 1)
                    {
                        x = i == 0 ? margin_left : (app.m_item_width + margin_left * 2);
                        y = 0;
                        y2 = distance_tit;
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            row = i / 2;
                            x = margin_left;
                            y = (app.m_item_height * row) + margin_bottom * row;
                        }
                        else
                        {
                            row = (int)(i / 2);
                            x = app.m_item_width + margin_left * 2;
                            y = (app.m_item_height * row) + margin_bottom * row;
                        }
                        y2 = y + distance_tit;
                    }

                    PictureBox pic = new PictureBox()
                    {
                        //Text = i.ToString(),
                        //TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.LightGray,
                        Width = app.m_item_width,
                        Height = app.m_item_height,
                        Location = new Point(x, y),
                    };

                    tit = ls[i].Title.ToLower();
                    if (tit.Length > 78) tit = tit.Substring(0, 75) + "...";

                    Label lbl = new Label()
                    {
                        Text = (i + 1).ToString() + ", " + tit,
                        TextAlign = ContentAlignment.MiddleLeft,

                        AutoSize = false,
                        BackColor = Color.Black,
                        ForeColor = Color.White,
                        Width = app.m_item_width,
                        Height = tit_height,
                        Location = new Point(x, y2),
                        Padding = new Padding(9, 0, 0, 0),
                        Font = font_Title,
                    };

                    pic.Click += (se, ev) => {
                        ((Control)se).BackColor = Color.Gray;
                        f_video_openMp4(ls[i].Id, ls[i].Title);
                    };
                    lbl.Click += (se, ev) => {
                        f_video_openMp3(ls[i].Id, ls[i].Title);
                        ((Control)se).BackColor = Color.Orange;
                    };

                    pics[i] = pic;
                    tits[i] = lbl;
                }

                #endregion

                m_search_Result.Controls.AddRange(tits);
                m_search_Result.Controls.AddRange(pics);
            }
        }

        void f_video_openMp4(string videoId, string title)
        {
            app.f_youtube_Open(videoId, title);
        }

        void f_video_openMp3(string videoId, string title)
        {
            for (int i = 0; i < m_search_Result.Controls.Count; i++) {
                if (m_search_Result.Controls[i] is Label)
                    m_search_Result.Controls[i].BackColor = Color.Black;
            }
            lbl_title.Text = title;
        }

        #endregion

        #region [ RESPONSE MESSAGE ]

        public void api_initMsg(msg m)
        {
        }

        public void api_responseMsg(object sender, threadMsgEventArgs e)
        {
        }

        #endregion
    }
}
