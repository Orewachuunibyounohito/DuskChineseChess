using ChineseChess.Chesses;
using ChuuniExtension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChineseChess
{
    [RequireComponent(typeof(Rigidbody))]
    public class ChessMoving : MonoBehaviour
    {
        private Rigidbody rb;
        private CheckerBoardView boardView;
        private Vector3 destination;
        private float duration;
        private float step;
        [SerializeField, ReadOnly]
        private bool IsMoving = false;
        private ICommand command;

        private bool IsFinished => Vector3.Distance(transform.position.InXZ(), destination.InXZ()) < 0.01f;

        private void Awake(){
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate(){
            if(!IsMoving){ return ; }
            if(IsFinished){
                Destroy(this);
                rb.isKinematic = true;
                transform.position = destination;
                if(command == null){ return ; }
                command.IsFinished = true;
            }
            
            var newPosition = Vector3.MoveTowards(transform.position.InXZ(), destination.InXZ(), step)
                            + new Vector3(0, transform.position.y, 0);
            transform.position = newPosition;
        }

        public void StartMoving(MoveMode mode){
            // rb.isKinematic = true;
            GetComponent<Collider>().isTrigger = false;
            IsMoving = true;
            command = ChessCommandInvoker.Instance.CurrentCommand;
            var horizontalVector = destination.InXZ() - transform.position.InXZ();
            step = horizontalVector.magnitude * (Time.fixedDeltaTime/duration);
            if(mode == MoveMode.Walk){ return ; }
            rb.isKinematic = false;
            rb.velocity = Physics.gravity * -((duration+Time.fixedDeltaTime) / 2);
        }

        public ChessMoving SetDestination(Vector3 destination){
            this.destination = destination;
            return this;
        }

        public ChessMoving SetDuration(float duration){
            this.duration = duration;
            return this;
        }

        public ChessMoving SetCommand(ICommand command){
            this.command = command;
            return this;
        }
    }
}
