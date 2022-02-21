using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TGF_Client.ViewModel;

namespace TGF_Client.View
{
    /// <summary>
    /// Interaction logic for SubjectSelection.xaml
    /// </summary>
    public partial class SubjectSelection : UserControl
    {
        public SubjectSelection()
        {
            InitializeComponent();
        }

        private void Robot_Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((SubjectSelectionVM)(this.DataContext)).SelectedRobot();
        }

        private void Human_Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((SubjectSelectionVM)(this.DataContext)).SelectedHuman();
        }
    }
}
