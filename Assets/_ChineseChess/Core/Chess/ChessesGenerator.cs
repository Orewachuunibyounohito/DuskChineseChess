using System.Collections.Generic;
using ChineseChess;
using ChineseChess.Chesses;
using UnityEngine;

public static class ChessesGenerater
{
    public static List<Chess> CreateStandardChesses(ChessCamp camp){
        return new List<Chess>(){
            new General(){ Position = new Vector2Int(4, 0), Camp = camp },
            new Guard(){ Position = new Vector2Int(3, 0), Camp = camp },
            new Guard(){ Position = new Vector2Int(5, 0), Camp = camp },
            new Bishop(){ Position = new Vector2Int(2, 0), Camp = camp },
            new Bishop(){ Position = new Vector2Int(6, 0), Camp = camp },
            new Knight(){ Position = new Vector2Int(1, 0), Camp = camp },
            new Knight(){ Position = new Vector2Int(7, 0), Camp = camp },
            new Rook(){ Position = new Vector2Int(0, 0), Camp = camp },
            new Rook(){ Position = new Vector2Int(8, 0), Camp = camp },
            new Artillery(){ Position = new Vector2Int(1, 2), Camp = camp },
            new Artillery(){ Position = new Vector2Int(7, 2), Camp = camp },
            new Pawn(){ Position = new Vector2Int(0, 3), Camp = camp },
            new Pawn(){ Position = new Vector2Int(2, 3), Camp = camp },
            new Pawn(){ Position = new Vector2Int(4, 3), Camp = camp },
            new Pawn(){ Position = new Vector2Int(6, 3), Camp = camp },
            new Pawn(){ Position = new Vector2Int(8, 3), Camp = camp },
        };
    }
}
