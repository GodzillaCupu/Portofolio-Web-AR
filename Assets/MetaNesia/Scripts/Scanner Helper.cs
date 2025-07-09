
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Imagine.WebAR;
using UnityEngine;

public class ScannerHelper : MonoBehaviour
{
    [SerializeField] private ImageTracker _iTracker;
    [SerializeField] private AR_InfoManager _InfoManager;
    [SerializeField] private GameObject panelScanInfo;


    [SerializeField] private GameObject targetAnimation;
    [SerializeField] private GameObject buttonClose;
    [SerializeField] private Vector3 targetScaleAnimation;
    [SerializeField] private float durationAnimation;

    void Awake()
    {
        if (_iTracker == null) Debug.LogError("[I_TRACKER] iTracker is Empty, Check Again");
        if (_InfoManager == null) Debug.LogError("[AR_INFO] AR_InfoManager is Empty, Check Again");
        if (panelScanInfo == null) Debug.LogError("[PANEL] Panel Info is Empty, Check Again");
    }

    void Start()
    {
        _iTracker.OnMarkerDetected.AddListener(DisplayPanelScanner);
    }

    private void DisplayPanelScanner(bool isDisplay)
    {
        panelScanInfo.SetActive(isDisplay == true ? false : true);
        // buttonClose.SetActive(isDisplay == true? true : false);
        _InfoManager.ClosePanel();
        // AnimateLogoScanner(targetScaleAnimation);
        Debug.Log($"[PANEL SCANNER] Panel Is {panelScanInfo.activeInHierarchy}");
    }

    // private void AnimateLogoScanner(Vector3 targetScale)
    // {
    //     if (panelScanInfo.activeInHierarchy == false) return;
    //     targetAnimation.transform.LeanScale(targetScale, durationAnimation).setEaseInOutQuart().setLoopPingPong();
    //     Debug.Log($"[PANEL SCANNER] Panel Succsess to Animate");
    // }
}
