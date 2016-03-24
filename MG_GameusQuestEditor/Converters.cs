using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MG_GameusQuestEditor {
    class VisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value is bool) return (bool)value == true ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    class ItemWidthConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (double)value - 8;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    class EnumValuesConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value is String) {
                return Enum.GetValues(Type.GetType(value + ""));
            } else {
                return Enum.GetValues(value.GetType());
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    class ItemsIdConverter : IMultiValueConverter {


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (values[0] is int) {
                var id = (int)values[0];
                if (id < 0) return D.N_A;
                IdNamePair[] ps = D.GetItems(values[1]);
                int i = D.IndexOf(ps, id);
                return ps[i].ToString();
            }
            return D.N_A;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    class StrEscapeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) return "";
            var s = value.ToString();
            return s.Replace("\r", "\\r").Replace("\n", "\\n");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    class IconClipConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int width = 32;
            int x = (int)value;
            var linecount = (int)D.IconSet.Width / width;
            var rect = new Rectangle(x % linecount * width, x / linecount * width, width, width);
            System.Drawing.Image img=System.Drawing.Image.FromFile(D.IconSetFile);
            Bitmap ob=new Bitmap(width,width);
            Graphics g = Graphics.FromImage(ob);
            g.DrawImage(img, new Rectangle(0, 0, width, width), rect, GraphicsUnit.Pixel);
            MemoryStream stream=new MemoryStream();
            ob.Save(stream,ImageFormat.Png);
            BitmapImage bitmapImage = new BitmapImage();  
            bitmapImage.BeginInit();  
            bitmapImage.StreamSource = stream;  
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
