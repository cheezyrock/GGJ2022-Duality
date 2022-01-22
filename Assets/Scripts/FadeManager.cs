using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    public float fadeInTime;
    public float fadeOutTime;

    public float startFadeOutAfterTime;
    public string sceneToLoadAfterFadeOut;

    public Image fadeImage;


    [HideInInspector]
    public bool transition = false;

    private void Awake ()
    {
        Instance = this;
    }

    void Start ()
    {
        Fade();

        if (startFadeOutAfterTime > 0)
        {
            StartCoroutine(StartFadeAfter());
        }
    }

    void Update()
    {
        if (transition)
        {
            Fade();
        }
    }

    public void SetTransition()
    {
        transition = true;
        StartCoroutine(Transition());
    }

    public void SetNextScene(string sceneName)
    {
        sceneToLoadAfterFadeOut = sceneName;
    }

    public IEnumerator StartFadeAfter ()
    {
        yield return new WaitForSeconds(startFadeOutAfterTime);
        transition = true;
        StartCoroutine(Transition());
        Fade();
    }

    public IEnumerator Transition()
    {
        yield return new WaitForSeconds(fadeOutTime);
        if (sceneToLoadAfterFadeOut != "")
        {
            SceneManager.LoadScene(sceneToLoadAfterFadeOut);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

    }

    public void Fade()
    {
        float fTime = (transition ? fadeOutTime : fadeInTime);
        float target = (transition ? 1 : 0f);
        transition = false;
        fadeImage.CrossFadeAlpha(target, fTime, false);

    }


}
