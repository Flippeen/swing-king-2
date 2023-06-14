using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticleController : MonoBehaviour
{
    [SerializeField] Rigidbody2D targetRB;
    ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        ps.startSize = targetRB.velocity.x * 0.01f;
    }
}
