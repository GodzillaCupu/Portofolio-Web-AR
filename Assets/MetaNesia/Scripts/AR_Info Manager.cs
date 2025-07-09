using System;
using System.Collections;
using System.Collections.Generic;
using Imagine.WebAR;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class AR_InfoManager : MonoBehaviour
{
    [Serializable]
    public enum PositionMarker
    {
        Left,
        Right,
        All
    }

    [Serializable]
    public class ListContent
    {
        public string namaIndo;
        public string namaEng;
        public string namaLat;

        [TextArea(1, 5)]
        public string contentText;
        public PositionMarker position;
    }

    [Serializable]
    public class PanelSize
    {
        public Vector2 _openSize = new Vector2(322, 150);
        public Vector2 _closeSize = new Vector2(322, 50);
    }

    [Serializable]
    public class DisplayText
    {
        public TextMeshProUGUI title;

        public TextMeshProUGUI content;

        public void SetTitle(string text) => title.text = text;

        public void SetContent(string text) => content.text = text;
    }

    [SerializeField]
    private ImageTracker tracker;
    private string targetName;
    private int targetDisplayId = 0;

    [SerializeField]
    private GameObject panelInfo;

    [Space(10), SerializeField]
    private PanelSize panelSize;

    [SerializeField]
    private DisplayText displayText;

    [SerializeField]
    private List<ListContent> contents;

    [HideInInspector]
    public UnityEngine.Events.UnityEvent<bool, ButtonHelper.button_id> OnButtonClick;
    private RectTransform panelInfoRect;
    private bool isPanelExtend = false;

    private void Awake()
    {
        if (tracker == null)
            Debug.LogError("[ITracker] is Empty, Check Again");
        if (panelInfo == null)
            Debug.LogError("[PANEL] panel AR Info is Empty, Check Again");
    }

    private void Start()
    {
        tracker.OnMarkerScan.AddListener(
            (x) =>
            {
                x = targetName;
                ChangeText(x);
            }
        );
        tracker.OnMarkerScan.AddListener(ChangeText);
        panelInfoRect =
            panelInfoRect == null ? panelInfo.GetComponent<RectTransform>() : panelInfoRect;
        panelInfo.SetActive(panelInfo.activeInHierarchy ? false : false);
    }

    public void ExtendPanel()
    {
        if (isPanelExtend == false)
        {
            isPanelExtend = true;
            panelInfoRect.LeanSize(panelSize._openSize, 1f).setEaseOutQuad();
        }
        else
        {
            isPanelExtend = false;
            panelInfoRect.LeanSize(panelSize._closeSize, 1f).setEaseOutQuad();
        }
    }

    public void ResetPanel()
    {
        isPanelExtend = false;
        OnButtonClick?.Invoke(true, ButtonHelper.button_id.Extend);
        panelInfoRect.LeanSize(panelSize._closeSize, 1f).setEaseOutQuad();
    }

    private void ChangeText(string id)
    {
        Debug.Log($"[IMAGE] display {id} as marker ");
        ResetPanel();
        CheckTextToDisplay(id);
    }

    private void CheckTextToDisplay(string pos)
    {
        ListContent contentToDisplay = new ListContent();
        string _markerLeft = "Marker_Sedang_L";
        string _markerRight = "Marker_Sedang_R";
        

        if(pos == null || pos == string.Empty)
        {
            Debug.LogError($"Marker Doesn't Exist : {pos}");
            return;
        }

        if(pos == _markerLeft)
        {
            contentToDisplay = contents.Find(x => x.position == PositionMarker.Left);
            Debug.Log($"[Content To Display name {contentToDisplay.namaEng}]");
        }
        else if(pos == _markerRight)
        {
            contentToDisplay = contents.Find(x => x.position == PositionMarker.Right);
            Debug.Log($"[Content To Display name {contentToDisplay.namaEng}]");
        }
        else
        {
            int _target = UnityEngine.Random.Range(0,1);
            contentToDisplay = contents[_target];
            targetDisplayId = _target;
            Debug.Log($"[Content To Display name {contentToDisplay.namaEng}]");
        }

        string dot = " • ";
        string displayTitleText =
            $"{contentToDisplay.namaIndo} {dot} <i>{contentToDisplay.namaEng}</i> ({contentToDisplay.namaLat})";
        displayText.SetTitle(displayTitleText);

        string displayContentText = contentToDisplay.contentText;
        displayText.SetContent(displayContentText);
    }

    public void ClosePanel()
    {
        ResetPanel();
        OnButtonClick?.Invoke(false, ButtonHelper.button_id.Open);
        panelInfo.SetActive(false);
        Debug.Log(
            $"[PANEL] AR PANEL IS SET TO {panelInfo.activeInHierarchy} BECAUSE NO MARKER DETECTED"
        );
    }

    public void OpenPanel()
    {
        ResetPanel();
        ChangeText(targetName);
        panelInfo.SetActive(panelInfo.activeInHierarchy ? false : true);
        Debug.Log($"[DESC] AR PANEL IS {targetName} ");
    }
}
