using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CardManager : MonoBehaviour
{
    //Card that is in hands of player
    private List<Card> _activatedCard = new List<Card>() ;
    private int _currentActiveCardIndex = -1;
    private bool _isSelectCard;
    private CardEffectFlowControl _cardEffectFlowControl = new CardEffectFlowControl(); 
    [SerializeField]
    private Deck _deck ;

    [SerializeField]
    private GameObject _cardPrefab ;

    [SerializeField]
    private RectTransform _cardPanel ;

    [SerializeField]
    private RectTransform _firstCardPos; 


    [Header("listen to")]
    [SerializeField]
    private CardEventChannel OnHoldingCardEvent;
    [SerializeField]
    private CardEventChannel OnReleaseCardEvent;
    [SerializeField]
    private CardEventChannel OnPointerEnterCardEvent;
    [SerializeField]
    private CardEventChannel OnPointerExitCardEvent;



    private void OnEnable()
    {
        OnHoldingCardEvent.OnEvent += SelectCard;
        OnReleaseCardEvent.OnEvent += ReleaseCard;
        OnPointerEnterCardEvent.OnEvent += PullUpCard;
        OnPointerExitCardEvent.OnEvent += PullDownCard;
    }

    private void OnDisable()
    {
        OnHoldingCardEvent.OnEvent -= SelectCard;
        OnReleaseCardEvent.OnEvent -= ReleaseCard;
        OnPointerEnterCardEvent.OnEvent -= PullUpCard;
        OnPointerExitCardEvent.OnEvent -= PullDownCard;
    }
    //Using card data to init physical card 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawCardDataFromDeck();
        }
    }

    #region EventListener
    private void PullUpCard(Card card)
    {
        if (_isSelectCard)
            return;  

        for (int i = 0; i < _activatedCard.Count; i++)
        {
            if (card == _activatedCard[i])
            {
                TranslateCard(i, new Vector3(0, 50.0f, 0) );
                return;
            }
        }
    }

    private void PullDownCard(Card card)
    {
        for (int i = 0; i < _activatedCard.Count; i++)
        {
            if (card == _activatedCard[i])
            {
                TranslateCard(i, new Vector3(0, 0, 0)) ;
                return; 
            }
        }
    }
    private void ReleaseCard(Card card)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool _applied = false ;

        List<Component> targets = new List<Component>();

        if (Physics.Raycast(ray, out hit) )
        {
            //1.)Check if card is release on tile (mosty for building related cards) 
            if (hit.collider.TryGetComponent(out Tile tile))
            {
                targets.Add(tile); 
                StartCoroutine(_cardEffectFlowControl.RunCardEffectFlowCoroutine(this,card, targets) ) ; 
            }
            //or collider with playground (mostly for universal usage card) 
            else if (hit.collider.TryGetComponent(out PlaygroundBoard playground))
            {

            } 
        }

        PullEveryCardBack(); 

        _currentActiveCardIndex = -1;
        _isSelectCard = false; 

    }

    private void DragCard(Card _card,Vector2 _dragAmount ) 
    {
        if (! _isSelectCard)
            return;

        _activatedCard[_currentActiveCardIndex].transform.position = Input.mousePosition;

        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //TODO Check if it collide with playfield
        if (Physics.Raycast(ray,out hit))
        {
             
        }
    }
    private void SelectCard(Card _card)
    {
        if (_isSelectCard)
            return;

        for (int i = 0; i < _activatedCard.Count; i ++)
        {
            if (_activatedCard[i] == _card)
            {
                _currentActiveCardIndex = i;
                _activatedCard[i].GetComponent<Card>().OnTogglePointerInteraction(true);
                _isSelectCard = true;
                _activatedCard[i].GetComponent<RectTransform>().SetAsLastSibling();

            }
            else
            {
                _activatedCard[i].GetComponent<Card>().OnTogglePointerInteraction(false); 
            }
        }
    }

    #endregion
    
    private void TranslateCard(int cardIndex, Vector3 adjustPos  )
    {
        float cardWidth = _activatedCard[cardIndex].GetComponent<RectTransform>().sizeDelta.x * _activatedCard[cardIndex].GetComponent<RectTransform>().localScale.x; 
        Vector3 targetPos = new Vector3(_firstCardPos.position.x + cardIndex * cardWidth, _firstCardPos.position.y, _firstCardPos.position.z) + adjustPos;

        StartCoroutine(_activatedCard[cardIndex].GetComponent<CardVisual>().TranslateCard(targetPos)) ;
        
    }


    public void DrawCardDataFromDeck()
    {
        BaseCardSO _baseCardSO = _deck.DrawCard();
        GameObject newObj = Instantiate(_cardPrefab, _cardPanel.transform);
        newObj.GetComponent<Card>().CardInit(_baseCardSO);
        newObj.GetComponent<Card>().OnDragCardEvent += DragCard ;
        newObj.transform.position = Vector3.zero; 

        _activatedCard.Add(newObj.GetComponent<Card>());

        TranslateCard(_activatedCard.Count - 1, new Vector3(0, 0, 0)) ; 
    }

    public void DiscardCard(Card card)
    {
        for (int i =0; i < _activatedCard.Count ;i++)
        {
            if (_activatedCard[i] == card)
            {
                Destroy(_activatedCard[i].gameObject); 
                _activatedCard.RemoveAt(i);
                PullEveryCardBack(); 
                return; 
            }
        }
    }

    private void PullEveryCardBack()
    {
        for (int i = 0; i < _activatedCard.Count; i++)
        {
            _activatedCard[i].GetComponent<Card>().OnTogglePointerInteraction(true);
            TranslateCard(i, new Vector3(0, 0, 0));
        }
    }
}
