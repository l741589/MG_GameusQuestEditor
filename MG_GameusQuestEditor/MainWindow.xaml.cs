using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MG_GameusQuestEditor {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {

        
        public MainWindow() {
            InitializeComponent();
            try {
                D.Init((Data)Resources["data"]);
            } catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("please place this program in your project directory");
                Close();
            }
        }
        

        private void listItem_Loaded(object sender, RoutedEventArgs e) {
            FrameworkElement elem = sender as FrameworkElement;
            while (elem!=null&&!(elem is ListBoxItem))
                elem = elem.Parent as FrameworkElement;
            if (elem == null) return;
            Binding b = new Binding("ActualWidth");
            b.Source = cateList;
            b.Mode = BindingMode.OneWay;
            b.Converter = new ItemWidthConverter();
            elem.SetBinding(FrameworkElement.MaxWidthProperty, b);
        }

        private void doEdit(object sender) {
            FrameworkElement g = sender as FrameworkElement;
            if (g == null) return;
            TextBox tb = g.FindName("cateEdit") as TextBox;
            if (tb == null) return;
            tb.Visibility = Visibility.Visible;
            //tb.Focus();
            Dispatcher.BeginInvoke(new Action(() => Keyboard.Focus(tb)));
        }

        private void cateItemDblClick(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left) {
                doEdit(sender);
            }
        }

        private void cateEdit_LostFocus(object sender, RoutedEventArgs e) {
            (sender as TextBox).Visibility = Visibility.Hidden;
        }

        private void stepAdd_Click(object sender, RoutedEventArgs e) {
            var items = ((ObservableCollection<Step>)gv_steps.ItemsSource);
            if (items == null) return;
            items.Add(new Step());
        }

        private void stepRemove_Click(object sender, RoutedEventArgs e) {
            int i = gv_steps.SelectedIndex;
            if (i == -1) return;
            var items = ((ObservableCollection<Step>)gv_steps.ItemsSource);
            items.RemoveAt(i);
        }

        private void stepUp_Click(object sender, RoutedEventArgs e) {
            var items = ((ObservableCollection<Step>)gv_steps.ItemsSource);
            if (items == null) return;
            int i = gv_steps.SelectedIndex;
            if (i <=0 ) return;
            items.Move(i, i - 1);
        }

        private void stepDown_Click(object sender, RoutedEventArgs e) {
            var items = ((ObservableCollection<Step>)gv_steps.ItemsSource);
            if (items == null) return;
            int i = gv_steps.SelectedIndex;
            if (i < 0 || i >= items.Count - 1) return;
            items.Move(i, i + 1);
        }

        private void gv_reward_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left) {
                EditReward er = new EditReward();
                var gv = (sender as DataGrid);
                if (gv == null) return;
                var quest = gv.DataContext as Quest;
                if (quest == null) return;
                int i = gv.SelectedIndex;
                er.Reward = (Reward)quest._rewards[i].Clone();
                if (er.ShowDialog() == true) {
                    quest._rewards[i] = er.Reward;
                }
            }
        }

        private void gv_steps_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left) {
                EditStep er = new EditStep();
                var gv = (sender as DataGrid);
                if (gv == null) return;
                var quest = gv.DataContext as Quest;
                if (quest == null) return;
                int i = gv.SelectedIndex;
                er.Step = (Step)quest._steps[i].Clone();
                if (er.ShowDialog() == true) {
                    quest._steps[i] = er.Step;
                }
            }
        }
    }
}
