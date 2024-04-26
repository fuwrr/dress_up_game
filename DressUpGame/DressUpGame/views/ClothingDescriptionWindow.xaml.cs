using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace DressUpGame
{
    public partial class ClothingDescriptionWindow : Window
    {
        public ClothingDescriptionWindow(string outfitDescription)
        {
            InitializeComponent();
            DescriptionTextBlock.Text = outfitDescription;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button button)
            {
                button.Width = 80; 
            }
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button button)
            {
                button.Width = 70;
            }
        }
    }
}
