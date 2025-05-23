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
using System.Windows.Shapes;

namespace ProblemsBoardLib.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для DocumentPreWatch.xaml
    /// </summary>
    public partial class DocumentPreWatch : Window
    {
        public DocumentPreWatch()
        {
            InitializeComponent();
        }

        private void AcceptBT_Click(object sender, RoutedEventArgs e)
        {
            DV.Document = null;
            DialogResult = true;
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            DV.Document = null;
            DialogResult = false;
        }
    }
}
