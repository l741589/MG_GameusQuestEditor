using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MG_GameusQuestEditor {
    /// <summary>
    /// SelectIcon.xaml 的交互逻辑
    /// </summary>
    public partial class SelectIcon : Window {

        private int iconWidth = 32;
        public int Index { get; set; }

        public SelectIcon() {
            InitializeComponent();
            image.Source = D.IconSet;
            Title = "SelectIcon: " + Index;
            Loaded += SelectIcon_Loaded;
        }

        void SelectIcon_Loaded(object sender, RoutedEventArgs e) {
            int lineCount = (int)D.IconSet.Width / iconWidth;
            rect.Margin = new Thickness(Index % lineCount * iconWidth + 2, Index / lineCount * iconWidth + 2, 0, 0);
        }        

        private void image_MouseDown(object sender, MouseButtonEventArgs e) {
            var pos=e.GetPosition(sender as IInputElement);
            rect.Margin = new Thickness((int)pos.X / iconWidth * iconWidth+2, (int)pos.Y / iconWidth * iconWidth+2, 0, 0);
            int lineCount=(int)D.IconSet.Width/iconWidth;
            Index = lineCount * ((int)pos.Y / iconWidth) + (int)pos.X / iconWidth;
            Title = "SelectIcon: " + Index;
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left) {
                DialogResult = true;
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }
    }
}
