using AxWMPLib;
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
using YoutubeExplode;
using YoutubeExplode.Models;

namespace appel
{
    public class fMain : Form, IFORM
    {
        #region [ VARIABLE ] 

        private readonly Font font_Title = new Font("Arial", 11f, FontStyle.Regular);

        private AxWindowsMediaPlayer m_media;
        private const int m_media_width = 215;

        private IconButton btn_exit;
        private IconButton btn_mini;

        private Label lbl_title;

        private FATabStrip m_tab;
        private FATabStripItem m_tab_Search;
        private FATabStripItem m_tab_Tag;
        private FATabStripItem m_tab_Listen;
        private FATabStripItem m_tab_Speaking;
        private FATabStripItem m_tab_Word;
        private FATabStripItem m_tab_Grammar;
        private FATabStripItem m_tab_Text;

        private msg m_search_current_msg = null;
        private TextBox m_search_Input;
        private bool m_search_Online = false;
        private Panel m_search_Result;
        private Label m_search_PageCurrent;
        private Label m_search_PageTotal;
        private Label m_media_Total;
        private Label m_search_Message;
        private IconButton m_search_saveResult;
        private Panel m_search_Header;

        private Label lbl_hide_border_left;
        private Label m_msg_api;

        #endregion

        public fMain()
        {
            this.Icon = Resources.favicon;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Shown += (se, ev) =>
            {
                m_search_Message.Text = string.Empty;
                m_media.Visible = false;
                lbl_title.Width = app.m_app_width * 2;// - (m_media_width + lbl_title.Location.X + 15);

                btn_exit.Location = new Point(3, 0);
                btn_exit.BringToFront();

                btn_mini.Location = new Point(25, 0);
                btn_mini.BringToFront();


                //m_media.uiMode = "mini";
                m_media.Width = m_media_width;
                m_media.Height = 44;
                m_media.Location = new Point(this.Width - (m_media_width - 2), 1);
                m_media.settings.volume = 100;
                m_media.BringToFront();

                //lbl_hide_border_left.BackColor = Color.Orange;
                lbl_hide_border_left.Height = 45;
                lbl_hide_border_left.Width = 42;
                lbl_hide_border_left.Location = new Point((this.Width - (m_media_width - 2)) - 40, -1);
                lbl_hide_border_left.BringToFront();

                m_search_Input.Focus();
                app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH, string.Empty);
            };

            #region [ MEDIA ]
            m_msg_api = new Label()
            {
                Dock = DockStyle.Bottom,
                Text = "English Media",
                TextAlign = ContentAlignment.TopLeft,
                AutoSize = false,
                Height = 15,
                Padding = new Padding(5, 0, 0, 0),
            };

