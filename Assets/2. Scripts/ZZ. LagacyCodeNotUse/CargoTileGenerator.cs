using UnityEngine;

public class CargoTileGenerator : MonoBehaviour
{
    [SerializeField] private float tileSize = 1; //default size
    [SerializeField] private float offsetX = -1; //타일 생성할 때 좌표 맞춰주기용
    [SerializeField] private float offsetY = 0;  //"
    [SerializeField] private GameObject tilePrefab; //TilePrefa
    [SerializeField] private Cargo cargo;
        
    private void Awake()
    {
        if (cargo == null | tilePrefab == null)
            return;

        CreateTile();
    }
    private void CreateTile()
    {
        //grid는 홀수로 입력이 되어야 함.
        Vector2 cargoGridInfo = new(cargo.width / 2, cargo.height / 2);
        Vector2 leftBottomPos = new(offsetX - cargoGridInfo.x, offsetY - cargoGridInfo.y);

        for (float i = leftBottomPos.x; i <= cargoGridInfo.x; i++)
        {
            for (float j = leftBottomPos.y; j <= cargoGridInfo.y; j++)
            {
                var tile = Instantiate(tilePrefab);
                tile.transform.parent = transform;
                tile.transform.localScale = new Vector3(tileSize, 0.1f, tileSize);
                tile.transform.localPosition = new Vector3(i * tileSize, j * tileSize, 0);
            }
        }
    }
}
