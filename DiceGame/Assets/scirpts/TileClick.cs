using UnityEngine;

public class TileClick : MonoBehaviour
{
    public Vector2Int gridPosition;

    void OnMouseDown() {
        
        FindObjectOfType<ChessGameManager>().OnClickCell(gridPosition);
    }
}