            lbl_hide_border_left = new Label()
            {
                AutoSize = false,
                Width = 3,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            this.Controls.Add(lbl_hide_border_left);
            lbl_hide_border_left.MouseMove += f_form_move_MouseDown;

            // MEDIA
            m_media = new AxWindowsMediaPlayer();
            m_media.Enabled = true;
            m_media.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(this.f_media_event_PlayStateChange);
            m_media.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Controls.Add(m_media);

            #endregion

            #region [ TAB ]

            lbl_title = new Label()
            {
                AutoSize = false,
                Text = "",
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(55, 0),
                //BackColor = Color.DimGray,
            };
            lbl_title.MouseMove += f_form_move_MouseDown;
            this.Controls.Add(lbl_title);

            btn_exit = new IconButton(20) { IconType = IconType.ios_close_empty, Anchor = AnchorStyles.Left | AnchorStyles.Top };
            btn_exit.Click += (se, ev) => { app.Exit(); };
            this.Controls.Add(btn_exit);

            btn_mini = new IconButton(20) { IconType = IconType.ios_minus_empty, Anchor = AnchorStyles.Left | AnchorStyles.Top };
            btn_mini.Click += (se, ev) => { this.WindowState = FormWindowState.Minimized; };
            this.Controls.Add(btn_mini);

            //////////////////////////////////////////////////////////
            // TAB

            m_tab = new FATabStrip()
            {
                Dock = DockStyle.Fill,
                AlwaysShowClose = false,
                AlwaysShowMenuGlyph = false,
                Margin = new Padding(0, 45, 0, 0),
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
            Label lbl_bgHeader = new Label() { Dock = DockStyle.Top, Height = lbl_title.Height };
            m_tab.MouseMove += f_form_move_MouseDown;
            lbl_bgHeader.MouseMove += f_form_move_MouseDown;
            this.Controls.AddRange(new Control[] {
                m_tab,lbl_bgHeader
                ,m_msg_api
            });

            #endregion

            #region [ SEARCH ]

            m_search_Message = new Label()
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                //BackColor = Color.Gray,
                Text = "Message here ...",
                TextAlign = ContentAlignment.MiddleCenter,
            };

            m_search_Result = new Panel()
            {
                AutoScroll = true,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
            };
            m_search_Result.MouseMove += f_form_move_MouseDown;

            m_search_Input = new TextBox()
            {
                Dock = DockStyle.Left,
                Width = 123,
            };
            m_search_Input.KeyDown += f_search_input_KeyDown;

            m_search_Header = new Panel()
            {
                Height = 35,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Padding = new Padding(9, 9, 9, 0),
            };
            var ico_search_Online = new IconButton(20)
            {
                IconType = IconType.android_globe,
                Dock = DockStyle.Left,
                BorderStyle = BorderStyle.None,
            };
            m_search_Header.MouseMove += f_form_move_MouseDown;
            m_tab_Search.Controls.AddRange(new Control[] {
                m_search_Result,
                m_search_Header,
                new Label(){ AutoSize = false, Height = 9, Dock = DockStyle.Top }
            });
            ico_search_Online.Click += (se, ev) =>
            {
                if (m_search_Online)
                {
                    m_search_Online = false;
                    ico_search_Online.InActiveColor = Color.DimGray;
                }
                else
                {
                    m_search_Online = true;
                    ico_search_Online.InActiveColor = Color.DodgerBlue;
                }
            };

            m_search_saveResult = new IconButton(24) { IconType = IconType.ios_cloud_download, Dock = DockStyle.Left };
            IconButton btn_tags = new IconButton(24) { IconType = IconType.pricetags, Dock = DockStyle.Left, ToolTipText = "Tags" };
            IconButton btn_user = new IconButton(22) { IconType = IconType.person, Dock = DockStyle.Left, ToolTipText = "User" };
            IconButton btn_channel = new IconButton(22) { IconType = IconType.android_desktop, Dock = DockStyle.Left, ToolTipText = "Channel" };

            IconButton btn_next = new IconButton(16) { IconType = IconType.ios_arrow_back, Dock = DockStyle.Right };
            IconButton btn_prev = new IconButton(16) { IconType = IconType.ios_arrow_next, Dock = DockStyle.Right };
            IconButton btn_remove = new IconButton(22) { IconType = IconType.trash_a, Dock = DockStyle.Right };
            IconButton btn_add_playlist = new IconButton(22) { IconType = IconType.android_add, Dock = DockStyle.Right, ToolTipText = "Add to Playlist" };

            m_search_PageCurrent = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Gray,
                Text = "1",
                TextAlign = ContentAlignment.BottomRight,
                Dock = DockStyle.Right,
                Padding = new Padding(9, 3, 0, 0)
            };
            m_search_PageTotal = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Yellow,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            m_media_Total = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Blue,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            btn_next.Click += f_search_goPageNextClick;
            btn_prev.Click += f_search_goPagePrevClick;


            m_search_Message.MouseMove += f_form_move_MouseDown;
            m_search_Header.Controls.AddRange(new Control[] {
                #region

                m_search_Message,
                btn_channel,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_user,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_tags,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                m_search_saveResult,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                ico_search_Online,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 3 },
                m_search_Input,

                btn_add_playlist,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                //btn_folder,
                //new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                //btn_tags_filter,
                //new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_remove,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_next,
                m_search_PageCurrent,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "|",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_search_PageTotal,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "_",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_media_Total,
                new Label(){ Dock = DockStyle.Right, Padding = new Padding(0,3,0,0), Text = " items ", TextAlign = ContentAlignment.BottomLeft, AutoSize = true, },
                btn_prev,

                #endregion
            });

