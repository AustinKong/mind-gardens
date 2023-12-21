using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioSource shearSFX, soilSFX, waterSFX;

    public void PlayShearSFX()
    {
        shearSFX.pitch = Time.timeScale;
        shearSFX.Play();
    }

    public void PlaySoilSFX()
    {
        soilSFX.pitch = Time.timeScale;
        soilSFX.Play();
    }

    public void PlayWaterSFX()
    {
        waterSFX.pitch = Time.timeScale;
        waterSFX.Play();
    }

    
}
