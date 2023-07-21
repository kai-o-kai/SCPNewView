using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView.Utils {
    public partial class Timer {
        private static List<Timer> s_activeTimers = new List<Timer>();

        public static void RemoveTimersWithCallback(Action callbackToMatch) {
            Timer[] timersToDisable = s_activeTimers.FindAll(cbk => cbk.TimerCompleteCallback == callbackToMatch).ToArray();
            foreach (Timer toDisable in timersToDisable) {
                toDisable.TimerIsActive = false;
            }
        }
    }
}