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
        [SerializeField] private float duration = 1.8f;
        private Vector3 startPosition;
        private Vector3 endPosition;

        private void Start()
        {
            startPosition = pivotCamera.transform.position;
            endPosition = startPosition;
            StopAllCoroutines();
            StartCoroutine(CheckDirectionOverTime());
        }

        private IEnumerator CheckDirectionOverTime()
        {
            yield return new WaitForSeconds(1.0f);
            startPosition = pivotCamera.transform.position;
            //Debug.Log("StartPos - x: " + startPosition.x + ", y: " + startPosition.y + ", z: " + startPosition.z);
            
            while (true)
            {
                Vector3 currentPosition = pivotCamera.transform.position;

                if (currentPosition != startPosition)
                {
                    //Debug.Log("CurrentPos - x: " + currentPosition.x + ", y: " + currentPosition.y + ", z: " + currentPosition.z);
                    //Debug.Log("StartPos - x: " + startPosition.x + ", y: " + startPosition.y + ", z: " + startPosition.z);
                    Vector3 direction = (currentPosition - startPosition).normalized;
                    //Debug.Log("Direction - x: " + direction.x + ", y: " + direction.y + ", z: " + direction.z);

                    if(direction.x > 0)
                    {
                        endPosition = new Vector3(nodeField.rectTransform.localPosition.x - 100, nodeField.rectTransform.localPosition.y - 40, 0);
                    }
                    else if (direction.z > 0)
                    {
                        endPosition = new Vector3(nodeField.rectTransform.localPosition.x + 100, nodeField.rectTransform.localPosition.y - 60, 0);
                    }
                    else if (direction.x < 0)
                    {
                        endPosition = new Vector3(nodeField.rectTransform.localPosition.x + 100, nodeField.rectTransform.localPosition.y + 40, 0);
                    }
                    else if (direction.z < 0)
                    {
                        endPosition = new Vector3(nodeField.rectTransform.localPosition.x - 100, nodeField.rectTransform.localPosition.y + 60, 0);
                    }
                    break;
                }

                yield return new WaitForSeconds(0.01f);
            }
            StartCoroutine(TranslateCoroutine());
            yield return null;
        }
        private IEnumerator TranslateCoroutine()
        {
            float elapsedTime = 0f;

            Vector3 startPosition = nodeField.rectTransform.localPosition;

            while (elapsedTime < duration)
            {
                nodeField.rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            nodeField.rectTransform.localPosition = endPosition;

            //Debug.Log("Translation completed");
            
            yield return CheckDirectionOverTime();
        }
    }
}
