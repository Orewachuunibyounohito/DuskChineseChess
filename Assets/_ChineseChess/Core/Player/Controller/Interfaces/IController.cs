using ChineseChess.Chesses;
using UnityEngine.InputSystem;

namespace ChineseChess.Controller
{
    public interface IController
    {
        void SelectChess(InputAction.CallbackContext callback);
        bool ConfirmDestination();
        CommandInvoker GetInvoker();
        void Enable();
        void Disable();
    }
}
