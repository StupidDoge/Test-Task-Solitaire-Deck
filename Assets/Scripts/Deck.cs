using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private OpenedCardsController _spawnedCardsController;
    [SerializeField] private Sprite[] _cardsSprites;

    private Queue<Card> _cards = new();
    private SpriteRenderer _spriteRenderer;
    private Quaternion _cardStartRotation;

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
            _spawnedCardsController.CollectOpenedCards();
            _cards = new Queue<Card>(_spawnedCardsController.PreviousCards);
            _spawnedCardsController.PreviousCards.Clear();
            foreach (Card card in _cards)
            {
                card.transform.SetParent(transform);
                card.transform.localPosition = Vector2.zero;
                card.transform.rotation = _cardStartRotation;
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
            _cardStartRotation.eulerAngles = new Vector3(0, -180, 0);
            Card card = Instantiate(_card, transform.position, _cardStartRotation);
            card.transform.SetParent(transform);
            card.gameObject.SetActive(false);
            card.SetCardSprite(_cardsSprites[i]);
            _cards.Enqueue(card);
        }
    }
}
