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

        private IconButton btn_exit;
        private IconButton btn_mini;

        private Label lbl_title;


        private TextBox m_media_text;
        private long m_media_current_id = 0;
        private string m_media_current_title = string.Empty;

        private Label m_msg_api;

        private fPlayer m_player;

        private StringBuilder log;

        #endregion

        void f_main_Shown()
        {
            this.Text = "English";
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
            lbl_hide_border_left.Width = 12;
            lbl_hide_border_left.Location = new Point((this.Width - (m_media_width - 2)) - 10, -1);
            lbl_hide_border_left.BringToFront();

            btn_play.Location = new Point(this.Width - m_media_width, 3);
            btn_play.BringToFront();

            m_search_Input.Focus();
            app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH_STORE, string.Empty);
        }

        #region [ AUDIO ]

        IconButton btn_play;
        private AxWindowsMediaPlayer m_media;
        private const int m_media_width = 215;
        private Label lbl_hide_border_left;

        void f_audio_initUI()
        {
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

            btn_play = new IconButton(24)
            {
                IconType = IconType.ios_play_outline,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Visible = false,
                Width = m_media_width * 2 - 100,
                Height = 43,
            };
            btn_play.MouseMove += f_form_move_MouseDown;
            btn_play.Click += f_audio_play_MouseClick;
            this.Controls.Add(btn_play);
        }

        private void f_audio_play_MouseClick(object sender, EventArgs e)
        {
            if (m_media_current_id > 0)
            {
                btn_play.Visible = false;
                if (string.IsNullOrEmpty(m_media.URL))
                {
                    f_video_openMp3_Request();
                }
                else
                {
                    m_media.Visible = true;
                    m_media.Ctlcontrols.play();
                }
            }
        }

        #endregion

        #region [ TAB ]

        private FATabStrip m_tab;
        private FATabStripItem m_tab_Store;
        private FATabStripItem m_tab_Search;
        private FATabStripItem m_tab_Tag;
        private FATabStripItem m_tab_Listen;
        private FATabStripItem m_tab_Speaking;
        private FATabStripItem m_tab_Word;
        private FATabStripItem m_tab_Grammar;
        private FATabStripItem m_tab_Text;
        private FATabStripItem m_tab_Writer;
        private FATabStripItem m_tab_Book;

        void f_tab_initUI()
        {

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

            //☆★☐☑⧉✉⦿⦾⚠⚿⛑✕✓⥀✖↭☊⦧▷◻◼⟲≔☰⚒❯►❚❚❮⟳⚑⚐✎✛
            //🕮🖎✍⦦☊🕭🔔🗣🗢🖳🎚🏷🖈🎗🏱🏲🗀🗁🕷🖒🖓👍👎♥♡♫♪♬♫🎙🎖🗝●◯⬤⚲☰⚒🕩🕪❯►❮⟳⚐🗑✎✛🗋🖫⛉ ⛊ ⛨⚏★☆

            m_tab = new FATabStrip()
            {
                Dock = DockStyle.Fill,
                AlwaysShowClose = false,
                AlwaysShowMenuGlyph = false,
                Margin = new Padding(0, 45, 0, 0),
            };
            m_tab_Store = new FATabStripItem()
            {
                CanClose = false,
                Title = "☰",
            };
            m_tab_Search = new FATabStripItem()
            {
                CanClose = false,
                Title = "⚲",
            };
            m_tab_Tag = new FATabStripItem()
            {
                CanClose = false,
                Title = "⛉",
            };
            m_tab_Speaking = new FATabStripItem()
            {
                CanClose = false,
                Title = "►",
            };
            m_tab_Listen = new FATabStripItem()
            {
                CanClose = false,
                Title = "☊", //☊
            };
            m_tab_Writer = new FATabStripItem()
            {
                CanClose = false,
                Title = "✍",
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
            m_tab_Text = new FATabStripItem()
            {
                CanClose = false,
                Title = "Text",
            };
            m_tab_Book = new FATabStripItem()
            {
                CanClose = false,
                Title = "Book",
            };

            m_tab.Items.AddRange(new FATabStripItem[] {
                m_tab_Store,
                m_tab_Search,
                m_tab_Tag,
                m_tab_Listen,
                m_tab_Speaking,
                m_tab_Writer,
                m_tab_Book,
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

            m_media_text = new TextBox()
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.Vertical,
                Font = font_Title
            };
            m_tab_Text.Padding = new Padding(9, 0, 0, 0);
            m_tab_Text.Controls.Add(m_media_text);

        }

        #endregion

        #region [ STORE ] 

        msg m_store_current_msg = null;
        long m_store_item_current_id = 0;
        string m_store_item_current_text = string.Empty;

        Label m_store_Message;
        Panel m_store_Result;
        TextBox m_store_Input;
        Panel m_store_Header;

        Label m_store_PageCurrent;
        Label m_store_PageTotal;
        Label m_store_TotalItems;

        void f_store_initUI()
        {
            m_store_Message = new Label()
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                //BackColor = Color.Gray,
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
            };

            m_store_Result = new Panel()
            {
                AutoScroll = true,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
            };
            m_store_Result.MouseMove += f_form_move_MouseDown;

            m_store_Input = new TextBox()
            {
                Dock = DockStyle.Left,
                Width = 123,
            };
            m_store_Input.KeyDown += f_store_input_KeyDown;
            m_store_Header = new Panel()
            {
                Height = 35,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Padding = new Padding(9, 9, 9, 0),
            };
            m_store_Header.MouseMove += f_form_move_MouseDown;
            m_tab_Store.Controls.AddRange(new Control[] {
                m_store_Result,
                m_store_Header,
                new Label(){ AutoSize = false, Height = 9, Dock = DockStyle.Top }
            });

            IconButton btn_saveResult = new IconButton(24) { IconType = IconType.ios_cloud_download, Dock = DockStyle.Left };
            IconButton btn_tags = new IconButton(24) { IconType = IconType.pricetags, Dock = DockStyle.Left, ToolTipText = "Tags" };
            IconButton btn_user = new IconButton(22) { IconType = IconType.person, Dock = DockStyle.Left, ToolTipText = "User" };
            IconButton btn_channel = new IconButton(22) { IconType = IconType.android_desktop, Dock = DockStyle.Left, ToolTipText = "Channel" };

            IconButton btn_next = new IconButton(16) { IconType = IconType.ios_arrow_next, Dock = DockStyle.Right };
            IconButton btn_prev = new IconButton(16) { IconType = IconType.ios_arrow_back, Dock = DockStyle.Right };
            IconButton btn_remove = new IconButton(22) { IconType = IconType.trash_a, Dock = DockStyle.Right };
            IconButton btn_add_playlist = new IconButton(22) { IconType = IconType.android_add, Dock = DockStyle.Right, ToolTipText = "Add to Playlist" };



            m_store_PageCurrent = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Gray,
                Text = "1",
                TextAlign = ContentAlignment.BottomRight,
                Dock = DockStyle.Right,
                Padding = new Padding(9, 3, 0, 0)
            };
            m_store_PageTotal = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Yellow,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            m_store_TotalItems = new Label()
            {
                AutoSize = true,
                //BackColor = Color.Blue,
                Text = "1",
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 3, 0, 0)
            };
            btn_next.Click += f_store_goPageNextClick;
            btn_prev.Click += f_store_goPagePrevClick;


            m_store_Message.MouseMove += f_form_move_MouseDown;
            m_store_Header.Controls.AddRange(new Control[] {
                #region

                m_store_Message,
                btn_channel,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_user,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_tags,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                //btn_saveResult,
                //new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                m_store_Input,

                btn_add_playlist,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_remove,
                new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
                btn_prev,
                m_store_PageCurrent,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "|",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_store_PageTotal,
                new Label()
                {
                    AutoSize = true,
                    //BackColor = Color.Red,
                    Text = "_",
                    TextAlign = ContentAlignment.BottomLeft,
                    Dock = DockStyle.Right,
                    Padding = new Padding(5,3,5,0),
                },
                m_store_TotalItems,
                new Label(){ Dock = DockStyle.Right, Padding = new Padding(0,3,0,0), Text = " items ", TextAlign = ContentAlignment.BottomLeft, AutoSize = true, },
                btn_next,

                #endregion
            });
        }

        private void f_store_draw_Media(List<long> ls)
        {
            m_store_Result.crossThreadPerformSafely(() =>
            {
                m_store_Result.Controls.Clear();
            });

            if (ls.Count == 0) return;

            const int margin_bottom = 5;
            const int margin_left = 9;

            int y = 0, x = 0, row = 0;
            Control[] pics = new Control[30];
            Control[] tits = new Control[30];
            Control[] stars = new Control[30];

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

                oMedia media = api_media.f_media_local_getInfo(ls[i]);
                if (media == null) continue;

                PictureBox pic = new PictureBox()
                {
                    //Text = i.ToString(),
                    //TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.LightGray,
                    Width = app.m_item_width,
                    Height = app.m_item_height,
                    Location = new Point(x, y),
                    Tag = media.Id
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
                    Name = media.Id.ToString(),
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
                };

                pic.MouseDoubleClick += f_store_picVideo_MouseDoubleClick;
                lbl.MouseClick += f_store_labelTitle_MouseClick;
                lbl.MouseMove += f_form_move_MouseDown;
                pic.MouseMove += f_form_move_MouseDown;

                pics[i] = pic;
                tits[i] = lbl;


                IconButton star = new IconButton(19)
                {
                    Name = "star-" + media.Id.ToString(),
                    IconType = IconType.ios_heart_outline,
                    Location = new Point(pic.Location.X + (app.m_box_width - 19), pic.Location.Y + 3),
                    BackColor = Color.LightGray,
                    ActiveColor = Color.Black,
                    Tag = media.Id.ToString() + "|" + media.Title,
                };
                star.MouseClick += f_store_bookmark_MouseClick;
                stars[i] = star;
            }

            #endregion

            m_store_Result.crossThreadPerformSafely(() =>
            {
                m_store_Result.Controls.AddRange(stars);
                m_store_Result.Controls.AddRange(tits);
                m_store_Result.Controls.AddRange(pics);
            });
        }

        void f_store_Result(oMediaSearchLocalResult rs)
        {
            f_store_draw_Media(rs.MediaIds);

            int page = rs.CountResult / rs.PageSize;
            if (rs.CountResult % rs.PageSize != 0) page++;

            m_store_PageCurrent.crossThreadPerformSafely(() =>
            {
                m_store_PageCurrent.Text = rs.PageNumber.ToString();
            });
            m_store_PageTotal.crossThreadPerformSafely(() =>
            {
                m_store_PageTotal.Text = page.ToString();
            });
            m_store_TotalItems.crossThreadPerformSafely(() =>
            {
                m_store_TotalItems.Text = rs.CountResult.ToString();
            });
        }

        private void f_store_goPagePrevClick(object sender, EventArgs e)
        {
            if (m_store_current_msg != null)
            {
                if (m_store_current_msg.PageNumber > 1)
                {
                    m_store_current_msg.PageNumber = m_store_current_msg.PageNumber - 1;
                    app.postToAPI(m_store_current_msg);
                }
            }
        }

        private void f_store_goPageNextClick(object sender, EventArgs e)
        {
            if (m_store_current_msg != null)
            {
                if ((m_store_current_msg.PageNumber - 1) * m_store_current_msg.PageSize < m_store_current_msg.Counter)
                {
                    m_store_current_msg.PageNumber = m_store_current_msg.PageNumber + 1;
                    app.postToAPI(m_store_current_msg);
                }
            }
        }

        private void f_store_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string key = m_store_Input.Text.Trim();
                if (key.Length > 1)
                {
                    m_store_Message.Text = "Finding [" + key + "] ...";
                    app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH_STORE, key);
                }
                else
                    app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH_STORE, string.Empty);
                //m_store_Message.Text = "Length of keywords must be greater than 1 characters.";
            }
        }

        private void f_store_picVideo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ((Control)sender).BackColor = Color.Gray;
            string mid = ((Control)sender).Tag.ToString();

            Control _lbl = m_store_Result.Controls.Find(mid, false).SingleOrDefault();
            if (_lbl != null)
                f_store_labelTitle_MouseClick(_lbl, null);
        }

        private void f_store_labelTitle_MouseClick(object sender, MouseEventArgs e)
        {
            Control it = ((Control)sender);
            long mid = long.Parse(it.Name);
            if (mid == m_media_current_id && e != null) return;

            it.BackColor = Color.Orange;
            Control star_sel = m_store_Result.Controls.Find("star-" + it.Name, false).SingleOrDefault();
            if (star_sel != null)
                star_sel.BackColor = Color.Orange;

            if (m_media_current_id > 0)
            {
                Control itprev = m_store_Result.Controls.Find(m_media_current_id.ToString(), false).SingleOrDefault();
                if (itprev != null)
                    itprev.BackColor = Color.LightGray;
                Control star_prev = m_store_Result.Controls.Find("star-" + m_media_current_id.ToString(), false).SingleOrDefault();
                if (star_prev != null)
                    star_prev.BackColor = Color.LightGray;
            }

            m_store_item_current_id = mid;
            m_store_item_current_text = it.Text;

            m_media_current_id = mid;
            m_media_current_title = it.Text;

            this.Text = m_media_current_title;
            lbl_title.Text = m_media_current_title;

            if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying) m_media.Ctlcontrols.stop();

            if (e != null)
            {
                // Only click on label title

                m_media.URL = string.Empty;
                if (m_media.Visible) m_media.Visible = false;
                btn_play.InActiveColor = Color.DimGray;
                btn_play.Visible = true;
                f_video_openMp3_Request();
                app.postToAPI(new msg() { API = _API.WORD, KEY = _API.WORD_KEY_ANALYTIC, Input = m_media_current_id });
            }
            else
            {
                // From picture click -> call click to lable title

                f_video_openMp4_Request();

                m_media.URL = string.Empty;
                if (m_media.Visible) m_media.Visible = false;
                btn_play.InActiveColor = Color.DimGray;
                btn_play.Visible = true;

                f_video_openMp3_Request();
                app.postToAPI(new msg() { API = _API.WORD, KEY = _API.WORD_KEY_ANALYTIC, Input = m_media_current_id });
            }
        }

        private void f_store_bookmark_MouseClick(object sender, MouseEventArgs e)
        {
            string tag = ((Control)sender).Tag.ToString(),
                mid = tag.Split('|')[0], title = mid.Length < tag.Length ? tag.Substring(mid.Length + 1) : string.Empty;
            m_msg_api.Text = "You saved item to bookmark: " + title;
            ((IconButton)sender).InActiveColor = Color.Red;
        }

        #endregion

        #region [ SEARCH ]

        long m_search_item_current_id = 0;
        string m_search_item_current_text = string.Empty;

        private msg m_search_current_msg = null;
        private TextBox m_search_Input;
        private Panel m_search_Result;
        private Label m_search_PageCurrent;
        private Label m_search_PageTotal;
        private Label m_search_TotalItems;
        private Label m_search_Message;
        private Panel m_search_Header;

        void f_search_initUI()
        {

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
            m_search_Header.MouseMove += f_form_move_MouseDown;
            m_tab_Search.Controls.AddRange(new Control[] {
                m_search_Result,
                m_search_Header,
                new Label(){ AutoSize = false, Height = 9, Dock = DockStyle.Top }
            });

            IconButton btn_save = new IconButton(24) { IconType = IconType.ios_cloud_download, Dock = DockStyle.Left };
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
            m_search_TotalItems = new Label()
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
                //btn_channel,
                //new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                //btn_user,
                //new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                //btn_tags,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                btn_save,
                new Label(){ Dock = DockStyle.Left, AutoSize = false, Width = 5 },
                m_search_Input,

                //btn_add_playlist,
                //new Label(){ Dock = DockStyle.Right, AutoSize = false, Width = 9 },
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
                m_search_TotalItems,
                new Label(){ Dock = DockStyle.Right, Padding = new Padding(0,3,0,0), Text = " items ", TextAlign = ContentAlignment.BottomLeft, AutoSize = true, },
                btn_prev,

                #endregion
            });
            btn_save.MouseClick += f_search_saveItemSelected;
            btn_remove.MouseClick += f_search_removeCacheAll;
        }

        private void f_search_removeCacheAll(object sender, MouseEventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to clear all result search?", "Confirm clear cache search!", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_ONLINE_CACHE_CLEAR });
            }
        }

        private void f_search_saveItemSelected(object sender, MouseEventArgs e)
        {
            if (m_search_item_current_id == 0 || string.IsNullOrEmpty(m_search_item_current_text))
            {
                MessageBox.Show("Please select item from tab search result to save!");
            }
            else
            {
                if (api_media.f_media_local_exist(m_search_item_current_id))
                    MessageBox.Show(string.Format("The [{0}] saved", m_search_item_current_text));
                else
                    app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_SEARCH_ONLINE_SAVE_TO_STORE, Input = m_search_item_current_id });
            }
        }

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

                oMedia media = api_media.f_media_search_getInfo(ls[i]);
                if (media == null) continue;

                PictureBox pic = new PictureBox()
                {
                    //Text = i.ToString(),
                    //TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.LightGray,
                    Width = app.m_item_width,
                    Height = app.m_item_height,
                    Location = new Point(x, y),
                    Tag = media.Id
                };

                Bitmap img = api_media.f_media_search_getPhoto(media.Id);
                if (img != null) pic.Image = img;

                Label lbl = new Label()
                {
                    Name = media.Id.ToString(),
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
                };

                pic.MouseDoubleClick += (se, ev) =>
                {
                    ((Control)se).BackColor = Color.Gray;
                    string mid = ((Control)se).Tag.ToString();

                    Control _lbl = m_search_Result.Controls.Find(mid, false).SingleOrDefault();
                    if (_lbl != null)
                        f_search_labelTitle_MouseClick(_lbl, null);
                };
                lbl.MouseDoubleClick += f_search_labelTitle_MouseClick;
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
            m_search_TotalItems.crossThreadPerformSafely(() =>
            {
                m_search_TotalItems.Text = rs.CountResult.ToString();
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
                    app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH_ONLINE, key);
                }
                else
                    m_search_Message.Text = "Length of keywords must be greater than 1 characters.";
            }
        }

        private void f_search_labelTitle_MouseClick(object sender, MouseEventArgs e)
        {
            Control it = ((Control)sender);
            it.BackColor = Color.Orange;
            long mediaId_prev = 0;
            if (m_search_Result.Tag != null) mediaId_prev = (long)m_search_Result.Tag;
            if (mediaId_prev > 0)
            {
                Control itprev = m_search_Result.Controls.Find(mediaId_prev.ToString(), false).SingleOrDefault();
                if (itprev != null)
                    itprev.BackColor = Color.LightGray;
            }

            long mediaId_sel = long.Parse(it.Name);
            m_search_Result.Tag = mediaId_sel;

            if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying) m_media.Ctlcontrols.stop();

            m_search_item_current_id = mediaId_sel;
            m_search_item_current_text = it.Text;

            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_TEXT_VIDEO_ONLINE, Input = mediaId_sel });
        }

        #endregion

        #region [ MEDIA ]

        private void f_media_event_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
        }

        void f_video_openMp4_Request()
        {
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_PLAY_VIDEO, Input = m_media_current_id });
        }

        void f_video_openMp4_Callback(string url, string title)
        {
            if (url != string.Empty)
            {
                //this.Invoke((Action)(() =>
                //{
                //    this.Cursor = Cursors.WaitCursor;
                //    //var f = new fPlayer(url, title);
                //    //f.Show();
                //    //f.Left = this.Location.X + 9;
                //    //f.Top = this.Location.Y + 99;

                //    this.Cursor = Cursors.Default;
                //}));

                this.Invoke((Action)(() =>
                {
                    int left = this.Location.X + 9,
                    top = this.Location.Y + 99,
                    width = app.m_player_width,
                    height = app.m_player_height;

                    if (m_player != null)
                    {
                        left = m_player.Left;
                        top = m_player.Top;
                        width = m_player.Width;
                        height = m_player.Height;

                        m_player.Close();
                        m_player = null;
                    }

                    m_player = new fPlayer(url, title);
                    m_player.Shown += (se, ev) =>
                    {
                        m_player.Height = height;
                        m_player.Width = width;
                        m_player.Left = left;
                        m_player.Top = top;
                    };
                    m_player.Show();
                }));
            }
            else
                MessageBox.Show("Cannot open videoId: " + title);
        }

        void f_video_openMp3_Request()
        {
            app.postToAPI(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_PLAY_AUDIO, Input = m_media_current_id });
        }

        void f_video_openMp3_Callback(string url, string title)
        {
            if (url != string.Empty)
            {
                this.Invoke((Action)(() =>
                {
                    this.Cursor = Cursors.WaitCursor;
                    btn_play.InActiveColor = Color.Orange;
                    m_media.Visible = true;
                    m_media.URL = url;
                    m_media.close();
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
                    case _API.MSG_MEDIA_SEARCH_RESULT: 
                        log.Append(m.Log + Environment.NewLine);
                        m_msg_api.crossThreadPerformSafely(() =>
                        {
                            m_msg_api.Text = m.Log;
                        });
                        break; 
                    case _API.MSG_MEDIA_SEARCH_SAVE_TO_STORE:
                        log.Append(m.Log + Environment.NewLine);
                        m_msg_api.crossThreadPerformSafely(() =>
                        {
                            m_msg_api.Text = m.Log;
                        });
                        app.postToAPI(m_search_current_msg);
                        break;
                    case _API.WORD:
                        #region
                        switch (m.KEY)
                        {
                            case _API.WORD_KEY_ANALYTIC:
                                if (m.Output.Ok && (long)m.Input == m_media_current_id)
                                {
                                    m_media_text.crossThreadPerformSafely(() =>
                                    {
                                        m_media_text.Text = (string)m.Output.Data;
                                    });
                                }
                                break;
                        }
                        break;
                    #endregion
                    case _API.MEDIA:
                        #region
                        switch (m.KEY)
                        {
                            case _API.MEDIA_KEY_SEARCH_ONLINE_CACHE_CLEAR:
                                log.Append(m.Log + Environment.NewLine);
                                m_msg_api.crossThreadPerformSafely(() =>
                                {
                                    m_msg_api.Text = m.Log;
                                }); 
                                break;
                            case _API.MEDIA_KEY_TEXT_VIDEO_ONLINE:
                                if (m.Output.Ok)
                                {
                                    m_media_text.crossThreadPerformSafely(() =>
                                    {
                                        m_media_text.Text = (string)m.Output.Data;
                                    });
                                }
                                break;
                            case _API.MEDIA_KEY_PLAY_VIDEO_ONLINE:
                                if (m.Output.Ok)
                                {
                                    f_video_openMp4_Callback((string)m.Output.Data, m.Log);
                                    this.Invoke((Action)(() =>
                                    {
                                        btn_play.Visible = false;
                                        m_media.Visible = false;
                                        this.Text = "English";
                                        this.lbl_title.Text = string.Empty;
                                    }));
                                }
                                break;
                            case _API.MEDIA_KEY_SEARCH_ONLINE_CACHE:
                                if (m.Output.Ok)
                                {
                                    var rs = (oMediaSearchLocalResult)m.Output.Data;
                                    f_search_Result(rs);
                                    m_search_current_msg = m.clone(m.Input);
                                }
                                else
                                {
                                    MessageBox.Show("Search online error");
                                }
                                break;
                            case _API.MEDIA_KEY_SEARCH_STORE:
                                if (m.Output.Ok)
                                {
                                    var rs = (oMediaSearchLocalResult)m.Output.Data;
                                    f_store_Result(rs);
                                    m_store_current_msg = m.clone(m.Input);
                                }
                                else
                                {
                                    MessageBox.Show("Search store error");
                                }
                                break;
                            case _API.MEDIA_KEY_PLAY_AUDIO:
                                if (m.Output.Ok && (long)m.Input == m_media_current_id)
                                    f_video_openMp3_Callback((string)m.Output.Data, m.Log);
                                break;
                            case _API.MEDIA_KEY_PLAY_VIDEO:
                                if (m.Output.Ok && (long)m.Input == m_media_current_id)
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

        public fMain()
        {
            log = new StringBuilder();
            this.Icon = Resources.favicon;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Shown += (se, ev) => f_main_Shown();

            f_audio_initUI();
            f_tab_initUI();

            f_store_initUI();
            f_search_initUI();
        }
    }
}
