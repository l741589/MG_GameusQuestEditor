using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        xp,
        custom,
    }

    class Step : BaseData {
        public String name { get { return _name; } set { this._name = value; _PC("name"); } }
        private String _name;

        public bool showProgress { get { return _showProgress; } set { this._showProgress = value; _PC("showProgress"); } }
        private bool _showProgress;

        public TrackableType trackType { get { return _trackType; } set { this._trackType = value; _PC("trackType"); } }
        private TrackableType _trackType;

        public int trackId { get { return _trackId; } set { this._trackId = value; _PC("trackId"); } }
        private int _trackId;

        public String code { get { return _code; } set { this._code = value; _PC("code"); } }
        private String _code;

        public int maxValue { get { return _maxValue; } set { this._maxValue = value; _PC("maxValue"); } }
        private int _maxValue;

        public bool percentage { get { return _percentage; } set { this._percentage = value; _PC("percentage"); } }
        private bool _percentage;
    }

    class Reward : BaseData {

        public TrackableType type { get { return _type; } set { this._type = value; _PC("type"); } }
        private TrackableType _type;

        public int amount { get { return _amount; } set { this._amount = value; _PC("amount"); } }
        private int _amount;


        public int id { get { return _id; } set { this._id = value; _PC("id"); } }
        private int _id;

        public String desc { get { return _desc; } set { this._desc = value; _PC("desc"); } }
        private String _desc;

        public bool hidden { get { return _hidden; } set { this._hidden = value; _PC("hidden"); } }
        private bool _hidden;
    }

    class Quest :BaseData{
        public int id { get; set; }

        public int icon { get { return _icon; } set { this._icon = value; _PC("icon"); } }
        private int _icon;

        public int cat { get { return _cat; } set { this._cat = value; _PC("cat"); } }
        private int _cat;


        public String name { get { return _name; } set { this._name = value; _PC("name"); } }
        private String _name;

        public String desc { get { return _desc; } set { this._desc = value; _PC("desc"); } }
        private String _desc;

        public object[][] steps { get; set; }
        public object[][] rewards { get; set; }

        public ObservableCollection<Step> _steps { get; set; }
        public ObservableCollection<Quest> _rewards { get; set; }

        public Quest() {

            _steps = new ObservableCollection<Step>();
            _rewards = new ObservableCollection<Quest>();
        }
    }

    class Data {
        public ObservableCollection<StringWrapper> Category { get; set; }
        public ObservableCollection<Quest> Quests { get; set; }
        public Data() {
            Category = new ObservableCollection<StringWrapper>();
            Quests = new ObservableCollection<Quest>();
        }
    }
}
