﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventarioCasaCeja
{
    public partial class LoadWindow : Form
    {
        public LoadWindow()
        {
            InitializeComponent();
        }
        public void setData(int progress, string labeltext)
        {
            progressBar1.Value = progress;
            label1.Text = labeltext;
            
        }
    }
}