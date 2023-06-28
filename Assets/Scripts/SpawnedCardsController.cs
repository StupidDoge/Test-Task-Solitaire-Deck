using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnedCardsController : MonoBehaviour
{
    private enum OrderLayers
    {
        First = 3,
        Second = 2,
        Third = 1,
        InDeck = 0
    }

    [SerializeField] private Transform _firstCardPosition;
    [SerializeField] private Transform _secondCardPosition;
    [SerializeField] private Transform _thirdCardPosition;
    [SerializeField] private float _cardMoveDuration;

    [SerializeField] private Card[] _openedCards = new Card[3];

    public Queue<Card> PreviousCards { get; private set; } = new();

    public void OpenNewCard(Card card)
    {
        if (_thirdCardPosition.childCount != 0)
        {
            SetupCard(_openedCards[2], _thirdCardPosition.transform, (int)OrderLayers.InDeck);
            _openedCards[2].gameObject.SetActive(false);
            PreviousCards.Enqueue(_openedCards[2]);
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
        StartCoroutine(MoveCard(card, targetTransform));
        card.transform.parent = targetTransform;
        card.SetSortingOrder(sortingLayer);
    }

    private IEnumerator MoveCard(Card card, Transform targetTransform)
    {
        float counter = 0f;
        while (counter < _cardMoveDuration)
        {
            counter += Time.deltaTime;
            card.transform.position = Vector3.Lerp(card.transform.position, targetTransform.position, _cardMoveDuration);
            yield return null;
        }
    }

    public void ClearOpenedCards()
    {
        for (int i = _openedCards.Length - 1; i >= 0; i--)
        {
            _openedCards[i].gameObject.SetActive(false);
            _openedCards[i].transform.parent = _thirdCardPosition;
            PreviousCards.Enqueue(_openedCards[i]);
        }
    }

    public void ClearPreviousCards()
    {
        PreviousCards.Clear();
    }
}
