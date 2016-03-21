using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Data;

namespace MG_GameusQuestEditor {
    class StringWrapper : BaseData{
        public String String { get { return _String; } set { _String = value; _PC("String"); } } private String _String = default(String);

        public StringWrapper(String val){
            this.String=val;
        }

        public static implicit operator StringWrapper(String val) {
            return new StringWrapper(val);
        }

        public static implicit operator String(StringWrapper val) {
            return val.String;
        }

        public override string ToString() {
            return String;
        }
    }
}