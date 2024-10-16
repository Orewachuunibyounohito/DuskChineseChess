using System.Collections;
using System.Collections.Generic;
using ChineseChess;
using ChineseChess.Chesses;
using ChineseChess.SO;
using ChineseChess.Tools;
using ChuuniExtension;
using UnityEngine;

public class FogView
{
    private const int MovingDuration = 1;
    private static readonly GameObject fogPrefab;
    private static readonly Dictionary<ChessCamp, List<Vector2Int>> tentPositions;
    private static readonly List<Vector2Int> riverSides;
    
    private Dictionary<Vector2Int, FogContent> fogsNew;
    private GameObject fogCollection;

    static FogView(){
        fogPrefab = Resources.Load<PrefabsSettings>(ResourcesPath.PREFABS_SETTINGS)
                             .Fog;
        tentPositions = new(){
            {ChessCamp.Red, new()},
            {ChessCamp.Black, new()}
        };
        tentPositions[ChessCamp.Red].AddRange(
            ChessPositionMappingTool.FindSurroundingByPosition(
                ChessPositionMappingTool.TentCenter, ChessCamp.Red));
        tentPositions[ChessCamp.Black].AddRange(
            ChessPositionMappingTool.FindSurroundingByPosition(
                ChessPositionMappingTool.TentCenter, ChessCamp.Black));
        riverSides = new();
        riverSides.AddRange(ChessPositionMappingTool.RiverSides);
    }

    public FogView(){
        int width = ChessPositionMappingTool.BoardWidth;
        int length = ChessPositionMappingTool.BoardLength;
        int riverWidth = ChessPositionMappingTool.RiverWidth;
        GameObject objectCollection = GameObject.Find("----Object----");
        fogCollection = GameObject.Find("FogCollection");
        if(fogCollection == default){
            fogCollection = new GameObject("FogCollection");
        }
        fogCollection.transform.parent = objectCollection.transform;
        fogsNew = new();

        for(int x = 0; x < width; x++){
            for(int y = 0; y < length+riverWidth; y++){
                var position = new Vector2Int(x, y);
                var fog = Object.Instantiate(
                    fogPrefab,
                    ChessPositionMappingTool.PositionToView(position),
                    fogPrefab.transform.rotation,
                    fogCollection.transform
                );
                fogsNew.Add(position, new FogContent{ FogObject = fog, Count = 0 });
            }
        }
    }

    private void LightUpTent(ChessCamp camp){
        foreach(var tentPositionWithCamp in tentPositions[camp]){
            fogsNew[tentPositionWithCamp].Count += 9;
            fogsNew[tentPositionWithCamp].FogObject.SetActive(false);
        }
    }

    private void LightUpRiverSide(){
        foreach(var riverSidePosition in riverSides){
            fogsNew[riverSidePosition].Count += 9;
            fogsNew[riverSidePosition].FogObject.SetActive(false);
        }
    }

    public void ActivateAllFogs(ChessCamp camp){
        foreach(var fogContent in fogsNew.Values){
            fogContent.Count = 0;
            fogContent.FogObject.SetActive(true);
        }
        LightUpTent(camp);
        LightUpRiverSide();
    }

    public void LightUpAll(){
        foreach(var fogContent in fogsNew.Values){
            fogContent.Count = 100;
            fogContent.FogObject.SetActive(false);
        }
    }
    
    public void UpdateFog(List<Vector2Int> positions){
        foreach(var position in positions){
            if(!fogsNew.TryGetValue(position, out var fogContent)){ continue; }
            fogContent.Count++;
            if(fogContent.Count > 0){
                fogContent.FogObject.SetActive(false);
            }
        }
    }
    public void UpdateFogForMove(Chess chess, Vector2Int newPosition){
        ChessSystem.CoroutineExecutor.StartCoroutine(UpdateTask(chess, newPosition));
    }

    private IEnumerator UpdateTask(Chess chess, Vector2Int newPosition){
        var mappedChessPosition = ChessPositionMappingTool.PositionMapping(chess);
        var mappedNewPosition = ChessPositionMappingTool.PositionMapping(newPosition, chess.Camp);
        var originalSurrounding = ChessPositionMappingTool.FindSurroundingByChess(chess);
        var newSurrounding = ChessPositionMappingTool.FindSurroundingByPosition(newPosition, chess.Camp, chess.VisionRadius);
        var vector = mappedNewPosition - mappedChessPosition;
        var distance = vector.magnitude;
        var interval = MovingDuration / distance;
        var currentPosition = mappedChessPosition;
        var currentTime = 0f;
        foreach(var position in originalSurrounding){
            if(!fogsNew.TryGetValue(position, out var fogContent)){ continue; }
            fogContent.Count--;
        }
        while(currentPosition != mappedNewPosition){
            ChessSystem.CoroutineExecutor.StartCoroutine(UpdateFogOnRouteTask(currentPosition, chess.VisionRadius, interval));
            yield return new WaitForSeconds(interval);
            currentTime += interval;
            var delta = (vector.AsVector2()*currentTime).AsRoundVector2Int();
            currentPosition = mappedChessPosition+delta;
        }
        foreach(var position in newSurrounding){
            if(!fogsNew.TryGetValue(position, out var fogContent)){ continue; }
            fogContent.Count++;
        }
    }

    private IEnumerator UpdateFogOnRouteTask(Vector2Int newPosition, int visionRadius, float interval){
        var surrounding = ChessPositionMappingTool.FindSurroundingByMappedPositionWithRadius(newPosition, visionRadius);
        foreach(var position in surrounding){
            if(!fogsNew.TryGetValue(position, out var fogContent)){ continue; }
            fogContent.Count++;
            if(fogContent.Count > 0){
                fogContent.FogObject.SetActive(false);
            }
        }
        yield return new WaitForSeconds(interval*1.5f);
        foreach(var position in surrounding){
            if(!fogsNew.TryGetValue(position, out var fogContent)){ continue; }
            fogContent.Count--;
            if(fogContent.Count == 0){
                fogContent.FogObject.SetActive(true);
            }
        }
    }

    public void RegisterLightUpSurroundingEvent(params Player[] players){
        foreach(var player in players){
            player.LightUpSurrounding += UpdateFog;
        }
    }
    public void RegisterLightUpSurroundingEvent(Chess chess){
        chess.LightUpSurrounding += UpdateFog;
    }

    public void RegisterActivateAllFogEvent(params Player[] players){
        foreach(var player in players){
            player.ActivateFogs += ActivateAllFogs;
        }
    }

    public void ShowFogsCounts_Debug(){
        int amountOfRow = 9;
        int amount = 1;
        string log = "";
        foreach(var fogPair in fogsNew){
            var fogPosition = fogPair.Key;
            var fogCount = fogPair.Value.Count;
            log += $"\"{fogPosition, -8}:{fogCount, 3}\"{"", 3}";
            if(amount % amountOfRow == 0){
                log += "\n";
            }
            amount++;
        }
        log += "\b\b";
        Debug.Log(log);
    }

    public class FogContent
    {
        public GameObject FogObject;
        public int Count;
    }
}
