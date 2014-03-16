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
        /// Converts the text in a control into an integer value and returns the result.
        /// </summary>
        public static int GetIntegerValue(Control c)
        {
            return Convert.ToInt32(c.Text);
        }
    }
}
 