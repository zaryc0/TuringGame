﻿using System;
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

namespace TGF_Client.View
{
    /// <summary>
    /// Interaction logic for MessageBoard.xaml
    /// </summary>
    public partial class MessageBoard : UserControl
    {
        public MessageBoard()
        {
            InitializeComponent();

            Scrollable.ScrollToBottom();
        }
    }
}
