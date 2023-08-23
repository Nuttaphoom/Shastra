using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardVisual : MonoBehaviour
{

    [Header("Card Visual")]
    [SerializeField]
    private Image _imageRenderer;

     
 
    private void OnEnable()
    {
 
        GetComponent<Card>().TogglePointerInteractionEvent += SetCardVisualActive; 


    }

    private void OnDisable()
    {
 
        GetComponent<Card>().TogglePointerInteractionEvent -= SetCardVisualActive;


    }
    #region EventListener
 

    private void SetCardVisualActive(bool active)
    {
        if (active)
        {
            EnableRenderer(); 
        }
        else
        {
            DisableRenderer();
        }
   
    }
    #endregion
    
    public IEnumerator TranslateCard (Vector3 targetPos)
    {
        
        Vector2 velocity = (targetPos - GetComponent<RectTransform>().transform.position);

        float timeNeeded = 0.2f; //half a second
        float speed = velocity.magnitude / timeNeeded; //half a second 

        while (timeNeeded > 0)
        {
            timeNeeded -= 1 * Time.deltaTime; 
            velocity = (targetPos - GetComponent<RectTransform>().transform.position);

            //Dont need to normalized  since we want speed to varry 
            GetComponent<RectTransform>().transform.Translate(velocity.normalized * speed * Time.deltaTime);

            yield return null;
        }

        GetComponent<RectTransform>().transform.position = targetPos;
    }

    private void EnableRenderer()
    {
        _imageRenderer.color = new Color(_imageRenderer.color.r, _imageRenderer.color.g, _imageRenderer.color.b, 1.0f); 
    }

    private void DisableRenderer()
    {
        _imageRenderer.color = new Color(_imageRenderer.color.r, _imageRenderer.color.g, _imageRenderer.color.b, 0);
    }
}
