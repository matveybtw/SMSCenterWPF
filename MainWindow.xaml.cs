using System;
using System.Windows;
using PorjectSMSCWPF.Viewmodels;
namespace PorjectSMSCWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewmodel();
        }
    }
}
