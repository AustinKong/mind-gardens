using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Play()
    {
        SceneTransition.instance.TransitionOut();
        StartCoroutine(PlayScene());
    }

    private IEnumerator PlayScene()
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(1);
    }

    public GameObject controlsPanel;
    public GameObject creditsPanel;

    bool showingControls = false;
    public void Controls()
    {
        if (!showingControls)
        {
            StartCoroutine(ShowControls());
        }
        else
        {
            controlsPanel.SetActive(false);
            SceneTransition.instance.TransitionIn();
        }
        showingControls = !showingControls;
    }

    private IEnumerator ShowControls()
    {
        SceneTransition.instance.TransitionOut();
        yield return new WaitForSeconds(0.5f);
        controlsPanel.SetActive(true);
    }

    bool showingCredits = false;
    public void Credits()
    {
        if (!showingCredits)
        {
            StartCoroutine(ShowCredits());
        }
        else
        {
            creditsPanel.SetActive(false);
            SceneTransition.instance.TransitionIn();
        }
        showingCredits = !showingCredits;
    }

    private IEnumerator ShowCredits()
    {
        SceneTransition.instance.TransitionOut();
        yield return new WaitForSeconds(0.5f);
        creditsPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
