using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SCPNewView {
    [RequireComponent(typeof(Looker))]
    public class Blinker : MonoBehaviour {
        [SerializeField] private float _timeBetweenBlinks;
        [SerializeField] private float _blinkTimerVariation;

        [SerializeField] private float _blinkDuration;
        [SerializeField] private float _blinkDurationVariation;

        [SerializeField] private bool _isPlayer;


        private float _timer;
        private Looker _looker;
        private InputSettings _inputSettings;

        private void Awake() {
            _timer = _timeBetweenBlinks + Random.Range(-_blinkTimerVariation, _blinkTimerVariation);
            _looker = GetComponent<Looker>();
            if (_isPlayer) {
                _inputSettings = new InputSettings();
                _inputSettings.Player.Blink.performed += (_) => Blink();
            }
        }
        private void OnEnable() {
            if (_isPlayer) {
                _inputSettings.Enable();
            }
        }
        private void Update() {
            if (_timer <= 0) {
                Blink();
            } else {
                _timer -= Time.deltaTime;
            }
        }
        private void OnDisable() {
            if (_isPlayer) {
                _inputSettings.Disable();
            }
        }
        private async void Blink() {
            if (_isPlayer) {
                FadeBlackScreen.Instance.FadeToBlack(20f);
            }
            _timer = Mathf.Infinity;
            _looker.Enabled = false;
            await Task.Delay(Mathf.RoundToInt(_blinkDuration * 1000));
            while (!_looker.Enabled) {
                if (!_inputSettings.Player.Blink.IsPressed()) {
                    _timer = _timeBetweenBlinks + Random.Range(-_blinkTimerVariation, _blinkTimerVariation);
                    _looker.Enabled = true;
                    FadeBlackScreen.Instance.UnFade(20f);
                    return;
                } else {
                    await Task.Yield();
                }
            }
        }
        private void OnValidate() {
            _blinkTimerVariation = Mathf.Clamp(_blinkTimerVariation, 0, Mathf.Infinity);
            _blinkDurationVariation = Mathf.Clamp(_blinkDurationVariation, 0, (_timeBetweenBlinks - _blinkDuration));
            _timeBetweenBlinks = Mathf.Clamp(_timeBetweenBlinks, 0, Mathf.Infinity);
            _blinkDuration = Mathf.Clamp(_blinkDuration, 0, _timeBetweenBlinks);
        }
    }
}
