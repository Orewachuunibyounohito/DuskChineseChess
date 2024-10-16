using System;
using System.Collections.Generic;
using ChineseChess.Chesses;
using ChineseChess.SO;
using UnityEngine;

namespace ChineseChess.Tools
{
    public static class ChessPositionMappingTool
    {
        public static readonly int BoardWidth;
        public static readonly int BoardLength;
        public static readonly int RiverWidth;
        public static readonly Vector2Int TentCenter;
        public static readonly HashSet<Vector2Int> RiverSides;

        private static readonly Vector2Int borderPosition;
        private static readonly float coefficient;
        private static Vector3 offset;
        private static readonly Dictionary<DirectionType, Vector2Int> AroundOffset;

        static ChessPositionMappingTool(){
            var checkerBoradSettings = Resources.Load<CheckerBoradSettings>(ResourcesPath.CHECKERBOARD_SETTINGS);
            coefficient = checkerBoradSettings.PositionToViewCoefficient;
            BoardWidth = checkerBoradSettings.Width;
            BoardLength = checkerBoradSettings.Length;
            RiverWidth = checkerBoradSettings.RiverWidth;
            borderPosition = new Vector2Int(BoardWidth-1, BoardLength-1);
            offset = new Vector3(-BoardWidth/2, 1.1f, -(BoardLength+RiverWidth)/2) * coefficient;
            TentCenter = new Vector2Int(4, 1);
            AroundOffset = new();
            foreach(var offsetData in checkerBoradSettings.AroundOffsets){
                AroundOffset.Add(offsetData.direction, offsetData.vector);
            }
            RiverSides = new();
            for(int x = 0; x < BoardWidth; x++){
                int leftSide = BoardLength/2;
                int rightSide = BoardLength/2 + RiverWidth - 1;
                RiverSides.Add(new Vector2Int(x, leftSide));
                RiverSides.Add(new Vector2Int(x, rightSide));
            }
        }

        public static Vector2Int PositionMapping(Vector2Int position, ChessCamp camp){
            Vector2Int mappedPosition = position;
            if(camp == ChessCamp.Black){
                mappedPosition = borderPosition - position;
            }
            if(mappedPosition.y > 4){
                mappedPosition += new Vector2Int(0, RiverWidth);
            }
            return mappedPosition;
        }

        public static Vector2Int PositionMapping(Chess chess) => PositionMapping(chess.Position, chess.Camp);

        public static Vector3 PositionToView(Vector2Int mappedChessPosition){
            Vector3 viewPosition = offset + new Vector3(mappedChessPosition.x, 0, mappedChessPosition.y)*coefficient;
            return viewPosition;
        }

        public static List<Vector2Int> FindSurroundingByChess(Chess chess){
            Vector2Int mappedChessPosition = PositionMapping(chess);
            // return FindSurroundingByMappedPosition(mappedChessPosition);
            return FindSurroundingByMappedPositionWithRadius(mappedChessPosition, chess.VisionRadius);
        }

        public static List<Vector2Int> FindSurroundingByPosition(Vector2Int newPosition, ChessCamp camp, int visionRadius = 1){
            Vector2Int mappedPosition = PositionMapping(newPosition, camp);
            // return FindSurroundingByMappedPosition(mappedPosition);
            return FindSurroundingByMappedPositionWithRadius(mappedPosition, visionRadius);
        }

        public static List<Vector2Int> FindSurroundingByMappedPosition(Vector2Int mappedPosition){
            List<Vector2Int> surrounding = new(){ mappedPosition };
            foreach(var offset in AroundOffset.Values){
                var mappedSurrounding = mappedPosition + offset;
                surrounding.Add(mappedSurrounding);
            }
            return surrounding;
        }
        public static List<Vector2Int> FindSurroundingByMappedPositionWithRadius(Vector2Int mappedPosition, int visionRadius){
            List<Vector2Int> surrounding = new(){ mappedPosition };
            for(int y = -visionRadius; y <= visionRadius; y++){
                for(int x = -visionRadius; x <= visionRadius; x++){
                    var offset = new Vector2Int(x, y);
                    var mappedSurrounding = mappedPosition + offset;
                    surrounding.Add(mappedSurrounding);
                }
            }
            return surrounding;
        }

        public static void AdjustOffset(Vector3 newOffset) => offset = newOffset;
        public static Vector3 GetOffset() => offset;
    }
}