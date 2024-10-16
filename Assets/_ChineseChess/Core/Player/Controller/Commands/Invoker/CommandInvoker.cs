using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChineseChess
{
    public class CommandInvoker : MonoBehaviour
    {
        [ShowInInspector]
        protected Queue<ICommand> commands;
        private ICommand currentCommand;
        public ICommand CurrentCommand{ get => currentCommand; }

        protected virtual void Awake(){
            commands = new();
        }

        public void AddCommand(ICommand command) => commands.Enqueue(command);
        public void AddCommands(IEnumerable<ICommand> commands){
            foreach(var command in commands){
                this.commands.Enqueue(command);
            }
        }

        public virtual void Execute(){
            if(IsDone()){ return ; }
            StartCoroutine(ExecuteTask());
        }

        public bool IsDone() => commands.Count == 0
                             && (currentCommand == null || currentCommand.IsFinished);

        private bool HasNext() => commands.Count > 0;

        protected virtual IEnumerator ExecuteTask(){            
            currentCommand = commands.Dequeue();
            currentCommand.Execute();
            while(HasNext()){
                yield return new WaitForSeconds(0.05f);
                if(currentCommand.IsFinished){
                    currentCommand = commands.Dequeue();
                    currentCommand.Execute();
                }
            }
        }
    }
}