            #endregion

        }

        #region [ SEARCH ]

        private void f_search_draw_Media(List<long> ls)
        {
            m_search_Result.crossThreadPerformSafely(() =>
            {
                m_search_Result.Controls.Clear();
            });

            if (ls.Count == 0) return;

            const int margin_bottom = 5;
            const int margin_left = 9;

            int y = 0, x = 0, row = 0;
            Control[] pics = new Control[30];
            Control[] tits = new Control[30];

            #region

            for (int i = 0; i < ls.Count; i++)
            {
                if (i > 29) break;
                if (i == 0 || i == 1)
                {
                    x = i == 0 ? margin_left : (app.m_box_width + margin_left * 2);
                    y = 0;
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
                        x = app.m_box_width + margin_left * 2;
                        y = (app.m_item_height * row) + margin_bottom * row;
                    }
                }

                oMedia media = api_media.f_media_getInfo(ls[i]);
                if (media == null) continue;

                PictureBox pic = new PictureBox()
                {
                    //Text = i.ToString(),
                    //TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.WhiteSmoke,
                    Width = app.m_item_width,
                    Height = app.m_item_height,
                    Location = new Point(x, y),
                    Tag = media.Id + "¦" + media.Title,
                };
                string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photo");
                file = Path.Combine(file, media.Id.ToString() + ".jpg");
                if (File.Exists(file))
                {
                    var fs = File.OpenRead(file);
                    pic.Image = new Bitmap(fs);
                }

                Label lbl = new Label()
                {
                    Text = (i + 1).ToString() + ", " + media.Title,
                    TextAlign = ContentAlignment.MiddleLeft,

                    AutoSize = false,
                    BackColor = Color.LightGray,
                    //ForeColor = Color.Black,
                    Width = app.m_box_width - app.m_item_width,
                    Height = app.m_box_height - app.m_item_height,
                    Location = new Point(pic.Location.X + app.m_item_width, pic.Location.Y),
                    Padding = new Padding(9, 0, 0, 0),
                    Font = font_Title,
                    Tag = media.Id + "¦" + media.Title,
                };

                pic.Click += (se, ev) =>
                {
                    ((Control)se).BackColor = Color.Gray;
                    string[] a = ((Control)se).Tag.ToString().Split('¦');
                    f_video_openMp4_Request(long.Parse(a[0]), a[1]);
                };
                lbl.Click += (se, ev) =>
                {
                    string[] a = ((Control)se).Tag.ToString().Split('¦');
                    f_video_openMp3_Request(long.Parse(a[0]), a[1]);
                };
                lbl.MouseMove += f_form_move_MouseDown;
                pic.MouseMove += f_form_move_MouseDown;

                pics[i] = pic;
                tits[i] = lbl;
            }

            #endregion

            m_search_Result.crossThreadPerformSafely(() =>
            {
                m_search_Result.Controls.AddRange(tits);
                m_search_Result.Controls.AddRange(pics);
            });
        }

        private void f_search_goPagePrevClick(object sender, EventArgs e)
        {
            if (m_search_current_msg != null)
            {
                if ((m_search_current_msg.PageNumber - 1) * m_search_current_msg.PageSize < m_search_current_msg.Counter)
                {
                    m_search_current_msg.PageNumber = m_search_current_msg.PageNumber + 1;
                    app.postToAPI(m_search_current_msg);
                }
            }
        }

        private void f_search_goPageNextClick(object sender, EventArgs e)
        {
            if (m_search_current_msg != null)
            {
                if (m_search_current_msg.PageNumber > 1)
                {
                    m_search_current_msg.PageNumber = m_search_current_msg.PageNumber - 1;
                    app.postToAPI(m_search_current_msg);
                }
            }
        }

        private void f_search_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string key = m_search_Input.Text.Trim();
                if (key.Length > 1)
                {
                    m_search_Message.Text = "Finding [" + key + "] ...";
                    app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH, key);

                    //if (m_search_Online)
                    //{
                    //    Cursor = Cursors.WaitCursor;
                    //    m_search_Message.Text = "SEARCH: " + key + "...";
                    //    var _client = new YoutubeClient();
                    //    List<Video> rs = _client.SearchVideosAsync(key);
                    //    //f_search_draw_Media(rs);
                    //    m_search_Message.Text = "SEARCH: " + key + " found " + rs.Count + " videos online.";
                    //    Cursor = Cursors.Default;
                    //}
                    //else
                    //{
                    //    m_search_Message.Text = "Search local [" + key + "] ...";
                    //    app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH, key);
                    //}
                }
                else
                    m_search_Message.Text = "Length of keywords must be greater than 1 characters.";
            }
        }

        void f_search_Result(oMediaSearchLocalResult rs)
        {
            f_search_draw_Media(rs.MediaIds);

            int page = rs.CountResult / rs.PageSize;
            if (rs.CountResult % rs.PageSize != 0) page++;

            m_search_PageCurrent.crossThreadPerformSafely(() =>
            {
                m_search_PageCurrent.Text = rs.PageNumber.ToString();
            });
            m_search_PageTotal.crossThreadPerformSafely(() =>
            {
                m_search_PageTotal.Text = page.ToString();
            });
            m_media_Total.crossThreadPerformSafely(() =>
            {
                m_media_Total.Text = rs.CountResult.ToString();
            });
        }

        #endregion

        #region [ MEDIA ]

        private void f_media_event_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
        }

        void f_video_openMp4_Request(long videoId, string title)
        {
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_PLAY_VIDEO, Log = title, Input = videoId });
        }

        void f_video_openMp4_Callback(string url, string title)
        {
            //string title = string.Empty;
            //string url = api_media.f_get_uriProxy(videoId, MEDIA_TYPE.MP4);

            if (url != string.Empty)
            {
                this.Invoke((Action)(() =>
                {
                    this.Text = title;
                    this.Cursor = Cursors.WaitCursor;
                    var f = new fPlayer(url, title);
                    f.Show();
                    f.Left = this.Location.X + 9;
                    f.Top = this.Location.Y + 99;
                    this.Cursor = Cursors.Default;
                }));
            }
            else
                MessageBox.Show("Cannot open videoId: " + title);
        }

        void f_video_openMp3_Request(long videoId, string title)
        {
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_PLAY_AUDIO, Log = title, Input = videoId });
        }

        void f_video_openMp3_Callback(string url, string title)
        {
            //string title = string.Empty;
            //string url = api_media.f_get_uriProxy(videoId, MEDIA_TYPE.M4A);
            if (url != string.Empty)
            {


                this.Invoke((Action)(() =>
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.Text = title;
                    m_media.Visible = true;

                    for (int i = 0; i < m_search_Result.Controls.Count; i++)
                    {
                        if (m_search_Result.Controls[i] is Label)
                            m_search_Result.Controls[i].BackColor = Color.LightGray;
                    }

                    string tit = title;
                    if (title.Length > 69) tit = title.Substring(0, 65) + "...";
                    lbl_title.Text = title;

                    m_media.URL = url;
                    this.Cursor = Cursors.Default;
                }));

            }
            else
                MessageBox.Show("Cannot open videoId: " + title);

        }

        #endregion

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

        #region [ RESPONSE MESSAGE ]

        public void api_responseMsg(object sender, threadMsgEventArgs e)
        {
            msg m = e.Message;
            if (m != null)
            {
                switch (m.API)
                {
                    case _API.MSG_ANALYTIC_WORD:
                        break;
                    case _API.MSG_ANALYTIC_CONTENT:
                        m_msg_api.crossThreadPerformSafely(() =>
                        {
                            m_msg_api.Text = m.Log;
                        });
                        break;
                    case _API.MEDIA:
                        #region
                        switch (m.KEY)
                        {
                            case _API.MEDIA_KEY_SEARCH:
                                if (m.Output.Ok)
                                {
                                    var rs = (oMediaSearchLocalResult)m.Output.Data;
                                    f_search_Result(rs);
                                    m_search_current_msg = m.clone();
                                }
                                else
                                    m_msg_api.Text = "Search error";
                                break;
                            case _API.MEDIA_KEY_PLAY_AUDIO:
                                if (m.Output.Ok)
                                    f_video_openMp3_Callback((string)m.Output.Data, m.Log);
                                break;
                            case _API.MEDIA_KEY_PLAY_VIDEO:
                                if (m.Output.Ok)
                                    f_video_openMp4_Callback((string)m.Output.Data, m.Log);
                                break;
                        }
                        break;
                        #endregion
                }
            }
        }

        public void f_form_freeResource()
        {
        }

        #endregion
    }
}
