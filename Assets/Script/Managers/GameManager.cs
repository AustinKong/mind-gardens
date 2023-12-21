using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct ItemPair
{
    public Item item;
    public int amount;

    public ItemPair(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() => instance = this;

    public bool viewMode = false;
    public bool devMode = false;

    private void Start()
    {
        SceneTransition.instance.TransitionIn();
        GameTicksInit();
        InventoryInit();
    }

    private void Update()
    {
        UseItem();
        ScrollItem();
        CheckViewMode();
        CheckGameSpeed();
    }

    private void CheckGameSpeed()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (Time.timeScale)
            {
                case 1:
                    Time.timeScale = 2;
                    break;
                case 2:
                    Time.timeScale = 4;
                    break;
                case 4:
                    Time.timeScale = 1;
                    break;
            }
        }
    }

    #region View mode
    private void CheckViewMode()
    {
        if(Input.GetKeyDown(KeyCode.V)) viewMode = !viewMode;

        if (viewMode) inventoryIndex = 0;
    }

    #endregion

    #region Inventory and items

    public List<ItemPair> inventory = new List<ItemPair>();
    private void InventoryInit()
    {
        
    }

    private void UseItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GetSelectedItem().item != null) GetSelectedItem().item.UseEffect();
        }
    }
    
    public int inventoryIndex = 0;
    private void ScrollItem()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) inventoryIndex++;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) inventoryIndex--;

        if (inventoryIndex > inventory.Count) inventoryIndex = 0;
        if (inventoryIndex < 0) inventoryIndex = inventory.Count;
    }

    public ItemPair GetSelectedItem()
    {
        if (inventoryIndex == 0) return new ItemPair(null, 0);
        else return inventory[inventoryIndex - 1];
    }

    public void ConsumeSelectedItem()
    {
        if(inventory[inventoryIndex - 1].amount - 1 > 0)
        {
            inventory[inventoryIndex - 1] = new ItemPair(inventory[inventoryIndex - 1].item, inventory[inventoryIndex - 1].amount - 1);
        }
        else
        {
            inventory.RemoveAt(inventoryIndex - 1);
            inventoryIndex--;
        }
    }

    public void AddItem(Item item, int amount)
    {
        for(int i = 0; i < inventory.Count; i++) //stacking
        {
            if(inventory[i].item == item)
            {
                inventory[i] = new ItemPair(inventory[i].item, inventory[i].amount + amount);
                return;
            }
        }
        inventory.Add(new ItemPair(item, amount));
    }

    #endregion

    #region Mood

    [Header("Mood values")]
    public float happy_sad = 0;
    public float calm_anxious = 0;
    public float energized_tired = 0;

    public void AffectMood(float happy_sadAmount, float calm_anxiousAmount, float energized_tiredAmount)
    {
        happy_sad = Mathf.Clamp(happy_sad + happy_sadAmount, -1f, 1f);
        calm_anxious = Mathf.Clamp(calm_anxious + calm_anxiousAmount, -1f, 1f);
        energized_tired = Mathf.Clamp(energized_tired + energized_tiredAmount, -1f, 1f);
    }

    private void WeedsAffectingMood()
    {
        int weedCount = GardenManager.instance.weeds.Count;
        AffectMood(-0.0005f * weedCount, -0.0005f * weedCount, -0.0005f * weedCount);
    }

    #endregion

    #region GameTicks

    public delegate void tick();
    public tick gameTick;
    public tick eventTick;

    private void GameTicksInit()
    {
        gameTick += () =>
        {
            WeedsAffectingMood();
        };

        eventTick += () => //events
        {
            if (Random.Range(0, 1f) > 0.5f) GrowWeeds(); //50% chance to grow weeds
            if (Random.Range(0, 1f) > 0.5f) TriggerPruning(); //50% chance to need prune
            if (Random.Range(0, 1f) > 0.8f) GrowWildFlowers(); //20% chance to wild flower
        }; 
        StartCoroutine(InvokeGameTick());
        StartCoroutine(InvokeEventTick());
    }

    private IEnumerator InvokeGameTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(devMode ? 0.25f : 1); //1 tps

            if (!viewMode) gameTick.Invoke();
        }
    }

    private IEnumerator InvokeEventTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(devMode ? 2.5f : 15); //0.1 tps

            if (!viewMode) eventTick.Invoke();
        }
    }
    #endregion

    #region Events

    private void GrowWeeds()
    {
        if (GardenManager.instance.flowers.Count == 0) return;

        for(int tries = 0; tries < 500; tries++) //no endless loops!
        {
            Vector2Int position = GardenManager.instance.flowers.Keys.ElementAt(Random.Range(0, GardenManager.instance.flowers.Count)) + Helper.neigbourTiles[Random.Range(0, 8)];
            if (!GardenManager.instance.flowers.ContainsKey(position) && !GardenManager.instance.weeds.ContainsKey(position))
            {
                GardenManager.instance.GrowWeeds(position);
                break;
            }
            else continue;
        }
    }

    private void TriggerPruning()
    {
        if (GardenManager.instance.flowers.Count == 0) return;

        FlowerScript target = GardenManager.instance.flowers.Values.ElementAt(Random.Range(0, GardenManager.instance.flowers.Count));
        target.statuses.Remove("Needs Pruning");
        target.statuses.Add("Needs Pruning");
    }

    private void GrowWildFlowers()
    {
        if (GardenManager.instance.flowers.Count == 0) return;

        for (int tries = 0; tries < 500; tries++) //no endless loops!
        {
            Vector2Int position = GardenManager.instance.flowers.Keys.ElementAt(Random.Range(0, GardenManager.instance.flowers.Count)) + Helper.farNeighbourTiles[Random.Range(0, 16)];
            if (!GardenManager.instance.flowers.ContainsKey(position) && !GardenManager.instance.weeds.ContainsKey(position))
            {
                GardenManager.instance.TryPlantFlower(position, allSeeds[Random.Range(0,allSeeds.Length)].flower);
                GardenManager.instance.flowers[position].wild = true;
                break;
            }
            else continue;
        }
    }

    #endregion

    public SeedItem[] allSeeds;
}