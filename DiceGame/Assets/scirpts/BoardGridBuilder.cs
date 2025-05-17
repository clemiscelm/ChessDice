using UnityEngine;

public class BoardGridBuilder : MonoBehaviour
{
    public GameObject tilePrefab;
    public Sprite tileLight;
    public Sprite tileDark;

    void Start()
    {
        Vector3 offset = new Vector3(-3.5f, -3.5f, 0f); // centre le plateau (8x8)

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 1) + offset, Quaternion.identity, transform);

                // Choix du sprite
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                sr.sprite = (x + y) % 2 == 0 ? tileLight : tileDark;
                sr.sortingOrder = -1; // fond derrière les pièces

                // Clic de sélection
                TileClick click = tile.GetComponent<TileClick>();
                if (click != null)
                    click.gridPosition = new Vector2Int(x, y);

                tile.name = $"Tile_{x}_{y}";
            }
        }
    }
}