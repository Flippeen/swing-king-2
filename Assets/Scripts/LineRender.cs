using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    [SerializeField] Transform hand, hammer;
    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        lr.SetPosition(0, hand.position);
        lr.SetPosition(1, hammer.position);
    }
}
