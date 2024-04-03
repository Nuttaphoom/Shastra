using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class MissionCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject pivotCamera;
        [SerializeField] private Image nodeField;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private CameraTranslateDirection dir;

        private void Start()
        {
            startPosition = pivotCamera.transform.position;
            StartCoroutine(CheckDirectionOverTime());
            Debug.Log("Start Position - x: " + startPosition.x + ", y: " + startPosition.y + ", z: " + startPosition.z);
        }

        private IEnumerator CheckDirectionOverTime()
        {
            while (true)
            {
                Vector3 currentPosition = pivotCamera.transform.position;

                if (currentPosition != startPosition)
                {
                    Vector3 direction = (currentPosition - startPosition).normalized;
                    Debug.Log("Direction - x: " + direction.x + ", y: " + direction.y + ", z: " + direction.z);

                    if(direction.x > 0)
                    {
                        dir = CameraTranslateDirection.Forward;
                    }
                    break;
                }

                yield return new WaitForSeconds(0.1f);
            }
            TranslateMinimapNodeField();
            yield return null;
        }
        private void TranslateMinimapNodeField()
        {
            Debug.Log("Start Translate");
            endPosition = new Vector3(nodeField.rectTransform.localPosition.x - 100, nodeField.rectTransform.localPosition.y - 40, nodeField.rectTransform.localPosition.z);
            Debug.Log("End Position - x: " + endPosition.x + ", y: " + endPosition.y + ", z: " + endPosition.z);
            StartCoroutine(TranslateCoroutine());
        }

        private IEnumerator TranslateCoroutine()
        {
            float elapsedTime = 0f;
            float duration = 1.5f;

            Vector3 startPosition = nodeField.rectTransform.localPosition;

            while (elapsedTime < duration)
            {
                nodeField.rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            nodeField.rectTransform.localPosition = endPosition;

            Debug.Log("Translation completed");
        }

        public CameraTranslateDirection CalculateCamearaTranslateDirection()
        {
            
            //pivotCamera
            return dir;
        }
    }

    public enum CameraTranslateDirection
    {
        Forward,
        BackWard,
        Left,
        Right
    }
}
