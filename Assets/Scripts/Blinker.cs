using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPNewView {
    [RequireComponent(typeof(Looker))]
    public class Blinker : MonoBehaviour {
        [SerializeField] private float _timeBetweenBlinks; 

        private float _timer;

        private void Awake() {
            _timer = _timeBetweenBlinks;
        }
        private void Update() {
            
        }
        public void Blink() {
            _timer = _timeBetweenBlinks;
        }
    }
}
