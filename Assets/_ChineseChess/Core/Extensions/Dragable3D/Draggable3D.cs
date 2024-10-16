using UnityEngine;
using UnityEngine.InputSystem;

namespace ChuuniExtension.Draggables
{
    public class Draggable3D : MonoBehaviour
    {
        private Vector3 originPosition;
        private bool isDragging;
        private PlayerControls.GameplayActions playerInputs;
        private bool inputDisabled;

        private void Awake() => originPosition = transform.position;
        private void Start() => playerInputs = new PlayerControls().Gameplay;

        private void OnEnable(){
            playerInputs.Enable();
            inputDisabled = false;
        }
        private void OnDisable(){
            playerInputs.Disable();
            inputDisabled = true;
        }

        private void Update(){
            HandleDrag();
        }

        private void HandleDrag(){
            if(inputDisabled){ return ; }
            if (isDragging){
                Vector3 mouseToWorld = Camera.main.ScreenToWorldPoint(playerInputs.Point.ReadValue<Vector2>());
                Vector3 mappedPosition = new Vector3(mouseToWorld.x, 0.8f, mouseToWorld.z);
                transform.position = mappedPosition;
            }
            if(Mouse.current.leftButton.wasReleasedThisFrame){
                DragEnd();
                transform.position = originPosition;
            }
        }

        public void DragBegin() => isDragging = true;
        private void DragEnd() => isDragging = false;
    }
}