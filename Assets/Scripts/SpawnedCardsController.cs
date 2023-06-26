using UnityEngine;

public class SpawnedCardsController : MonoBehaviour
{
    private enum OrderLayers
    {
        First = 3,
        Second = 2,
        Third = 1,
        InDeck = 0
    }

    [SerializeField] private Deck _deck;
    [SerializeField] private Transform _firstCardPosition;
    [SerializeField] private Transform _secondCardPosition;
    [SerializeField] private Transform _thirdCardPosition;

    [SerializeField] private Card[] _openedCards = new Card[3];

    public Transform FirstCardPosition => _firstCardPosition;
    public Transform SecondCardPosition => _secondCardPosition;
    public Transform ThirdCardPosition => _thirdCardPosition;

    private void OnEnable()
    {
        _deck.OnDeckClicked += OpenNewCard;
    }

    private void OnDisable()
    {
        _deck.OnDeckClicked -= OpenNewCard;
    }

    private void OpenNewCard(Card card)
    {
        if (_thirdCardPosition.childCount != 0)
        {
            SetupCard(_openedCards[2], _deck.transform, (int)OrderLayers.InDeck);
            _deck.ReturnCardToDeck(_openedCards[2]);
        }

        if (_secondCardPosition.childCount != 0)
        {
            SetupCard(_openedCards[1], _thirdCardPosition, (int)OrderLayers.Third);
            _openedCards[2] = _openedCards[1];
        }

        if (_firstCardPosition.childCount != 0)
        {
            SetupCard(_openedCards[0], _secondCardPosition, (int)OrderLayers.Second);
            _openedCards[1] = _openedCards[0];
        }

        _openedCards[0] = card;
        _openedCards[0].gameObject.SetActive(true);
        SetupCard(_openedCards[0], _firstCardPosition, (int)OrderLayers.First);
    }

    private void SetupCard(Card card, Transform targetTransform, int sortingLayer)
    {
        card.transform.position = targetTransform.position;
        card.transform.parent = targetTransform;
        card.SetSortingOrder(sortingLayer);
    }
}
