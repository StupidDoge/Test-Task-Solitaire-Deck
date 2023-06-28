using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private Sprite[] _cardsSprites;
    [SerializeField] private SpawnedCardsController _spawnedCardsController;

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
        if (_cards.Count == 0)
        {
            _spriteRenderer.enabled = true;
            _spawnedCardsController.ClearOpenedCards();
            _cards = new Queue<Card>(_spawnedCardsController.PreviousCards);
            _spawnedCardsController.ClearPreviousCards();
            foreach (Card card in _cards)
            {
                card.transform.SetParent(transform);
            }
        }
        else if (_cards.Count == 1)
        {
            _spriteRenderer.enabled = false;
            _spawnedCardsController.OpenNewCard(_cards.Dequeue());
        }
        else
        {
            _spriteRenderer.enabled = true;
            _spawnedCardsController.OpenNewCard(_cards.Dequeue());
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
}
