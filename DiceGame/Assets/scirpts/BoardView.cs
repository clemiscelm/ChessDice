
using UnityEngine;
using System.Collections.Generic;

public class BoardView : MonoBehaviour {
    public GameObject piecePrefab;
    public ChessGameManager gameLogic;
    public Dictionary<string, Sprite> spriteDict;

    private Dictionary<Vector2Int, ChessPieceView> piecesOnBoard = new();
    private List<GameObject> tileHighlights = new();

    private void Awake() {
        LoadSprites();
    }

    void Start() {
        UpdateBoardView();
    }

    void LoadSprites() {
        spriteDict = new();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
        foreach (Sprite s in sprites) {
            spriteDict[s.name] = s;
        }
    }

    void ClearHighlights() {
        foreach (var go in tileHighlights)
            Destroy(go);
        tileHighlights.Clear();
    }

    public void UpdateBoardView() {
        ClearHighlights();

        foreach (var kvp in piecesOnBoard) {
            Destroy(kvp.Value.gameObject);
        }
        piecesOnBoard.Clear();

        List<PieceType> playable = gameLogic.GetUsableTypes();
        var selected = gameLogic.GetSelectedPiece();
        var legal = gameLogic.GetLegalMoves();

        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                var piece = gameLogic.Board[x, y];
                if (piece != null) {
                    var pos = new Vector2Int(x, y);
                    string spriteKey = (piece.Color == PieceColor.White ? "w_" : "b_") + piece.Type.ToString().ToLower();

                    if (!spriteDict.ContainsKey(spriteKey)) {
                        Debug.LogError("Sprite non trouvé : " + spriteKey);
                        continue;
                    }

                    GameObject go = Instantiate(piecePrefab);
                    var view = go.GetComponent<ChessPieceView>();
                    if (view == null) {
                        Debug.LogError("ChessPieceView manquant sur le prefab !");
                        continue;
                    }

                    view.Init(piece.Type, piece.Color, spriteDict[spriteKey]);
                    view.SetPosition(pos);
                    piecesOnBoard[pos] = view;

                    // 🔶 Surlignage jaune si sélectionnée
                    if (piece == selected) {
                        view.Highlight(Color.yellow);
                    }
                    // ✅ Vert clair si jouable par les dés
                    else if (piece.Color == gameLogic.currentTurn && playable.Contains(piece.Type)) {
                        view.Highlight(new Color(0.5f, 1f, 0.5f, 1f));
                    }
                }
            }
        }

        // 🔷 Affichage des coups possibles
        foreach (var pos in legal) {
            GameObject highlight = new GameObject("Highlight");
            highlight.transform.position = new Vector3(pos.x - 3.5f, pos.y - 3.5f, 0);
            SpriteRenderer sr = highlight.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("Sprites/highlight_tile");
            sr.color = new Color(0f, 1f, 0f, 0.3f);
            sr.sortingOrder = 1;
            tileHighlights.Add(highlight);
        }
    }
}
