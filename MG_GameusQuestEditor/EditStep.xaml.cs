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
    /// EditReward.xaml 的交互逻辑
    /// </summary>
    public partial class EditStep : Window {

        private bool selectionInitialized = false;

        internal Step Step { get { return DataContext as Step; } set { DataContext = value; } }
        public EditStep() {
            InitializeComponent();
        }
        
        private void cb_type_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var r = D.GetItems(cb_type.SelectedValue);
            var i = cb_id.SelectedIndex;
            if (!selectionInitialized) {
                i = D.IndexOf(r, Step.id);
            }
            cb_id.ItemsSource = r;
            cb_id.SelectedIndex = i >= r.Length || i < 0 ? 0 : i;
            selectionInitialized = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Step.id = ((IdNamePair)cb_id.SelectedValue).id;
            DialogResult = true;
            Close();
        }
    }
}
