using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace SalesMaster.View
{
    /// <summary>
    /// NotifyBox.xaml 的交互逻辑
    /// </summary>
    public partial class NotifyBox : Window
    {
        public string NotifyMessage { get; set; }

        public NotifyBox()
        {
            InitializeComponent();
            Left = SystemParameters.WorkArea.Width - Width;
            Top = SystemParameters.WorkArea.Height - Height;
        }

        async private void Storyboard_Completed(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            BeginStoryboard(FindResource("CloseStoryBoard") as Storyboard);
        }

        private void Storyboard_Completed_1(object sender, EventArgs e)
        {
            Close();
        }

        private void NotifyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Text = NotifyMessage;
        }
    }
}
