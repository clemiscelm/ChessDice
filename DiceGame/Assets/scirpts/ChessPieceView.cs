using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ChessPieceView : MonoBehaviour {
    public PieceType Type;
    public PieceColor Color;

    public void Init(PieceType type, PieceColor color, Sprite sprite) {
        Type = type;
        Color = color;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetPosition(Vector2Int gridPos) {
        float cellSize = 1f;
        Vector3 offset = new Vector3(-3.5f, -3.5f, 0f); // centre le plateau
        transform.position = new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0) + offset;
    }
    public void Highlight(Color color) {
        GetComponent<SpriteRenderer>().color = color;
    }


}
