using AxWMPLib;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace appel
{
    public class fPlayer : Form, IFORM
    {
        const bool m_hook_MouseMove = true;

        private AxWindowsMediaPlayer m_media;
        private ControlTransparent m_modal;
        private Panel m_resize;
        private bool m_resizing = false;
        private IconButton btn_exit;

        public bool isOpening { set; get; } = false;
        public void f_init()
        {

            this.TopMost = true;
            this.Icon = Resources.favicon;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            //if (!string.IsNullOrEmpty(title)) this.Text = title;
            this.Shown += (se, ev) =>
            {
                if (isOpening == false)
                {
                    f_init();
                    isOpening = true;
                }

                this.TopMost = true;
                this.Width = app.m_player_width;
                this.Height = app.m_player_height;

                m_media.settings.volume = 100;
                m_media.uiMode = "none";
                m_modal.Size = new Size(this.Width, this.Height);

                m_resize.Location = new Point(this.Width - m_resize.Width, this.Height - m_resize.Height);


                m_modal.BringToFront();

                btn_exit.Location = new Point(this.Width - (btn_exit.Width - 5), 0);
                btn_exit.BringToFront();

                m_resize.BringToFront();

                //f_play(url, title);
                isOpening = true;
            };

            // MEDIA
            m_media = new AxWindowsMediaPlayer();
            m_media.Dock = DockStyle.Fill;
            m_media.Enabled = true;
            m_media.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(this.f_media_event_PlayStateChange);
            this.Controls.Add(m_media);

            // MODAL
            m_modal = new ControlTransparent();
            m_modal.Location = new Point(0, 0);
            m_modal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            m_modal.BackColor = Color.Black;
            m_modal.Opacity = 1;
            m_modal.MouseMove += f_form_move_MouseDown;
            m_modal.Click += (se, ev) =>
            {
                if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    m_media.Ctlcontrols.pause();
                }
                else
                {
                    m_media.Ctlcontrols.play();
                }
            };
            this.Controls.Add(m_modal);
            m_modal.MouseDoubleClick += (se, ev) =>
            {
                if (m_media.uiMode == "none")
                {
                    m_media.uiMode = "full";
                    m_modal.Height = this.Height - 45;
                }
                else
                {
                    m_media.uiMode = "none";
                    m_modal.Height = this.Height;
                }
                m_media.Ctlcontrols.play();
            };

            // RESIZE
            m_resize = new Panel();
            m_resize.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            m_resize.BackColor = Color.Black;
            m_resize.Size = new Size(8, 8);
            this.Controls.Add(m_resize);
            m_resize.MouseDown += (se, ev) => { f_hook_mouse_Open(); m_resizing = true; };
            m_resize.MouseUp += (se, ev) =>
            {
                m_resizing = false;
                f_hook_mouse_Close();
                //Debug.WriteLine("RESIZE: ok ");
                if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    m_media.Ctlcontrols.pause();
                    m_media.Ctlcontrols.play();
                }
                else if (m_media.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    m_media.Ctlcontrols.play();
                    m_media.Ctlcontrols.pause();
                }
            };

            btn_exit = new IconButton(18) { IconType = IconType.ios_close_empty, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            btn_exit.Click += exit_Click;
            this.Controls.Add(btn_exit);
        }

        public fPlayer()
        {
            //this.Location = new Point(-99, -99);
            //this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
            //this.Top = -99;
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.Width = 1;
            //this.Height = 1;
            //this.BackColor = Color.Black;
            //this.Shown += (se, ev) => {
            //    this.Width = 1;
            //    this.Height = 1;
            //    //this.Location = new Point(-99, -99);
            //};
        }

        public void f_form_freeResource()
        {
        }


        private void exit_Click(object sender, EventArgs e)
        {
            m_media.Ctlcontrols.stop();
            m_media.Dispose();
            this.Close();
        }

        #region [ MEDIA PLAYER ]

        private void menuItem_Create()
        {
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
        }

        private void f_media_event_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            /**** Don't add this if you want to play it on multiple screens***** /
             * 
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
            /********************************************************************/

            if (m_media.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                //Application.Exit();
            }


            switch (m_media.playState)
            {
                case WMPLib.WMPPlayState.wmppsTransitioning:

                    break;
                case WMPLib.WMPPlayState.wmppsPlaying:

                    break;
            }


        }

        private string URL = string.Empty;
        private string TITLE = string.Empty;
        public void open(string path, string title)
        {
            URL = path;
            TITLE = title;

            m_media.URL = path;
            this.Text = title;
        }

        public void pause()
        {
            if (string.IsNullOrEmpty(URL)) return;

            if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying)
                m_media.Ctlcontrols.pause();
            else
                m_media.Ctlcontrols.play();
        }

        public void play()
        {
            if (string.IsNullOrEmpty(URL)) return;

            if (m_media.playState != WMPLib.WMPPlayState.wmppsPlaying)
                m_media.Ctlcontrols.play();
        }

        public void stop()
        {
            if (string.IsNullOrEmpty(URL)) return;

            m_media.Ctlcontrols.stop();
        }

        #endregion

        /*////////////////////////////////////////////////////////////////////////*/

        #region [ MOUSE MOVE: IN FORM, OUT FORM ]

        private void f_mouse_move_intoForm(int x, int y)
        {
            f_form_Resize(x, y, MOUSE_XY.INT);
        }

        private void f_mouse_move_outForm(int x, int y)
        {
            f_form_Resize(x, y, MOUSE_XY.OUT);
        }

        #endregion

        #region [ FORM MOVE, RESIZE ]

        enum MOUSE_XY { OUT, INT };

        void f_form_Resize(int x, int y, MOUSE_XY type)
        {
            if (m_resizing)
            {
                int max_x = this.Location.X + this.Width;
                int max_y = this.Location.Y + this.Height;
                this.Width = x - this.Location.X;
                this.Height = y - this.Location.Y;
            }
        }

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

        #region [ HOOK MOUSE: MOVE, WHEEL ... ]

        void f_hook_mouse_move_CallBack(MouseEventArgs e)
        {
            int max_x = this.Width + this.Location.X,
                max_y = e.Location.Y + this.Height;
            //Debug.WriteLine(this.Location.X + " " +e.X  + " " + max_x + " | " + this.Location.Y + " " +e.Y  + " " + max_y);

            if (e.X > this.Location.X && e.X < max_x
                && e.Y > this.Location.Y && e.Y < max_y)
            {
                //Debug.WriteLine("IN FORM: "+ this.Location.X + " " + e.X + " " + max_x + " | " + this.Location.Y + " " + e.Y + " " + max_y);
                f_mouse_move_intoForm(e.X, e.Y);
            }
            else
            {
                //Debug.WriteLine("OUT FORM: " + this.Location.X + " " + e.X + " " + max_x + " | " + this.Location.Y + " " + e.Y + " " + max_y);
                f_mouse_move_outForm(e.X, e.Y);
            }
        }

        void f_hook_mouse_Open()
        {
            if (m_hook_MouseMove)
                f_hook_mouse_SubscribeGlobal();
        }

        void f_hook_mouse_Close()
        {
            if (m_hook_MouseMove)
                f_hook_mouse_Unsubscribe();
        }

        /*////////////////////////////////////////////////////////////////////////*/

        private IKeyboardMouseEvents hook_events;

        private void f_hook_mouse_SubscribeApplication()
        {
            f_hook_mouse_Unsubscribe();
            f_hook_mouse_Subscribe(Hook.AppEvents());
        }

        private void f_hook_mouse_SubscribeGlobal()
        {
            f_hook_mouse_Unsubscribe();
            f_hook_mouse_Subscribe(Hook.GlobalEvents());
        }

        private void f_hook_mouse_Subscribe(IKeyboardMouseEvents events)
        {
            hook_events = events;
            //m_Events.KeyDown += OnKeyDown;
            //m_Events.KeyUp += OnKeyUp;
            //m_Events.KeyPress += HookManager_KeyPress;

            //m_Events.MouseUp += OnMouseUp;
            //m_Events.MouseClick += OnMouseClick;
            //m_Events.MouseDoubleClick += OnMouseDoubleClick;

            hook_events.MouseMove += f_hook_mouse_HookManager_MouseMove;

            //m_Events.MouseDragStarted += OnMouseDragStarted;
            //m_Events.MouseDragFinished += OnMouseDragFinished;

            //if (checkBoxSupressMouseWheel.Checked)
            //m_Events.MouseWheelExt += f_hook_mouse_HookManager_MouseWheelExt;
            //else
            ////hook_events.MouseWheel += f_hook_mouse_HookManager_MouseWheel;

            //if (checkBoxSuppressMouse.Checked)
            //m_Events.MouseDownExt += HookManager_Supress;
            //else
            //m_Events.MouseDown += OnMouseDown;
        }


        private void f_hook_mouse_Unsubscribe()
        {
            if (hook_events == null) return;
            //m_Events.KeyDown -= OnKeyDown;
            //m_Events.KeyUp -= OnKeyUp;
            //m_Events.KeyPress -= HookManager_KeyPress;

            //m_Events.MouseUp -= OnMouseUp;
            //m_Events.MouseClick -= OnMouseClick;
            //m_Events.MouseDoubleClick -= OnMouseDoubleClick;

            hook_events.MouseMove -= f_hook_mouse_HookManager_MouseMove;

            //m_Events.MouseDragStarted -= OnMouseDragStarted;
            //m_Events.MouseDragFinished -= OnMouseDragFinished;

            //if (checkBoxSupressMouseWheel.Checked)
            //m_Events.MouseWheelExt -= f_hook_mouse_HookManager_MouseWheelExt;
            //else
            //hook_events.MouseWheel -= f_hook_mouse_HookManager_MouseWheel;

            //if (checkBoxSuppressMouse.Checked)
            //m_Events.MouseDownExt -= HookManager_Supress;
            //else
            //m_Events.MouseDown -= OnMouseDown;

            hook_events.Dispose();
            hook_events = null;
        }

        private void f_hook_mouse_HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            f_hook_mouse_move_CallBack(e);
        }

        ////private void f_hook_mouse_HookManager_MouseWheel(object sender, MouseEventArgs e)
        ////{
        ////    //Debug.WriteLine(string.Format("Wheel={0:000}", e.Delta));
        ////    //f_hook_mouse_wheel_CallBack(e);
        ////}

        ////private void f_hook_mouse_HookManager_MouseWheelExt(object sender, MouseEventExtArgs e)
        ////{
        ////    //Debug.WriteLine(string.Format("Wheel={0:000}", e.Delta)); 
        ////    //Debug.WriteLine("Mouse Wheel Move Suppressed.\n");
        ////    e.Handled = true;
        ////    //e.Handled = true; // true: break event at here, stop mouse wheel at here
        ////}

        /////////////////////////////////////////////////////////////


        #endregion

        /*////////////////////////////////////////////////////////////////////////*/

        #region [ API RESPONSE ]


        public void api_responseMsg(object sender, threadMsgEventArgs e)
        {
        }

        public void api_initMsg(msg m)
        {
        }
        #endregion
    }
}
