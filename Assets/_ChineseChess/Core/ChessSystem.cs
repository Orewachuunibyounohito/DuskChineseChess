using System;
using ChineseChess.Chesses;
using ChineseChess.SO;
using ChuuniExtension.Loggers;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ChineseChess
{
    public class ChessSystem : MonoBehaviour
    {
        public static MonoBehaviour CoroutineExecutor;

        private Player red;
        private Player black;
        private CheckerBoard boardModel;
        private CheckerBoardView boardView;
        private FogView fogView;

        [ShowInInspector]
        private ChessStateMachine stateMachine;

        private void Awake(){
            CoroutineExecutor = this;
            GameInitialize();
        }

        private void Start(){
            ViewerInitialize();
            stateMachine.Opening();
        }

        private void Update() => stateMachine.FrameUpdate();

        private void GameInitialize(){
            SetupCheckerBoard();
            SetupFog();

            red = new Player(ChessCamp.Red, boardModel, PlayerControlType.HumanControl, fogView);
            black = new Player(ChessCamp.Black, boardModel, PlayerControlType.HumanControl, fogView);
            boardModel.AssignPlayers(red, black);
            
            fogView.RegisterLightUpSurroundingEvent(red, black);
            fogView.RegisterActivateAllFogEvent(red, black);
            foreach(var chess in red.GetAlives()){
                fogView.RegisterLightUpSurroundingEvent(chess);
            }
            foreach(var chess in black.GetAlives()){
                fogView.RegisterLightUpSurroundingEvent(chess);
            }

            stateMachine = new ChessStateMachine(GetGameplayContent());
        }

        private void SetupCheckerBoard(){
            boardModel = new CheckerBoard();
            boardView = new CheckerBoardView();
            boardModel.ChessAdded += boardView.Add;
            boardModel.ChessMoved += boardView.UpdateChessView;
            boardModel.ChessMovedWithCommand += boardView.UpdateChessViewWithCommand;
        }

        private void SetupFog(){
            fogView = new FogView();
        }

        private void ViewerInitialize(){
            var viewer = FindFirstObjectByType<ChessContentViewer>();
            stateMachine.UpdateViewer += viewer.UpdateField;
            stateMachine.PlayingState.UpdateViewer += viewer.UpdateField;
            red.RegisterViewerUpdate(viewer);
            black.RegisterViewerUpdate(viewer);
        }

        public GameplayContent GetGameplayContent() => new GameplayContent{
            Red = red,
            Black = black,
            Board = boardModel,
            BoardView = boardView,
            FogView = fogView
        };

        [Button, ShowIf("@EditorApplication.isPlaying")]
        public void ShowBoardInConsole() => boardModel.PrintBoard();
        
        [TitleGroup("Runtime Test Tool")]
        public TestContent TestObject;

        [TitleGroup("Runtime Test Tool")]
        [Button, ShowIf("@EditorApplication.isPlaying")]
        public void CreateTestObject(){
            var prefabsSettings = Resources.Load<PrefabsSettings>(ResourcesPath.PREFABS_SETTINGS);
            GameObject chessObject = Instantiate(prefabsSettings.Chess, TestObject.Position, prefabsSettings.Chess.transform.rotation);
            ChessMoving moving = chessObject.AddComponent<ChessMoving>();
            moving.SetDestination(chessObject.transform.position+new Vector3(0, 0, 5));
            moving.SetDuration(2);
            moving.StartMoving(TestObject.Mode);
        }

        [Button, ShowIf("@EditorApplication.isPlaying")]
        public void ShowFogsCounts(){
            fogView.ShowFogsCounts_Debug();
        }

        [Serializable]
        public class TestContent
        {
            public MoveMode Mode;
            public Vector3 Position;
        }
    }
}
