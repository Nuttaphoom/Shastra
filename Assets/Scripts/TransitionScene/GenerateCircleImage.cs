using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class GenerateCircleImage : MonoBehaviour
    {
        private Canvas _canvas;
        [SerializeField]
        private Image _blackScreen;

        private Vector2 _playerCanvasPos;

        private static readonly int RADIUS = Shader.PropertyToID("_Radius");
        private static readonly int CENTER_X = Shader.PropertyToID("_CenterX");
        private static readonly int CENTER_Y = Shader.PropertyToID("_CenterY");

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _blackScreen = GetComponentInChildren<Image>();
        }

        private void Start()
        {
            DrawBlackScreen();
        }

        [ContextMenu("Out")]
        public void OpenBlackScreen()
        {
            DrawBlackScreen();
            StartCoroutine(Transition(1f, 0, 1));
        }
        [ContextMenu("In")]
        public void CloseBlackScreen()
        {
            DrawBlackScreen();
            StartCoroutine(Transition(1f, 1, 0));
        }

        private void DrawBlackScreen()
        {
            var canvasRect = _canvas.GetComponent<RectTransform>().rect;
            var canvasWidth = canvasRect.width;
            var canvasHeight = canvasRect.height;

            var squareValue = 0f;
            if (canvasWidth > canvasHeight)
            {
                // Landscape
                squareValue = canvasWidth;
                //_playerCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
            }
            else
            {
                // Portrait            
                squareValue = canvasHeight;
                //_playerCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
            }

            _blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);

        }

        private IEnumerator Transition(float duration, float beginRadius, float endRadius)
        {
            var mat = _blackScreen.material;
            var time = 0f;
            while (time <= duration)
            {
                time += Time.deltaTime;
                var t = time / duration;
                var radius = Mathf.Lerp(beginRadius, endRadius, t);

                mat.SetFloat(RADIUS, radius);

                yield return null;
            }
        }
    }
}
