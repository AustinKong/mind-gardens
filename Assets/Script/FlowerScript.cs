using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    public Flower flowerData;
    public List<string> statuses = new List<string> { };
    public int age = 0;
    public new string name = "";
    public bool wild = false;

    private void Start()
    {
        GameManager.instance.gameTick += OnTick;
        name = (wild ? "Wild " : "") + flowerData.name + " Sprout";
    }

    private void OnTick()
    {
        age++;
        CheckGrowth();
        UpdateThirstStatus();
        UpdateCrowdingStatus();
        StatusesAffectingSeed();
        StatusesAffectingMood();

        if (age >= flowerData.growthDuration) StatusesAffectingMood();
    }

    //probability of getting new seed from this blossom
    private float seedStatus = 1;

    private void StatusesAffectingSeed()
    {
        if (statuses.Contains("Overcrowded")) seedStatus -= 0.005f;
        if (statuses.Contains("Very thirsty") || statuses.Contains("Very overwatered")) seedStatus -= 0.005f;
        if (statuses.Contains("Needs Pruning")) seedStatus -= 0.003f;
    }

    private void CheckGrowth()
    {
        //growth stages visual
        if (age == flowerData.growthDuration / 2 || age == flowerData.growthDuration)
        {
            Destroy(transform.GetChild(0).gameObject);
            Transform newModel = Instantiate(flowerData.models[age == flowerData.growthDuration ? 2 : 1]).transform;
            newModel.transform.parent = transform;
            newModel.localPosition = Vector3.zero;
            newModel.localRotation = Quaternion.Euler(-90f, 0, 0);
            newModel.gameObject.AddComponent<Sway>();
        }

        //growth stages functional
        if (age == flowerData.growthDuration / 2 || age == flowerData.growthDuration)
        {
            name = (wild ? "Wild " : "") + flowerData.name + (age == flowerData.growthDuration / 2 ? " Seedling" : " Blossoming");
        }

        //withering
        if(age >= flowerData.growthDuration + flowerData.blossomDuration)
        {
            CompleteBlossom();
        }
    }

    public void CompleteBlossom()
    {
        List<ItemPair> items = new List<ItemPair>();

        //give seed of same type
        Item item = flowerData.seed;
        int amount = Random.Range(2, 5);
        amount = Mathf.Clamp((int)(amount * seedStatus), 1, 99); //will always give atleast 1 seed
        items.Add(new ItemPair(item, amount));
        GameManager.instance.AddItem(item, amount);


        //chance to give new seed
        if (Random.Range(0,1f) < (seedStatus - 0.8f))
        {
            Item newSeed = GameManager.instance.allSeeds[Random.Range(0, GameManager.instance.allSeeds.Length)];
            items.Add(new ItemPair(newSeed, 1));
            GameManager.instance.AddItem(newSeed, 1);
        }
        UIManager.instance.TriggerItemReceive(items, Helper.Vec3toVec2(transform.position));

        Wither();
    }

    private void StatusesAffectingMood()
    {
        GameManager.instance.AffectMood(flowerData.happy_sad * 0.0001f, flowerData.calm_anxious * 0.0001f, flowerData.energized_tired * 0.0001f);
        if (statuses.Contains("Overcrowded")) GameManager.instance.AffectMood(-0.003f, -0.003f, -0.003f);
        if (statuses.Contains("Very thirsty") || statuses.Contains("Very overwatered")) GameManager.instance.AffectMood(-0.003f, -0.003f, -0.003f);
        if (statuses.Contains("Needs Pruning")) GameManager.instance.AffectMood(-0.001f, -0.001f, -0.001f);
    }

    public void UpdateCrowdingStatus()
    {
        Vector2Int position = Helper.Vec3toVec2(transform.position);

        int crowdingScore = 0;
        foreach(Vector2Int offset in Helper.neigbourTiles)
        {
            if (GardenManager.instance.flowers.ContainsKey(position + offset)) crowdingScore++;
            if (GardenManager.instance.weeds.ContainsKey(position + offset)) crowdingScore+=2;
        }

        statuses.Remove("Overcrowded");
        if (crowdingScore > flowerData.crowdingTolerance) statuses.Add("Overcrowded");
    }

    //-1 to +1
    //with -1 and 1 meaning death via drying out and overwatering respectively
    public float thirst = 0;
    public void UpdateThirstStatus()
    {
        thirst -= 0.01f; //33s to drop one status

        statuses.Remove("Very thirsty");
        statuses.Remove("Very overwatered");
        statuses.Remove("Thirsty");
        statuses.Remove("Overwatered");

        if (thirst <= -1) Wither();
        else if (thirst >= 1) Wither();
        else if (thirst <= -0.66f) statuses.Add("Very thirsty");
        else if (thirst >= 0.66f) statuses.Add("Very overwatered");
        else if (thirst <= -0.33f) statuses.Add("Thirsty");
        else if (thirst >= 0.33f) statuses.Add("Overwatered");
    }

    public void Wither()
    {
        GardenManager.instance.flowers.Remove(Helper.Vec3toVec2(transform.position));
        GameManager.instance.gameTick -= OnTick;
        Destroy(gameObject);
    }
}
