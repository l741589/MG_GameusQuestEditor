using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MG_GameusQuestEditor {

    class IdNamePair{

        public int id { get; set; }
        public string name { get; set; }

        public override string ToString() {
            return String.Format("{0:0000}: {1}", id, name);
        }
    }

    static class D {
        private static JavaScriptSerializer jss = new JavaScriptSerializer();
        public static Data Data;
        public static IdNamePair[] Items;
        public static IdNamePair[] Weapons;
        public static IdNamePair[] Armors;
        public static IdNamePair[] Variables;
        public static IdNamePair[] Switches;
        public static IdNamePair N_A = new IdNamePair { id = 0, name = "N/A" };
        public static ImageSource IconSet;
        public static string IconSetFile;

        private class SystemFile {
            public string[] variables;
            public string[] switches;
        }


        private static void AddAll<T>(Collection<T> dest, IEnumerable<T> src) {
            foreach (T e in src) dest.Add(e);
        }

        private static String T(String name) {
            String path = App.Path + "data/" + name + ".json";
            if (File.Exists(path)) return File.ReadAllText(path);
            return null;
        }

        public static void Init(Data data) {
            Data = data;
            String text = T("Quests");
            if (text != null) {
                var obj = jss.Deserialize<Object[]>(text);
                var cates = jss.ConvertToType<ObservableCollection<String>>(obj[0]);
                AddAll(D.Data.Category, cates.Select(s => new Category(s)));
                var quests = jss.ConvertToType<ObservableCollection<Quest>>(obj.Skip(1).ToArray());
                AddAll(D.Data.Quests, quests.Select(q => q.Init()));
            } else {
                Data.Category.Add("category1");
                Data.Category.Add("category2");
            }
            Items = jss.Deserialize<IdNamePair[]>(T("Items")).Select(v => v == null ? N_A : v).Skip(1).ToArray();
            Weapons = jss.Deserialize<IdNamePair[]>(T("Weapons")).Select(v => v == null ? N_A : v).Skip(1).ToArray();
            Armors = jss.Deserialize<IdNamePair[]>(T("Armors")).Select(v => v == null ? N_A : v).Skip(1).ToArray();

            SystemFile sf = jss.Deserialize<SystemFile>(T("System"));
            Variables = sf.variables.Select((v, i) => v == null ? N_A : new IdNamePair { id = i, name = v }).Skip(1).ToArray();
            Switches = sf.switches.Select((v, i) => v == null ? N_A : new IdNamePair { id = i, name = v }).Skip(1).ToArray();
            IconSetFile=App.Path + "img/system/IconSet.png";
            IconSet = new BitmapImage(new Uri(IconSetFile));
        }

        public static IdNamePair[] GetItems(object value) {
            if (value is RewardType) {
                switch ((RewardType)value) {
                case RewardType.item: return D.Items;
                case RewardType.armor: return D.Armors;
                case RewardType.weapon: return D.Weapons;
                default:return new IdNamePair[] { D.N_A };
                }
            } else if (value is TrackableType) {
                switch ((TrackableType)value) {
                case TrackableType.item: return D.Items;
                case TrackableType.armor: return D.Armors;
                case TrackableType.weapon: return D.Weapons;
                case TrackableType.variable: return D.Variables;
                default: return new IdNamePair[] { D.N_A };
                }
            }
            return new IdNamePair[] { D.N_A };
        }

        public static int IndexOf(IdNamePair[] ps, int id) {
            int len = ps.Length;
            for (int i = 0; i < len; ++i) if (ps[i].id == id) return i;
            return 0;
        }

        public static void Save(){
            
            LinkedList<object> o = new LinkedList<object>();
            o.AddLast(Data.Category.Select(s => s.Name).ToArray());
            foreach (var e in Data.Quests) o.AddLast(e.Update());
            Backup();
            File.WriteAllText(App.Path + "data/Quests.json", jss.Serialize(o));
        }

        public static void Backup() {
            if (!File.Exists(App.Path + "BackupData")) Directory.CreateDirectory(App.Path + "BackupData");
            string o = App.Path + "BackupData/Quests.bak." + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".json";
            if (!File.Exists(o)) File.Copy(App.Path + "data/Quests.json", o);
        }
    }
}
