using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OpenedCardsController : MonoBehaviour
{
    private enum OrderLayers
    {
        First = 3,
        Second = 2,
        Third = 1,
        InDeck = 0,
        Opening = 100
    }

    [Header("Parent Transforms")]
    [SerializeField] private Transform _firstCardPosition;
    [SerializeField] private Transform _secondCardPosition;
    [SerializeField] private Transform _thirdCardPosition;

    [Header("Animations Durations")]
    [SerializeField] private float _cardMoveDuration;
    [SerializeField] private float _cardRotationDuration;

    [Space]
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
        StartCoroutine(RotateCard(_openedCards[0], (int)OrderLayers.First));
        StartCoroutine(MoveCard(card, _firstCardPosition));
        card.transform.parent = _firstCardPosition;
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

    private IEnumerator RotateCard(Card card, int sortingLayer)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = card.transform.rotation;
        Quaternion targetRotation = Quaternion.identity;

        card.SetSortingOrder((int)OrderLayers.Opening);
        while (elapsedTime < _cardRotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _cardRotationDuration);
            card.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        card.transform.rotation = targetRotation;
        card.SetSortingOrder(sortingLayer);
    }

    public void CollectOpenedCards()
    {
        for (int i = _openedCards.Length - 1; i >= 0; i--)
        {
            _openedCards[i].gameObject.SetActive(false);
            _openedCards[i].transform.parent = _thirdCardPosition;
            PreviousCards.Enqueue(_openedCards[i]);
        }
    }
}
