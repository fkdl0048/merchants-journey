using UnityEngine;

// 타일 클래스
public class Tile : MonoBehaviour
{
    public bool hasUnit { get; set; }

    [SerializeField] private Color TileEnterColor;
    [SerializeField] private Color TileExitColor;

    public void EnableTileHover(bool enable)
    {
        if (!enable)
            GetComponent<MeshRenderer>().material.color = TileExitColor;
        else
            GetComponent<MeshRenderer>().material.color = TileEnterColor;
    }
}
