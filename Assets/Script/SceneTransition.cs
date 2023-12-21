using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject transitionUI;

    public void TransitionIn() => StartCoroutine(TransIn());

    private IEnumerator TransIn()
    {
        transitionUI.SetActive(true);
        Time.timeScale = 1;
        float time = 0;
        while (time < 0.5f)
        {
            transitionUI.transform.localScale = Vector3.one * Mathf.Lerp(200f, 1f, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        transitionUI.SetActive(false);
    }

    public void TransitionOut() => StartCoroutine(TransOut());

    private IEnumerator TransOut()
    {
        transitionUI.SetActive(true);
        Time.timeScale = 1;
        float time = 0;
        while (time < 0.5f)
        {
            transitionUI.transform.localScale = Vector3.one * Mathf.Lerp(1f, 200f, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
