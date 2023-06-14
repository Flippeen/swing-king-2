using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{

    [SerializeField] float grabRadius;

    public bool IsGrabbable(Vector2 grabbingPosition)
    {
        return Vector2.Distance(grabbingPosition, transform.position) < grabRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
