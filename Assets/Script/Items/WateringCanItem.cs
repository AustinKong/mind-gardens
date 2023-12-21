using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WateringCanItem : Item
{
    ParticleSystem wateringParticle = null;

    public override void UseEffect()
    {
        if (wateringParticle == null) wateringParticle = GameObject.Find("WateringParticle").GetComponent<ParticleSystem>();

        base.UseEffect();

        if (wateringParticle.isPlaying) return;
        wateringParticle.transform.position = Helper.Vec2toVec3(MouseManager.instance.GetMousePositionSnapped()) + new Vector3(0, 3, 0);
        wateringParticle.Play();
        SFXManager.instance.PlayWaterSFX();

        if (GardenManager.instance.flowers.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            //thirst +41 then -1 => +40
            GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()].thirst += 0.41f;
            GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()].UpdateThirstStatus();
        }
        
    }
}
