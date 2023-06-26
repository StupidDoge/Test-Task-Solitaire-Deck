using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SpawnedCardsController _gameManager;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = FindObjectOfType<SpawnedCardsController>();
        _spriteRenderer.color = Random.ColorHSV();
    }

    public void SetSortingOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
    }

    public void SetCardSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
