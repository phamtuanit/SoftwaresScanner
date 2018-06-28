#region Reference

using MahApps.Metro.Controls;

#endregion

namespace SoftwaresAnalyze
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowModel();
        }
    }
}