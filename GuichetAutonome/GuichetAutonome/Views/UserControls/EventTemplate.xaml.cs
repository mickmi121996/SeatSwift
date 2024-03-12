﻿using GuichetAutonome.ViewModels.UserControls;
using SeatSwiftDLL;
using System;
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

namespace GuichetAutonome.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EventTemplate.xaml
    /// </summary>
    public partial class EventTemplate : UserControl
    {
        public EventTemplate(Show show)
        {
            InitializeComponent();
            VMEventTemplate vMEventTemplate = new VMEventTemplate(show);
            this.DataContext = vMEventTemplate;
        }
    }
}
