using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.Utils
{
    static class AddedControlsAligment
    {
        public static void AlignHorisontalByMaximum(List<Control> controls)
        {
            SetAligmentToAddedControls(WhichControlAddedLargesize(controls), controls);
        }
        public static int WhichControlAddedLargesize(List<Control> controls)
        {
            if (controls != null)
            {
                int max = 0;
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i].Location.X + controls[i].Width > max)
                    {
                        max = controls[i].Location.X + controls[i].Width;
                    }
                }
                return max;
            }
            return 0;
        }

        private static void SetAligmentToAddedControls(int max, List<Control> controls)
        {
            foreach (Control item in controls)
            {
                item.Location = new Point(max - item.Width, item.Location.Y);
            }
        }
    }
}
