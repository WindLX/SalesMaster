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
using System.Text.RegularExpressions;
using SalesMaster.ViewModel;

namespace SalesMaster.View
{
    /// <summary>
    /// EditMain.xaml 的交互逻辑
    /// </summary>
    public partial class EditMain : UserControl
    {
        public EditMain(object parameter)
        {
            DataContext = new EditMainViewModel(parameter);
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            if (Regex.IsMatch(textBox.Text, @"[\u4e00-\u9fbb]+$"))
            {
                textBox.Text = "0";
                e.Handled = true;
                return;
            }
            if (Regex.IsMatch(textBox.Text, @"[a-zA-Z]+$"))
            {
                textBox.Text = "0";
                e.Handled = true;
                return;
            }
            if (Regex.IsMatch(textBox.Text, @"[。，、·；‘’“”：《》？]+$"))
            {
                textBox.Text = "0";
                e.Handled = true;
                return;
            }
            Regex numbeRegex = new Regex("^[1-9]+[0-9]*$");
            e.Handled =
                    !numbeRegex.IsMatch(
                        textBox.Text.Insert(
                            textBox.SelectionStart, e.Text));
            textBox.Text = textBox.Text.Trim();
        }

        private void TextBox_PreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            if (Regex.IsMatch(textBox.Text, @"[\u4e00-\u9fbb]+$"))
            {
                textBox.Text = "0";
                e.Handled = true;
                return;
            }
            if (Regex.IsMatch(textBox.Text, @"[a-zA-Z]+$"))
            {
                textBox.Text = "0";
                e.Handled = true;
                return;
            }
            if (Regex.IsMatch(textBox.Text, @"[。，、·；‘’“”：《》？]$"))
            {
                textBox.Text = "0";
                e.Handled = true;
                return;
            }
            Regex numbeRegex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled =
                    !numbeRegex.IsMatch(
                        textBox.Text.Insert(
                            textBox.SelectionStart, e.Text));
            textBox.Text = textBox.Text.Trim();
        }
    }
}
