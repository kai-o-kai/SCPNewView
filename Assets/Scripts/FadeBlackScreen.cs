using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SCPNewView {
    public class FadeBlackScreen : MonoBehaviour {
        public static FadeBlackScreen Instance { get {
                if (s_instance == null) {
                    GameObject canvasGO = new GameObject("Canvas");
                    Canvas canvas = canvasGO.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    
                    CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
                    GraphicRaycaster raycaster = canvasGO.AddComponent<GraphicRaycaster>();

                    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.referenceResolution = new Vector2(1920, 1080);

                    GameObject fadeBlackGO = new GameObject("FadeBlack");
                    fadeBlackGO.transform.parent = canvasGO.transform;
                    fadeBlackGO.AddComponent<CanvasRenderer>();
                    RectTransform fadeBlackRectTrans = fadeBlackGO.AddComponent<RectTransform>();
                    fadeBlackRectTrans.anchorMin = Vector2.zero;
                    fadeBlackRectTrans.anchorMax = Vector2.one;

                    Image fadeBlackImg = fadeBlackGO.AddComponent<Image>();
                    fadeBlackImg.color = new Color(0, 0, 0, 0);

                    s_instance = fadeBlackGO.AddComponent<FadeBlackScreen>();
                }
                return s_instance;
            }
        }
        private static FadeBlackScreen s_instance;

        private bool _isFaded;
        private Image _img;
        private Coroutine _fadeUnfadeRoutine;
        
        private void Awake() {
            DontDestroyOnLoad(transform.parent);
            s_instance = this;
            _img = GetComponent<Image>();
        }
        public void FadeToBlack(float fadeSpeed) {
            if (_isFaded) return;

            if (_fadeUnfadeRoutine != null) {
                StopCoroutine(_fadeUnfadeRoutine);
                _fadeUnfadeRoutine = null;
            }
            _fadeUnfadeRoutine = StartCoroutine(C_FadeBlack(fadeSpeed));
        }
        private IEnumerator C_FadeBlack(float fadeSpeed) {
            _isFaded = true;
            StopCoroutine(nameof(C_UnFade));
            while (_img.color.a < 0.9f) {
                _img.color = Color.Lerp(_img.color, Color.black, Time.deltaTime * fadeSpeed);
                yield return new WaitForEndOfFrame();
            }
            _img.color = Color.black;
            yield break;
        }
        public void UnFade(float fadeSpeed) {
            if (!_isFaded) return;

            if (_fadeUnfadeRoutine != null) {
                StopCoroutine(_fadeUnfadeRoutine);
                _fadeUnfadeRoutine = null;
            }
            _fadeUnfadeRoutine = StartCoroutine(C_UnFade(fadeSpeed));
        }
        private IEnumerator C_UnFade(float fadeSpeed) {
            _isFaded = false;
            StopCoroutine(nameof(C_FadeBlack));
            while (_img.color.a > 0.1f) {
                _img.color = Color.Lerp(_img.color, Color.clear, Time.deltaTime * fadeSpeed);
                yield return new WaitForEndOfFrame();
            }
            _img.color = Color.clear;
            yield break;
        }
    }
}
