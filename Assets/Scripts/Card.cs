using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _cardBackSpriteRenderer;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSortingOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
        _cardBackSpriteRenderer.sortingOrder = order;
    }

    public void SetCardSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
