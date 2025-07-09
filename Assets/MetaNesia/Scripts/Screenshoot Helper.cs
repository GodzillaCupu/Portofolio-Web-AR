using System.Collections;
using System.Collections.Generic;
using Imagine.WebAR;
using UnityEngine;

public class ScreenshootHelper : MonoBehaviour
{
    public GameObject UI;

    [SerializeField]
    private ScreenshotManager manager;

    public AudioClip clip;
    public AudioSource source;

    // public void TakeScreenshoot() => StartCoroutine(Screenshoot(UI));

    public void TakeScreenshoot() => StartCoroutine(ScreenCapture(UI));

    private IEnumerator Screenshoot(GameObject ui)
    {
        ui.SetActive(false);
        yield return new WaitForEndOfFrame();
        manager.GetScreenShot();
        ui.SetActive(true);
    }

    private IEnumerator ScreenCapture(GameObject ui)
    {
        //Disable ALL UI
        ui.SetActive(false);

        //Getting Screen Capture
        yield return new WaitForEndOfFrame();
        manager.GetScreenCapure();

        //Enable All Ui
        ui.SetActive(false);
    }
}
