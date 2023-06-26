using UnityEngine;
using System;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public static event Action<Card> OnDeckClicked;
    private const int DECK_SIZE = 52;

    [SerializeField] private Card _card;

    private Queue<Card> _cards = new();
    private SpriteRenderer _spriteRenderer;

    public Queue<Card> Cards => _cards;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SpawnDeck();
    }

    private void OnMouseDown()
    {
        OnDeckClicked?.Invoke(_cards.Dequeue());
        if (_cards.Count == 0)
        {
            _spriteRenderer.enabled = false;
        }
        else
        {
            _spriteRenderer.enabled = true;
        }
    }

    private void SpawnDeck()
    {
        for (int i = 0; i < DECK_SIZE; i++)
        {
            Card card = Instantiate(_card, transform.position, Quaternion.identity);
            card.transform.SetParent(transform);
            card.gameObject.SetActive(false);
            _cards.Enqueue(card);
        }
    }

    public void ReturnCardToDeck(Card card)
    {
        _cards.Enqueue(card);
        card.gameObject.SetActive(false);
    }
}
