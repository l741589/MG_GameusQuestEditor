using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MG_GameusQuestEditor {

    class BaseData : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void _PC(String propertyName) {
            if (PropertyChanged!=null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    enum TrackableType {
        item,
        weapon,
        armor,
        gold,
        custom,
        variable,
        //Switch
        //kill,
    }

    enum RewardType {
        item,
        weapon,
        armor,
        gold,
        xp,
        custom,
    }

    class Category : BaseData {
        public String Name { get { return _Name; } set { _Name = value; _PC("Name"); } } private String _Name = default(String);

        public Category() {
            Name = "<NewCategory>";
        }

        public Category(String val) {
            this.Name = val;
        }

        public static implicit operator Category(String val) {
            return new Category(val);
        }

        public static implicit operator String(Category val) {
            return val.Name;
        }

        public override string ToString() {
            return Name;
        }
    }

    class Step : BaseData, ICloneable {

        public String desc { get { return _desc; } set { this._desc = value; _PC("desc"); } }
        private String _desc = "";

        public TrackableType type { get { return _type; } set { this._type = value; _PC("type"); } }
        private TrackableType _type;

        public int id { get { return _id; } set { this._id = value; _PC("id"); } }
        private int _id = 1;

        public String code { get { return _code; } set { this._code = value; _PC("code"); } }
        private String _code = "";

        public int amount { get { return _amount; } set { this._amount = value; _PC("amount"); } }
        private int _amount = 1;

        public bool showProgress { get { return _showProgress; } set { this._showProgress = value; _PC("showProgress"); } }
        private bool _showProgress = true;

        public bool percentage { get { return _percentage; } set { this._percentage = value; _PC("percentage"); } }
        private bool _percentage = false;

        public object Clone() {
            return MemberwiseClone();
        }
    }

    class Reward : BaseData,ICloneable {

        public String desc { get { return _desc; } set { this._desc = value; _PC("desc"); } }
        private String _desc = "";

        public RewardType type { get { return _type; } set { this._type = value; _PC("type"); } }
        private RewardType _type;

        public int amount { get { return _amount; } set { this._amount = value; _PC("amount"); } }
        private int _amount = 1;


        public int id { get { return _id; } set { this._id = value; _PC("id"); } }
        private int _id = 1;

        public bool hidden { get { return _hidden; } set { this._hidden = value; _PC("hidden"); } }
        private bool _hidden = false;

        public String code { get { return _code; } set { _code = value; _PC("code"); } } private String _code = "";

        /////////////////////////////////////////////////

        public object Clone() {
            return MemberwiseClone();
        }
    }

    class Quest :BaseData{
        public int id { get { return _id; } set { _id = value; _PC("id"); _PC("DisplayName"); } } private int _id = default(int);

        public int icon { get { return _icon; } set { this._icon = value; _PC("icon"); } }
        private int _icon = 1;

        public int cat { get { return _cat; } set { this._cat = value; _PC("cat"); } }
        private int _cat = 0;


        public String name { get { return _name; } set { this._name = value; _PC("name"); _PC("DisplayName"); } }
        private String _name = "<NewQuest>";

        public String desc { get { return _desc; } set { this._desc = value; _PC("desc"); } }
        private String _desc = "<Description>";

        public bool autoComplete { get { return _autoComplete; } set { _autoComplete = value; _PC("autoComplete"); } } private bool _autoComplete = default(bool);
        
        [ScriptIgnore]
        public String DisplayName { get { return String.Format("{0:0000}: {1}", id, name); } }

        public object[][] steps { get; set; }
        public object[][] rewards { get; set; }

        public ObservableCollection<Step> _steps { get; set; }
        public ObservableCollection<Reward> _rewards { get; set; }

        public Quest() {
            for (int i = 1; i < 9999; ++i) {
                if (D.Data.Quests.Any(q => q._id == i)) continue;
                id = i;
                break;
            }
        }

        public Quest Init() {
            if (_steps == null) {
                _steps = new ObservableCollection<Step>();
                if (steps != null) {
                    foreach (var e in steps) {
                        Step step = new Step();
                        step.desc = e[0] + "";
                        bool b;
                        int x;
                        if (bool.TryParse(e[1] + "", out b)) step.showProgress = b;
                        step.type = TrackableType.variable;
                        if (int.TryParse(e[2] + "", out x)) step.id = x;
                        if (int.TryParse(e[3] + "", out x)) step.amount = x;
                        if (bool.TryParse(e[4] + "", out b)) step.percentage = b;
                        step.code = "";
                        _steps.Add(step);
                    }
                }
            }
            if (_rewards == null) {
                _rewards = new ObservableCollection<Reward>();
                if (rewards != null) {
                    foreach (var e in rewards) {
                        Reward r = new Reward();
                        r.type = (RewardType)Enum.Parse(typeof(RewardType), e[0] + "", true);
                        int x;
                        bool b;
                        if (bool.TryParse(e[3] + "", out b)) r.hidden = b;
                        if (r.type == RewardType.custom) {
                            r.desc = e[1] + "";
                            r.amount = 0;
                            r.id = 0;
                        } else if (r.type == RewardType.item || r.type == RewardType.armor || r.type == RewardType.weapon) {
                            if (int.TryParse(e[1] + "", out x)) r.id = x;
                            if (int.TryParse(e[2] + "", out x)) r.amount = x;
                            r.desc = "";
                        } else {
                            r.desc = "";
                            r.id = 0;
                            if (int.TryParse(e[1] + "", out x)) r.amount = x;
                        }
                        _rewards.Add(r);
                    }
                }
            }
            return this;
        }

        public Quest Update() {
            rewards = _rewards.Select(e=>{
                switch (e.type) {
                case RewardType.armor:
                case RewardType.item:
                case RewardType.weapon:
                    return new object[] { e.type.ToString(), e.id, e.amount, e.hidden };
                case RewardType.gold:
                case RewardType.xp:
                    return new object[] { e.type.ToString(), e.amount, 1, e.hidden };
                default:
                    return new object[] { e.type.ToString(), e.desc, 1, e.hidden };
                }
            }).ToArray();

            steps = (from e in _steps where e.type == TrackableType.variable select new object[] { e.desc, e.showProgress, e.id, e.amount, e.percentage }).ToArray();
            return this;
        }
    }

    class Data {
        public ObservableCollection<Category> Category { get; set; }
        public ObservableCollection<Quest> Quests { get; set; }
        public Data() {
            Category = new ObservableCollection<Category>();
            Quests = new ObservableCollection<Quest>();
        }
    }
}
