using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SCPNewView.Utils {
    public partial class Timer {
        public float SecondsLeft { get => _currentDurSeconds; }
        public bool TimerIsActive { get => _timerIsActive; set {
                if (value == false) {
                    s_activeTimers.Remove(this);
                }
                _timerIsActive = value;
            }
        }
        public Action TimerCompleteCallback => _onTimerComplete;

        private bool _timerIsActive = true;
        private Action _onTimerComplete;
        private float _currentDurSeconds;

        public Timer(Action onTimerComplete, float durInSeconds) {
            if (durInSeconds < 0) { throw new ArgumentOutOfRangeException(nameof(durInSeconds), durInSeconds, "Duration cannot be less than 0."); }
            _onTimerComplete = onTimerComplete;
            _currentDurSeconds = durInSeconds;
            s_activeTimers.Add(this);
            RunTimer();
        }
        private async void RunTimer(CancellationTokenSource cancelToken = default) {
            while (_currentDurSeconds > 0) {
                await Task.Yield();
                _currentDurSeconds -= Time.deltaTime;
            }
            if (_timerIsActive) _onTimerComplete?.Invoke();
        }
    }

}