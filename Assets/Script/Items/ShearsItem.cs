using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShearsItem : Item
{

    ParticleSystem pruneParticle = null;
    public override void UseEffect()
    {
        if (pruneParticle == null) pruneParticle = GameObject.Find("PruneParticle").GetComponent<ParticleSystem>();

        base.UseEffect();

        if (pruneParticle.isPlaying) return;

        SFXManager.instance.PlayShearSFX();

        if (GardenManager.instance.flowers.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            if(GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()].statuses.Contains("Needs Pruning"))
            {
                pruneParticle.transform.position = Helper.Vec2toVec3(MouseManager.instance.GetMousePositionSnapped());
                pruneParticle.Play();
                GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()].statuses.Remove("Needs Pruning");
            }
        }
    }
}
