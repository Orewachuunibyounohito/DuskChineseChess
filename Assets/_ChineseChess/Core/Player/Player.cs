using System;
using System.Collections.Generic;
using ChineseChess.Chesses;
using ChineseChess.Controller;
using ChineseChess.Tools;
using ChuuniExtension.Loggers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace ChineseChess
{
    [Serializable]
    public class Player
    {
        [ShowInInspector, ReadOnly]
        public ChessCamp Camp{ get; private set; }
        public bool IsWinner { get; private set; }

        // [ShowInInspector, ReadOnly]
        private List<Chess> alives;
        private List<Chess> defeated;
        private IController controller;
        
        [ShowInInspector]
        private PlayerStateMachine stateMachine;
        [SerializeField, ReadOnly]
        private Cinemachine.CinemachineVirtualCamera vCam;

        public event Action<List<Vector2Int>> LightUpSurrounding;
        public event Action<ChessCamp> ActivateFogs;

        public Player(ChessCamp camp, CheckerBoard board, PlayerControlType controlType, FogView fogView){
            alives = new(ChessesGenerater.CreateStandardChesses(camp));
            defeated = new();
            Camp = camp;

            foreach(var chess in alives){
                chess.Camp = Camp;
                chess.Moved += board.UpdateBoard;
                chess.MovedWithCommand += board.UpdateBoardWithCommand;
                chess.MovedWithCommand += fogView.UpdateFogForMove;
                chess.ChessDefeated += OnChessDefeated;
            }

            controller = PlayerControllerFactory.Generate(controlType, this);
            stateMachine = new PlayerStateMachine(this, controller);
            stateMachine.OnNewPlayer();

            vCam = RootGroup.Enviroment.Find($"{Camp}Viewport").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }

        public void FrameUpdate() => stateMachine.FrameUpdate();

        public List<Chess> GetAlives() => new List<Chess>(alives);
        public bool IsTurnEnd() => stateMachine.IsTurnEnd();
        public void TurnEnter() => stateMachine.TurnStart();

        public bool Contains(Chess chess) => alives.Contains(chess);

        public void OnChessDefeated(Chess chess){
            alives.Remove(chess);
            defeated.Add(chess);
        }

        public void OnMyVision(){
            List<Vector2Int> vision = new();
            foreach(var chess in alives){
                vision.AddRange(ChessPositionMappingTool.FindSurroundingByChess(chess));
            }

            LightUpSurrounding.Invoke(vision);
        }
        public void OnActivateFogs() => ActivateFogs.Invoke(Camp);

        public void RegisterViewerUpdate(ChessContentViewer viewer){
            stateMachine.UpdateViewer += viewer.UpdateField;
            if(controller is HumanController){
                var humanController = (HumanController)controller;
                humanController.UpdateViewer += viewer.UpdateField;
                humanController.RegisterViewerUpdate(viewer);
            }
        }

        public void GetWin(){
            IsWinner = true;
        }

        public void TurnToMyVision() => vCam.Priority = 100;
        public void TurnAwayMyVision() => vCam.Priority = 10;
    }
}
