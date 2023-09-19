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
        
        private void Awake() {
            DontDestroyOnLoad(transform.parent);
            _img = GetComponent<Image>();
        }
        public void FadeToBlack(float fadeTimeSeconds) {
            if (_isFaded) return;
            StartCoroutine(C_FadeBlack(fadeTimeSeconds));
        }
        private IEnumerator C_FadeBlack(float fadeTimeSeconds) {
            float lerpValue = 0;
            while (_img.color .a < 1) {
                _img.color = Color.Lerp(_img.color, Color.black, Time.deltaTime * fadeTimeSeconds);
                yield return new WaitForEndOfFrame();
            } 
            
            _isFaded = true;
        }
    }
}
