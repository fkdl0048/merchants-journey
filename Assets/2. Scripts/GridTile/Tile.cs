using Scripts.Utils;
using UnityEngine;

// 타일 클래스
public class Tile : MonoBehaviour
{
    public TileType tileType = TileType.Normal;
    public bool isWalkable = true;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void OnMouseEnter()
    {
        spriteRenderer.enabled = true;
    }
    private void OnMouseExit()
    {
        spriteRenderer.enabled = false;
    }
}
