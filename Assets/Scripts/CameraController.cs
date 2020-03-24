using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform referenceTransform;
    public float collisionOffset = 0.4f; 

    Vector3 defaultPosition;
    Vector3 normalizedDirection;
    Transform parentTransform;
    float defaultDistance;

    void Start()
    {
        defaultPosition = transform.localPosition;
        normalizedDirection = defaultPosition.normalized;
        defaultDistance = Vector3.Distance(defaultPosition, Vector3.zero);

        parentTransform = transform.parent;
    }

    void FixedUpdate()
    {
        Vector3 currentPosition = defaultPosition;
        RaycastHit hit;
        Vector3 tempDirection = parentTransform.TransformPoint(defaultPosition) - referenceTransform.position;
        if (Physics.SphereCast(referenceTransform.position, collisionOffset, tempDirection, out hit, defaultDistance))
        {
            currentPosition = (normalizedDirection * (hit.distance - collisionOffset));
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, currentPosition, Time.deltaTime * 15f);
    }
}