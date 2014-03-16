using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pathfinder
{
    class FormUtil
    {
        /// <summary>
        /// Converts the text in a textbox into an integer value and returns the result.
        /// </summary>
        public static int GetIntegerValue(System.Windows.Forms.TextBox tb)
        {
            return Convert.ToInt32(tb.Text);
        }

        /// <summary>
        /// Converts the text in a combobox into an integer value and returns the result.
        /// </summary>
        public static int GetIntegerValue(ComboBox cb)
        {
            return Convert.ToInt32(cb.Text);
        }
    }
}
 