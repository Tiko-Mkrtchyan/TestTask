using UnityEngine;
using DG.Tweening;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float turnSpeed;
        [SerializeField] private float walkSpeed;

        private void Update()
        {
            if (transform.rotation.y>-20 && transform.rotation.y<20)
            {
                if (Input.GetKey(KeyCode.A) && transform.localRotation.eulerAngles.y > -45)
                {
                    transform.Rotate(Vector3.down, turnSpeed * Time.deltaTime);
                }
                
                if (Input.GetKey(KeyCode.D) && transform.localRotation.eulerAngles.y < 45)
                {
                    transform.Rotate(Vector3.down, turnSpeed * Time.deltaTime);
                }
            }
            
        }
    }
}