using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public virtual void UseEffect()
    {
        Debug.Log("Used " + name);
    }
}
