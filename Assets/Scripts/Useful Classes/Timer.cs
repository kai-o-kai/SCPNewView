using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SCPNewView.Utils {
    public partial class Timer {
        public float SecondsLeft { get => _currentDurSeconds; }
        public bool TimerIsActive { get => _timerIsActive; set {
                if (value == false) {
                    _timerCancelToken.Cancel();
                    s_activeTimers.Remove(this);
                }
                _timerIsActive = value;
            }
        }
        public Action TimerCompleteCallback => _onTimerComplete;

        private bool _timerIsActive;
        private Action _onTimerComplete;
        private float _currentDurSeconds;
        private CancellationTokenSource _timerCancelToken;

        public Timer(Action onTimerComplete, float durInSeconds) {
            if (durInSeconds < 0) { throw new ArgumentOutOfRangeException(nameof(durInSeconds), durInSeconds, "Duration cannot be less than 0."); }
            if (durInSeconds % 0.5 != 0) { throw new ArgumentException("Timer duration must be multiples of 0.5f (eg. 1.5f, 1f, 3f, 2.5f)", nameof(durInSeconds)); }
            _onTimerComplete = onTimerComplete;
            _currentDurSeconds = durInSeconds;
            s_activeTimers.Add(this);
            RunTimer();
        }
        private async void RunTimer(CancellationTokenSource cancelToken = default) {
            while (_currentDurSeconds > 0) {
                _currentDurSeconds -= 0.5f;
                await Task.Delay(500, cancelToken.Token);
            }
            _onTimerComplete?.Invoke();
        }
    }

}