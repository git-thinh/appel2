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
        private AxWindowsMediaPlayer m_media;
        private ControlTransparent m_modal;
        private ContextMenu m_menu;
        private bool allow_hookMouseWheel = true;

        bool m_mouse_right = false;
        public fPlayer()
        {
            // FORM
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;

            // MEDIA
            m_media = new AxWindowsMediaPlayer();
            m_media.Dock = DockStyle.None;
            m_media.Location = new Point(0, 0);
            m_media.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            m_media.Enabled = true;
            m_media.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(this.f_media_event_PlayStateChange);
            this.Controls.Add(m_media);

            // MODAL
            m_modal = new ControlTransparent();
            m_media.Dock = DockStyle.None;
            m_modal.Location = new Point(0, 0);
            m_modal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            m_modal.BackColor = Color.Black;
            m_modal.Opacity = 1;
            m_modal.DoubleClick += (se, ev) =>
            {
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.TopMost = false;
                    m_media.Ctlcontrols.pause();
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.TopMost = true;
                    m_media.Ctlcontrols.play();
                }
            };
            m_modal.MouseDown += (se, ev) =>
            {
                m_mouse_right = false;
                if (ev.Button == MouseButtons.Right)
                {
                    m_mouse_right = true;
                    // show menu context
                    m_menu.Show(this, new Point(ev.X, ev.Y));
                    m_mouse_right = false;
                }
                else if (ev.Button == MouseButtons.Left)
                {
                    if (m_media.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        m_media.Ctlcontrols.pause();
                    }
                    else
                    {
                        m_media.Ctlcontrols.play();
                    }
                }
            };
            this.Controls.Add(m_modal);
            m_modal.MouseLeave += (se, ev) =>
            {
                Debug.WriteLine("MouseLeave: " + m_mouse_right.ToString());
                //menuItem_Click(m_menu.MenuItems[0], new EventArgs());

                //m_menu.Show(this, new Point(-1000, 0)); 
                //SendKeys.SendWait("{ESC}");
                //this.Focus();
            };
            m_modal.MouseMove += (se, ev) =>
            {
                //Debug.WriteLine("MouseMove: " + m_mouse_right.ToString());
                //menuItem_Click(m_menu.MenuItems[0], new EventArgs());

                f_form_move_MouseDown(se, ev);
            };

            MenuItem[] mi = new MenuItem[3];
            mi[0] = new MenuItem("Item1", menuItem_Click);
            mi[1] = new MenuItem("Item2", menuItem_Click);
            mi[2] = new MenuItem("Item3", menuItem_Click);
            m_menu = new ContextMenu(mi);
            m_menu.MenuItems[0].Visible = false;

            // FORM SHOWN
            this.Shown += (se, ev) =>
            {
                this.TopMost = true;
                m_media.uiMode = "none";
                m_media.Size = new Size(this.Width, this.Height);
                m_modal.Size = new Size(this.Width, this.Height);

                m_modal.BringToFront();
                f_hook_mouse_Open();
            };
        }

        public void f_free_Resource()
        {
            f_hook_mouse_Close();
        }

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
        public void f_play(string path)
        {
            m_media.URL = path;
        }

        /*////////////////////////////////////////////////////////////////////////*/

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

        #region [ HOOK MOUSE: MOUSE WHEEL, RUN SCROLLBAR ... ]

        void f_hook_mouse_move_CallBack(MouseEventArgs e)
        {
            if (e.X > this.Location.X && e.X < this.Width + this.Location.X && e.Y > e.Location.Y && e.Y < e.Location.Y + this.Height)
            {
                Debug.WriteLine("IN FORM");
            }
            else
            {
                Debug.WriteLine("OUT FORM");

            }
        }

        void f_hook_mouse_Open()
        {
            if (allow_hookMouseWheel)
                f_hook_mouse_SubscribeGlobal();
        }

        void f_hook_mouse_Close()
        {
            if (allow_hookMouseWheel)
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
