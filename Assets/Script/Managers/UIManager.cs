using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manage all the UI in here
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake() => instance = this;

    public TMP_Text[] flowerInfo;
    public TMP_Text worldObjectInfo;
    public TMP_Text[] screenObjectInfo;
    public TMP_Text selectedItem;
    public TMP_Text moodStatus;
    public TMP_Text[] itemReceive;
    public GameObject viewMode;
    public Image gameSpeed;

    private void UpdateMoodInfo()
    {
        moodStatus.transform.parent.gameObject.SetActive(true);
        string moodText = "";
        //generate mood text
        float happy_sad = GameManager.instance.happy_sad;
        float calm_anxious = GameManager.instance.calm_anxious;
        float energized_tired = GameManager.instance.energized_tired;

        if (happy_sad > 0.1f) moodText += MoodModifierText(happy_sad) + " Joyful, ";
        else if (happy_sad < -0.1f) moodText += MoodModifierText(happy_sad) + " Moody, ";

        if (calm_anxious > 0.1f) moodText += MoodModifierText(calm_anxious) + " Calming, ";
        else if (calm_anxious < -0.1f) moodText += MoodModifierText(calm_anxious) + " Discomforting, ";

        if (energized_tired > 0.1f) moodText += MoodModifierText(energized_tired) + " Energizing, ";
        else if (energized_tired < -0.1f) moodText += MoodModifierText(energized_tired) + " Weary, ";

        if (moodText == "") moodText = "Neutral";
        else
        {
            moodText =  moodText.Remove(moodText.Length - 2, 2);
        }
        moodStatus.text = moodText;
    }

    private void ToggleGameSpeed()
    {
        if (Time.timeScale == 1) gameSpeed.gameObject.SetActive(false);
        else gameSpeed.gameObject.SetActive(true);
    }

    private string MoodModifierText(float value)
    {
        if (Mathf.Abs(value) == 1) return "Too";
        if (Mathf.Abs(value) > 0.66f) return "Extremely";
        if (Mathf.Abs(value) > 0.33f) return "Rather";
        return "Slightly";
    }

    private void UpdateScreenObjectInfo()
    {
        bool setActive = false;

        if (GardenManager.instance.flowers.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            FlowerScript flowerScript = GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()];

            screenObjectInfo[0].text = flowerScript.name;
            if (flowerScript.statuses.Count == 0) screenObjectInfo[1].text = "Healthy";
            else screenObjectInfo[1].text = string.Join(", ", flowerScript.statuses);
            setActive = true;
        }
        else if (GardenManager.instance.weeds.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            screenObjectInfo[0].text = "Weed";
            screenObjectInfo[1].text = "";
            setActive = true;
        }

        screenObjectInfo[0].transform.parent.gameObject.SetActive(setActive);
    }

    private void UpdateWorldObjectInfo()
    {
        Vector3 offset = Vector3.zero;

        if (GardenManager.instance.flowers.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            worldObjectInfo.text = GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()].name;
            offset = new Vector3(0, GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()].GetComponentInChildren<Renderer>().bounds.extents.y * 2f, 0);
        }
        else if (GardenManager.instance.weeds.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            worldObjectInfo.text = "Weed";
            offset = Vector3.up * 0.6f;
        }
        else
        {
            worldObjectInfo.gameObject.SetActive(false);
            return;
        }

        worldObjectInfo.rectTransform.parent.position = Helper.Vec2toVec3(MouseManager.instance.GetMousePositionSnapped()) + offset; ;
        worldObjectInfo.rectTransform.parent.rotation = Quaternion.Euler(0, CameraController.instance.angle, 0);

        worldObjectInfo.gameObject.SetActive(true);
    }

    private void UpdateSelectedItem()
    {
        if (GameManager.instance.GetSelectedItem().item == null) selectedItem.text = "-";
        else
        {
            if(GameManager.instance.GetSelectedItem().amount == 1)
            {
                selectedItem.text = GameManager.instance.GetSelectedItem().item.name;
            }
            else
            {
                selectedItem.text = GameManager.instance.GetSelectedItem().item.name + " (" + GameManager.instance.GetSelectedItem().amount + ")";
            }
        }
    }

    private void ToggleFlowerInfo()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            flowerInfo[0].transform.parent.gameObject.SetActive(!flowerInfo[0].transform.parent.gameObject.activeInHierarchy);
        }
    }

    private void UpdateFlowerInfo()
    {
        if (GardenManager.instance.flowers.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            FlowerScript flower = GardenManager.instance.flowers[MouseManager.instance.GetMousePositionSnapped()];
            flowerInfo[0].text = flower.flowerData.name;
            flowerInfo[1].text = flower.flowerData.scientificName;

            string flowerMoodAffectorText = "";
            flowerMoodAffectorText += "Happiness " + new string(flower.flowerData.happy_sad > 0 ? '+' : '-', Mathf.Abs((int)flower.flowerData.happy_sad)) + "\n";
            flowerMoodAffectorText += "Calmness  " + new string(flower.flowerData.calm_anxious > 0 ? '+' : '-', Mathf.Abs((int)flower.flowerData.calm_anxious)) + "\n";
            flowerMoodAffectorText += "Energy    " + new string(flower.flowerData.energized_tired > 0 ? '+' : '-', Mathf.Abs((int)flower.flowerData.energized_tired));
            flowerInfo[2].text = flowerMoodAffectorText;
        }
        else if (GardenManager.instance.weeds.ContainsKey(MouseManager.instance.GetMousePositionSnapped()))
        {
            flowerInfo[0].text = "Weed";
            flowerInfo[1].text = "Common garden weed";
            flowerInfo[2].text = "Happiness -\nCalmness  -\nEnergy    -";
        }
        else
        {
            flowerInfo[0].text = "";
            flowerInfo[1].text = "";
            flowerInfo[2].text = "";
        }
    }

    private void Update()
    {
        ToggleFlowerInfo();
        UpdateFlowerInfo();
        UpdateWorldObjectInfo();
        UpdateScreenObjectInfo();
        UpdateSelectedItem();
        UpdateMoodInfo();
        ToggleGameSpeed();

        CheckViewMode();
    }

    private void CheckViewMode()
    {
        if (GameManager.instance.viewMode)
        {
            viewMode.SetActive(true);
            flowerInfo[0].transform.parent.gameObject.SetActive(false);
            screenObjectInfo[0].transform.parent.gameObject.SetActive(false);
            moodStatus.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            viewMode.SetActive(false);
        }
    }

    #region Item Receive

    private bool[] itemReceiveIsPlaying = { false, false, false };

    public void TriggerItemReceive(List<ItemPair> items, Vector2Int position)
    {
        //have a pool of item receives (currently statically three, possibly make it dynamic in future)
        int targetId;
        if (!itemReceiveIsPlaying[0]) targetId = 0;
        else if (!itemReceiveIsPlaying[1]) targetId = 1;
        else targetId = 2;

        itemReceive[targetId].rectTransform.parent.position = Helper.Vec2toVec3(position);
        itemReceive[targetId].rectTransform.parent.rotation = Quaternion.Euler(0, CameraController.instance.angle, 0);
        itemReceive[targetId].text = "";
        foreach(ItemPair item in items)
        {
            itemReceive[targetId].text += item.item.name + " +" + item.amount + "\n";
        }
        StartCoroutine(ItemReceiveAnimation(targetId));
    }

    public AnimationCurve itemReceiveFade;

    private IEnumerator ItemReceiveAnimation(int targetId)
    {
        itemReceiveIsPlaying[targetId] = true;

        Transform parent = itemReceive[targetId].rectTransform.parent;
        parent.gameObject.SetActive(true);
        Vector3 parentStartPosition = parent.position + Vector3.up;
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime * 1.33f;
            yield return null;
            parent.position = Vector3.Lerp(parentStartPosition, parentStartPosition + Vector3.up * 0.2f, Helper.EaseInOut(time));
            itemReceive[targetId].color = new Color(1, 1, 1,  itemReceiveFade.Evaluate(time));
        }
        parent.gameObject.SetActive(false);

        itemReceiveIsPlaying[targetId] = false;
    }
    #endregion
}
