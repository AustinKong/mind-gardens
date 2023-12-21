using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoeItem : Item
{
    ParticleSystem deweedParticle = null;

    public override void UseEffect()
    {
        if (deweedParticle == null) deweedParticle = GameObject.Find("DeweedParticle").GetComponent<ParticleSystem>();

        base.UseEffect();

        if (deweedParticle.isPlaying) return;

        SFXManager.instance.PlaySoilSFX();

        if (GardenManager.instance.weeds.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            deweedParticle.transform.position = Helper.Vec2toVec3(MouseManager.instance.GetMousePositionSnapped());
            deweedParticle.Play();
            Destroy(GardenManager.instance.weeds[MouseManager.instance.GetMousePositionSnapped()]);
            GardenManager.instance.weeds.Remove(MouseManager.instance.GetMousePositionSnapped());
        }

        if (GardenManager.instance.flowers.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            deweedParticle.transform.position = Helper.Vec2toVec3(MouseManager.instance.GetMousePositionSnapped());
            deweedParticle.Play();

            FlowerScript flower = GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()];

            if (flower.age >= flower.flowerData.growthDuration)
            {
                flower.CompleteBlossom();
            }
            else
            {
                flower.Wither();
            }
        }
    }
}
