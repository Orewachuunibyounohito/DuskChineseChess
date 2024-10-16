using UnityEngine;

namespace ChineseChess
{
    public class DisappearByCollided : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other){
            if(other.CompareTag("Chess")){
                GetComponent<MeshRenderer>().renderingLayerMask = 0;
            }
        }
    }
}
