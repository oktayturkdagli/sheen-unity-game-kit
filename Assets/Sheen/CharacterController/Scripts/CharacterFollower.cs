using UnityEngine;
 
public class CharacterFollower : MonoBehaviour
{
    [SerializeField] Transform Target; // camera will follow this object
    [SerializeField] Transform camTransform; //camera transform
    [SerializeField] Vector3 Offset; // offset between camera and target
    [SerializeField] float SmoothTime = 0.3f; // change this value to get desired smoothness
    private Vector3 velocity = Vector3.zero; // This value will change at the runtime depending on target movement. Initialize with zero vector.

    private void Start()
    {
        Offset = camTransform.position - Target.position;
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 targetPosition = Target.position + Offset;
            camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        }  
    }
}