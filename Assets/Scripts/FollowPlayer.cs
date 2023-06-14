using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] bool followX, followY;
    void LateUpdate()
    {
        transform.position = new Vector3(followX ? targetTransform.position.x : transform.position.x, followY ? targetTransform.position.y : transform.position.y, transform.position.z);
    }
}
