using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class OpenLinkHelper : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void BackToPreviousWeb();

    public void ButtonBack()
    {
        BackToPreviousWeb();
        Debug.Log($"[BUTTON] Button Back is pressed to ");
    }
}