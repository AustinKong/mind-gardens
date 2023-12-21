using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedItem", menuName = "ScriptableObjects/Item/SeedItem")]
public class SeedItem : Item
{
    public Flower flower;

    public override void UseEffect()
    {
        base.UseEffect();
        if(GardenManager.instance.TryPlantFlower(MouseManager.instance.GetMousePositionSnapped(), flower))
        {
            SFXManager.instance.PlaySoilSFX();
            GameManager.instance.ConsumeSelectedItem(); //planted
        }
    }
}
