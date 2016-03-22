using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MG_GameusQuestEditor {
    public class DataPro {
        public static int[] GetNumbers() {
            int[] r = new int[10];
            for (int i = 0; i < 10; ++i) {
                r[i] = i;
            }
            return r;
        }
    }
}
