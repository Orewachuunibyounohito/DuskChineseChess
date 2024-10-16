using System;
using System.Collections.Generic;
using ChineseChess.Chesses;
using UnityEngine;

namespace ChineseChess
{
    public class ChessCommandInvoker : CommandInvoker
    {
        protected static ChessCommandInvoker _instance;
        public static ChessCommandInvoker Instance => _instance;
        public static bool IsNull() => _instance == null;

        public CheckerBoardView boardView;

        [SerializeField]
        private List<ChessCommandContent> customCommands;

        public Chess Target{ get; set; }

        protected override void Awake(){
            ChessCommandInvoker found = FindFirstObjectByType<ChessCommandInvoker>();
            if(_instance != null){ Destroy(gameObject); }
            else if(found != null){ _instance = found; }
            else{ _instance = this; }

            base.Awake();
            customCommands = new();
        }

        private void Start(){
            foreach(var commandType in customCommands){
                commands.Enqueue(CommandFactory.Generate(commandType));
            }
        }

        public override void Execute()
        {
            base.Execute();
        }

        public bool IsConfirm() => commands.Count > 0;
    }
}
