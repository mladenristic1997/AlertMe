﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlertMe.Timeline
{
    /// <summary>
    /// Interaction logic for Timeline.xaml
    /// </summary>
    public partial class Progress : UserControl
    {
        public ProgressViewModel ViewModel { get; set; }
        public Progress()
        {
            ViewModel = new ProgressViewModel();
            InitializeComponent();
            ProgressControl.DataContext = ViewModel;
        }
    }
}
