using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
