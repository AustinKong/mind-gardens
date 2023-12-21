using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    private void Awake() => instance = this;

    public ParticleSystem dirtBurstParticle;

    public void PlayDirtBurst(Vector2Int position)
    {
        dirtBurstParticle.transform.position = Helper.Vec2toVec3(position);
        dirtBurstParticle.Play();
    }
}
