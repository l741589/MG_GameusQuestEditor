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

        private Data data = new Data();
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        public MainWindow() {
            InitializeComponent();
            data = (Data)Resources["data"];
            Loaded += MainWindow_Loaded;
        }

        private void AddAll<T>(Collection<T> dest, IEnumerable<T> src) {
            foreach (T e in src) dest.Add(e);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            try {
                String text = File.ReadAllText(App.Path + "data/Quests.json");
                var obj=jss.Deserialize<Object[]>(text);
                var cates=jss.ConvertToType<ObservableCollection<String>>(obj[0]);
                AddAll(data.Category, cates.Select(s=>new StringWrapper(s)));
                var quests = jss.ConvertToType<ObservableCollection<Quest>>(obj.Skip(1).ToArray());
                AddAll(data.Quests, quests.Select(q => q.Init()));
                Console.WriteLine();
            } catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show("please place this file in your project folder");
                Close();
            }
        }

        private void addCate_Click(object sender, RoutedEventArgs e) {
            data.Category.Add("<新类别>");
            int i = data.Category.Count;
            cateList.SelectedIndex = i - 1;            
        }

        private void removeCate_Click(object sender, RoutedEventArgs e) {
            if (cateList.SelectedIndex>=0) data.Category.RemoveAt(cateList.SelectedIndex);
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

        private void doDoubleClick(object sender) {
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
                doDoubleClick(sender);
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
    }
}
