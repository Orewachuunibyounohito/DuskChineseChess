using ChuuniExtension;
using UnityEngine;

namespace ChineseChess
{
    public class AddForceByCollided : MonoBehaviour
    {
        private static float vectorY = 0.8f;
        private static float force = 150f;

        [SerializeField]
        private float _vectorY = 0.3f;
        [SerializeField]
        private float _force = 100f;
        [SerializeField]
        private bool isDebugger;

        private void Start(){
            if(isDebugger){ 
                vectorY = _vectorY;
                force = _force;
                return ;
            }
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        private void OnCollisionEnter(Collision other){
            if(other.collider.CompareTag("Chess")){
                other.rigidbody.isKinematic = true;
                other.collider.isTrigger = true;
                var contactPoint = other.GetContact(0).point;
                var forceVector = (transform.position - contactPoint).InXZ();
                forceVector.Normalize();
                forceVector += Vector3.up * vectorY;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce(forceVector * force);
            }
        }
    }
}
