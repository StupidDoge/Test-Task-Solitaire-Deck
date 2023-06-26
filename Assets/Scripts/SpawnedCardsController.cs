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
    [SerializeField] private int _cardsShown = 0;

    [SerializeField] private Card[] _cards = new Card[3];

    public int CardsShown => _cardsShown;

    public Transform FirstCardPosition => _firstCardPosition;
    public Transform SecondCardPosition => _secondCardPosition;
    public Transform ThirdCardPosition => _thirdCardPosition;

    private void OnEnable()
    {
        Deck.OnDeckClicked += OpenNewCard;
    }

    private void OnDisable()
    {
        Deck.OnDeckClicked -= OpenNewCard;
    }

    private void OpenNewCard(Card card)
    {
        _cardsShown++;
        if (_cardsShown == 4) 
        {
            _cardsShown = 0;
        }

        if (_thirdCardPosition.childCount != 0)
        {
            _cards[2].transform.position = _deck.transform.position;
            _cards[2].transform.parent = _deck.transform;
            _cards[2].SetSortingOrder(-10);
            _deck.ReturnCardToDeck(_cards[2]);
        }

        if (_secondCardPosition.childCount != 0)
        {
            _cards[1].transform.position = _thirdCardPosition.position;
            _cards[1].transform.parent = _thirdCardPosition;
            _cards[1].SetSortingOrder((int)OrderLayers.Third);
            _cards[2] = _cards[1];
        }

        if (_firstCardPosition.childCount != 0)
        {
            _cards[0].transform.position = _secondCardPosition.position;
            _cards[0].transform.parent = _secondCardPosition;
            _cards[0].SetSortingOrder((int)OrderLayers.Second);
            _cards[1] = _cards[0];
        }

        card.gameObject.SetActive(true);
        card.SetSortingOrder((int)OrderLayers.First);
        card.transform.position = _firstCardPosition.position;
        card.transform.parent = _firstCardPosition;
        _cards[0] = card;
    }
}
