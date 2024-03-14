using AppGestion.ViewModels.Pages;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AppGestion.Views.Pages
{
    public partial class ManageTheater : Page
    {

        #region properties



        #endregion

        #region Constructor

        public ManageTheater()
        {
            InitializeComponent();
            VMManageTheater vMManageTheater = new VMManageTheater();
            this.DataContext = vMManageTheater;
        }

        #endregion
    }
}
