using UnityEngine;
using System;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public static event Action<Card> OnDeckClicked;

    [SerializeField] private Card _card;
    [SerializeField] private Sprite[] _cardsSprites;

    private Queue<Card> _cards = new();
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        System.Random random = new();
        random.Shuffle(_cardsSprites);
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
        for (int i = 0; i < _cardsSprites.Length; i++)
        {
            Card card = Instantiate(_card, transform.position, Quaternion.identity);
            card.transform.SetParent(transform);
            card.gameObject.SetActive(false);
            card.SetCardSprite(_cardsSprites[i]);
            _cards.Enqueue(card);
        }
    }

    public void ReturnCardToDeck(Card card)
    {
        _cards.Enqueue(card);
        card.gameObject.SetActive(false);
    }
}
