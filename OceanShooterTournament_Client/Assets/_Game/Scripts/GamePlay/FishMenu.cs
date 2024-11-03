using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishMenu : MonoBehaviour
{
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private GameObject menuList;
    private void Start()
    {
        menuBtn.onClick.AddListener(() =>
        {
            TouchMenuBtn();
        });
        exitBtn.onClick.AddListener(() =>
        {
            TouchExitBtn();
        });
    }

    public void TouchMenuBtn()
    {
        menuList.SetActive(!menuList.activeSelf);
    }

    public void TouchExitBtn()
    {
        NetworkManager.instance.Request("quit_room", $"");
    }
}
