using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YouTrackPowerToy
{
    public partial class SearchBox : Form
    {
        public SearchBox()
        {
            InitializeComponent();
        }

        public string SearchString { get { return textFilter.Text; } }
    }
}
