using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour
{
    [Serializable]
    public class ImageButton
    {
        public string title;
        public Sprite image;
    }

    [Serializable]
    public enum button_id
    {
        Open,
        Extend
    }

    [SerializeField]
    private button_id id;

    [Header("THIS SCRIPT DOESN'T NEED TO ADDED VIA INSPECTOR")]
    [SerializeField] private List<ImageButton> _images;
    private bool isClicked = false;
    private Button thisButton;
    private AR_InfoManager infoManager;

    private void Awake()
    {
        if (infoManager == null)
            infoManager = FindObjectOfType<AR_InfoManager>();

        if (thisButton == null)
            thisButton = this.GetComponent<Button>();
    }
    private void Start()
    {
      
        thisButton.onClick?.AddListener(ChangeSprite);
        infoManager.OnButtonClick?.AddListener(ChangeSprite);
        Debug.Log($"[BUTTON] This ID {id}");
    }

    public void ChangeSprite()
    {
        isClicked = isClicked == false ? true : false;
        thisButton.image.sprite = isClicked == true ? _images.Find(x => x.title == "Open").image : _images.Find(x => x.title == "Close").image;
    }

    public void ChangeSprite(bool _isClicked,button_id _id)
    {
        if(_id != id) return;
        isClicked = _isClicked;
        thisButton.image.sprite = isClicked == true ? _images.Find(x => x.title == "Open").image : _images.Find(x => x.title == "Close").image;
    }
}
