
using UnityEngine;
using System.Collections.Generic;

public enum PieceType { King, Queen, Rook, Bishop, Knight, Pawn }
public enum PieceColor { White, Black }

public class ChessGameManager : MonoBehaviour {
    public ChessPiece[,] Board = new ChessPiece[8, 8];
    public PieceColor currentTurn = PieceColor.White;
    public bool gameOver = false;
    public string victoryMessage = "";
    public string[] diceResults = new string[3];

    private List<PieceType> usableTypes = new();
    private ChessPiece selectedPiece = null;
    private List<Vector2Int> legalMoves = new();
    private bool diceRolled = false;
    public List<PieceType> GetUsableTypes() => new List<PieceType>(usableTypes);
    public bool HasRolledDice() => diceRolled;


    void Start() {
        InitBoard();
        FindObjectOfType<BoardView>().UpdateBoardView();
        FindObjectOfType<UIManager>().UpdateUI(this);
    }

    public void InitBoard() {
        Board = new ChessPiece[8, 8];
        gameOver = false;
        currentTurn = PieceColor.White;
        usableTypes.Clear();
        selectedPiece = null;
        legalMoves.Clear();
        diceRolled = false;

        for (int i = 0; i < 8; i++) {
            Board[i, 1] = new Pawn(PieceColor.White, new Vector2Int(i, 1));
            Board[i, 6] = new Pawn(PieceColor.Black, new Vector2Int(i, 6));
        }

        PieceType[] layout = { PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook };
        for (int i = 0; i < 8; i++) {
            Board[i, 0] = CreatePiece(layout[i], PieceColor.White, new Vector2Int(i, 0));
            Board[i, 7] = CreatePiece(layout[i], PieceColor.Black, new Vector2Int(i, 7));
        }

        FindObjectOfType<BoardView>().UpdateBoardView();
        FindObjectOfType<UIManager>().UpdateUI(this);
    }

    ChessPiece CreatePiece(PieceType type, PieceColor color, Vector2Int pos) {
        return type switch {
            PieceType.Pawn => new Pawn(color, pos),
            PieceType.Rook => new Rook(color, pos),
            PieceType.Knight => new Knight(color, pos),
            PieceType.Bishop => new Bishop(color, pos),
            PieceType.Queen => new Queen(color, pos),
            PieceType.King => new King(color, pos),
            _ => null,
        };
    }

    public void RollDice() {
        if (diceRolled || gameOver) return;

        usableTypes.Clear();
        for (int i = 0; i < 3; i++) {
            PieceType type = (PieceType)Random.Range(0, 6);
            diceResults[i] = type.ToString();
            usableTypes.Add(type);
        }

        diceRolled = true;

        if (!CanPlayAny()) {
            Debug.Log("⏭ Aucun coup possible, on passe le tour.");
            EndTurn();
        }

        FindObjectOfType<UIManager>().UpdateUI(this);
    }

    public void OnClickCell(Vector2Int pos) {
        if (gameOver || !diceRolled) return;

        var piece = Board[pos.x, pos.y];
        Debug.Log("🟦 Clicked cell: " + pos);

        if (selectedPiece == null) {
            if (piece != null && piece.Color == currentTurn && usableTypes.Contains(piece.Type)) {
                selectedPiece = piece;
                legalMoves = piece.GetLegalMoves(Board);
            }
        } else {
            if (legalMoves.Contains(pos)) {
                MovePiece(selectedPiece.Position, pos);
                selectedPiece = null;
                legalMoves.Clear();
            }
            else if (piece == selectedPiece) {
                // Allow re-selection of the same piece if it's still usable
                legalMoves = selectedPiece.GetLegalMoves(Board);
            }
            else {
                selectedPiece = null;
                legalMoves.Clear();
            }
        }

        FindObjectOfType<BoardView>().UpdateBoardView();
        FindObjectOfType<UIManager>().UpdateUI(this);
    }

    void MovePiece(Vector2Int from, Vector2Int to) {
        var piece = Board[from.x, from.y];
        var target = Board[to.x, to.y];

        if (target != null && target.Type == PieceType.King) {
            gameOver = true;
            victoryMessage = $"{currentTurn} a capturé le roi !";
        }

        Board[to.x, to.y] = piece;
        Board[from.x, from.y] = null;
        piece.Position = to;
        piece.HasMoved = true;

        usableTypes.Remove(piece.Type);

        FindObjectOfType<BoardView>().UpdateBoardView();
        FindObjectOfType<UIManager>().UpdateUI(this);

        if (usableTypes.Count == 0 || !CanPlayAny()) {
            EndTurn();
        }
    }

    void EndTurn() {
        currentTurn = currentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White;
        selectedPiece = null;
        legalMoves.Clear();
        usableTypes.Clear();
        diceRolled = false;
        for (int i = 0; i < 3; i++) diceResults[i] = "-";

        FindObjectOfType<BoardView>().UpdateBoardView();
        FindObjectOfType<UIManager>().UpdateUI(this);
    }

    bool CanPlayAny() {
        foreach (var type in usableTypes) {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++) {
                    var p = Board[x, y];
                    if (p != null && p.Color == currentTurn && p.Type == type) {
                        if (p.GetLegalMoves(Board).Count > 0)
                            return true;
                    }
                }
        }
        return false;
    }

    public string GetPieceLabel(ChessPiece piece) {
        return piece.Type.ToString().Substring(0, 1).ToUpper();
    }

    public ChessPiece GetSelectedPiece() => selectedPiece;
    public List<Vector2Int> GetLegalMoves() => legalMoves;
    public void RestartGame()
    {
        InitBoard();                     // Réinitialise le plateau
        currentTurn = PieceColor.White; // Repart du joueur blanc
        usableTypes.Clear();            // Vide les dés
        selectedPiece = null;
        legalMoves.Clear();
        diceRolled = false;
        gameOver = false;
        victoryMessage = "";

        for (int i = 0; i < diceResults.Length; i++)
            diceResults[i] = "-";

        FindObjectOfType<BoardView>().UpdateBoardView();
        FindObjectOfType<UIManager>().UpdateUI(this);
    }

}
