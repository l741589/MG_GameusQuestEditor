using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MG_GameusQuestEditor {
    /// <summary>
    /// TableControler.xaml 的交互逻辑
    /// </summary>
    public partial class TableControler : UserControl {
        public TableControler() {
            InitializeComponent();
        }


        public static DependencyProperty ForProperty = DependencyProperty.Register("For", typeof(Selector), typeof(TableControler));

        [System.ComponentModel.Description("For")]
        [System.ComponentModel.Category("For Category")]
        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Selector For {
            get {
                return ((Selector)(base.GetValue(TableControler.ForProperty)));
            }
            set {
                base.SetValue(TableControler.ForProperty, value);
            }
        }


        public static DependencyProperty ElementTypeProperty = DependencyProperty.Register("ElementType", typeof(string), typeof(TableControler));

        [System.ComponentModel.Description("ElementType")]
        [System.ComponentModel.Category("ElementType Category")]
        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public string ElementType {
            get {
                return ((string)(base.GetValue(TableControler.ElementTypeProperty)));
            }
            set {
                base.SetValue(TableControler.ElementTypeProperty, value);
            }
        }

        private Type type;

        private void add_Click(object sender, RoutedEventArgs e) {
            dynamic items = For.ItemsSource;
            if (items == null) return;
            if (type == null) type = Type.GetType(ElementType);
            dynamic elem = type.GetConstructor(new Type[0]).Invoke(new object[0]);
            items.Add(elem);
        }

        private void remove_Click(object sender, RoutedEventArgs e) {
            dynamic items = For.ItemsSource;
            if (items == null) return;
            int i = For.SelectedIndex;
            if (i == -1) return;            
            items.RemoveAt(i);
        }

        private void up_Click(object sender, RoutedEventArgs e) {
            dynamic items = For.ItemsSource;
            if (items == null) return;
            int i = For.SelectedIndex;
            if (i <= 0) return;
            items.Move(i, i - 1);
        }

        private void down_Click(object sender, RoutedEventArgs e) {
            dynamic items = For.ItemsSource;
            if (items == null) return;
            int i = For.SelectedIndex;
            if (i < 0 || i >= items.Count - 1) return;
            items.Move(i, i + 1);
        }
    }
}
