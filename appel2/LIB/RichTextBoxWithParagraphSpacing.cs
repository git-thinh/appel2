using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appel
{
    public class RichTextBoxWithParagraphSpacing : RichTextBox
    {
        private const int PFM_SPACEBEFORE = 64;
        private const int PFM_SPACEAFTER = 128;
        private const int EM_SETPARAFORMAT = 1095;
        private const int SCF_SELECTION = 1;

        public int SelectionParagraphSpacingAfter
        {
            set
            {
                PARAFORMAT fmt = new PARAFORMAT();
                fmt.cbSize = Marshal.SizeOf(fmt);
                fmt.dwMask = PFM_SPACEAFTER;
                fmt.dySpaceAfter = value;
                SendMessage(new HandleRef(this, this.Handle),
                             EM_SETPARAFORMAT,
                             SCF_SELECTION,
                             ref fmt
                           );
            }
        }

        public int SelectionParagraphSpacingBefore
        {
            set
            {
                PARAFORMAT fmt = new PARAFORMAT();
                fmt.cbSize = Marshal.SizeOf(fmt);
                fmt.dwMask = PFM_SPACEBEFORE;
                fmt.dySpaceBefore = value;
                SendMessage(new HandleRef(this, this.Handle),
                             EM_SETPARAFORMAT,
                             SCF_SELECTION,
                             ref fmt
                           );
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PARAFORMAT
        {
            public int cbSize;
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;

            // PARAFORMAT2 from here onwards.
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
                                               int msg,
                                               int wParam,
                                               ref PARAFORMAT lp);
    }

}