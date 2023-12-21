using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the managing of garden data, including creation and destruction of flowers.
/// Terrain generation code should not be in here (if implemented)
/// </summary>
public class GardenManager : MonoBehaviour
{
    public static GardenManager instance;

    private void Awake() => instance = this;

    public Dictionary<Vector2Int, FlowerScript> flowers = new Dictionary<Vector2Int, FlowerScript>();
    public Dictionary<Vector2Int, GameObject> weeds = new Dictionary<Vector2Int, GameObject>();
    //more dicts for weeds, rocks deco etc

    public bool TryPlantFlower(Vector2Int position, Flower flower)
    {
        if (!flowers.ContainsKey(position) && !weeds.ContainsKey(position))
        {
            GameObject newFlower = new GameObject(flower.name);
            FlowerScript newFlowerScript = newFlower.AddComponent<FlowerScript>();
            newFlowerScript.flowerData = flower;

            //change only the child model
            Transform newModel = Instantiate(flower.models[0]).transform;
            newModel.parent = newFlower.transform;
            newModel.localRotation = Quaternion.Euler(-90f, 0, 0);
            newModel.gameObject.AddComponent<Sway>();

            newFlower.transform.position = Helper.Vec2toVec3(position);
            newFlower.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

            flowers.Add(position, newFlowerScript);

            ParticleManager.instance.PlayDirtBurst(position);
            
            //pan camera
            Vector2 center = Vector2.zero;
            foreach (Vector2 pos in flowers.Keys)
            {
                center += pos;
            }
            center /= flowers.Count;
            CameraController.instance.UpdatePivot(Helper.RoundVec2(center));
            
            return true;
        }
        else { return false; }
    }

    #region Growing Weeds

    public GameObject weedsPrefab;

    public void GrowWeeds(Vector2Int position)
    {
        GameObject weed = Instantiate(weedsPrefab, Helper.Vec2toVec3(position), Quaternion.Euler(0, Random.Range(0, 360f), 0));
        ParticleManager.instance.PlayDirtBurst(position);
        weeds.Add(position, weed);
    }

    #endregion
}
