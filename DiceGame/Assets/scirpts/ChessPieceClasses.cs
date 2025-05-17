
using UnityEngine;
using System.Collections.Generic;

public abstract class ChessPiece {
    public PieceType Type { get; protected set; }
    public PieceColor Color { get; protected set; }
    public Vector2Int Position { get; set; }
    public bool HasMoved { get; set; }

    public ChessPiece(PieceColor color, Vector2Int pos) {
        Color = color;
        Position = pos;
        HasMoved = false;
    }

    public abstract List<Vector2Int> GetLegalMoves(ChessPiece[,] board);

    protected bool InBounds(Vector2Int pos) => pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
}

// === Piece Implementations ===

public class Pawn : ChessPiece {
    public Pawn(PieceColor color, Vector2Int pos) : base(color, pos) {
        Type = PieceType.Pawn;
    }

    public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board) {
        List<Vector2Int> moves = new();
        int dir = Color == PieceColor.White ? 1 : -1;

        Vector2Int forward = new(Position.x, Position.y + dir);
        if (InBounds(forward) && board[forward.x, forward.y] == null)
            moves.Add(forward);

        Vector2Int doubleForward = new(Position.x, Position.y + 2 * dir);
        if (!HasMoved && InBounds(doubleForward) && board[forward.x, forward.y] == null && board[doubleForward.x, doubleForward.y] == null)
            moves.Add(doubleForward);

        foreach (int dx in new int[] { -1, 1 }) {
            Vector2Int diag = new(Position.x + dx, Position.y + dir);
            if (InBounds(diag) && board[diag.x, diag.y] != null && board[diag.x, diag.y].Color != Color)
                moves.Add(diag);
        }

        return moves;
    }
}

public class Rook : ChessPiece {
    public Rook(PieceColor color, Vector2Int pos) : base(color, pos) {
        Type = PieceType.Rook;
    }

    public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board) {
        return GetMovesInDirections(board, new[] {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        });
    }

    protected List<Vector2Int> GetMovesInDirections(ChessPiece[,] board, Vector2Int[] directions) {
        List<Vector2Int> moves = new();
        foreach (var dir in directions) {
            Vector2Int pos = Position + dir;
            while (InBounds(pos)) {
                if (board[pos.x, pos.y] == null)
                    moves.Add(pos);
                else {
                    if (board[pos.x, pos.y].Color != Color)
                        moves.Add(pos);
                    break;
                }
                pos += dir;
            }
        }
        return moves;
    }
}

public class Knight : ChessPiece {
    public Knight(PieceColor color, Vector2Int pos) : base(color, pos) {
        Type = PieceType.Knight;
    }

    public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board) {
        List<Vector2Int> moves = new();
        Vector2Int[] deltas = {
            new(2, 1), new(1, 2), new(-1, 2), new(-2, 1),
            new(-2, -1), new(-1, -2), new(1, -2), new(2, -1)
        };

        foreach (var delta in deltas) {
            Vector2Int next = Position + delta;
            if (InBounds(next) && (board[next.x, next.y] == null || board[next.x, next.y].Color != Color))
                moves.Add(next);
        }

        return moves;
    }
}

public class Bishop : Rook {
    public Bishop(PieceColor color, Vector2Int pos) : base(color, pos) {
        Type = PieceType.Bishop;
    }

    public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board) {
        return GetMovesInDirections(board, new[] {
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(-1, -1), new Vector2Int(1, -1)
        });
    }
}

public class Queen : Rook {
    public Queen(PieceColor color, Vector2Int pos) : base(color, pos) {
        Type = PieceType.Queen;
    }

    public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board) {
        return GetMovesInDirections(board, new[] {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(-1, -1), new Vector2Int(1, -1)
        });
    }
}

public class King : ChessPiece {
    public King(PieceColor color, Vector2Int pos) : base(color, pos) {
        Type = PieceType.King;
    }

    public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board) {
        List<Vector2Int> moves = new();
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0) continue;
                Vector2Int next = new(Position.x + dx, Position.y + dy);
                if (InBounds(next) && (board[next.x, next.y] == null || board[next.x, next.y].Color != Color))
                    moves.Add(next);
            }
        }
        return moves;
    }
}
