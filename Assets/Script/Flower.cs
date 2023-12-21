using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flower", menuName = "ScriptableObjects/Flower")]
public class Flower : ScriptableObject
{
    public new string name;
    public string scientificName;
    public GameObject[] models;

    [Header("Lifecycle")]
    public int growthDuration;
    public int blossomDuration;

    [Header("Mood affectors (x10^-4)")]
    [Range(-10f, 10f)]
    public float happy_sad;
    [Range(-10f, 10f)]
    public float calm_anxious;
    [Range(-10f, 10f)]
    public float energized_tired;

    [Header("Other info")]
    public int crowdingTolerance;
    public Item seed;
}
