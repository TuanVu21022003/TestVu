using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrepareGame : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textPrepare;
    private Action action;
    public void Show(Action action)
    {
       this.action = action;
        gameObject.SetActive(true);
        OnHandlePrepare();
    }

    public void Hide()
    {
        action?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnHandlePrepare()
    {       
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        // Chuỗi số đếm ngược
        int[] countdownNumbers = { 3, 2, 1 };
        textPrepare.rectTransform.localPosition = Vector3.zero;
        foreach (int number in countdownNumbers)
        {
            // Cập nhật text và hiển thị
            textPrepare.text = number.ToString();
            textPrepare.rectTransform.localScale = Vector3.one * 6; // Đặt scale về 6

            // Thực hiện hiệu ứng scale nhỏ lại
            textPrepare.rectTransform.DOScale(0.1f, 1f).SetEase(Ease.InElastic);

            // Đợi 1 giây cho mỗi số
            yield return new WaitForSeconds(1f);
        }

        // Hiển thị "StartGame" sau khi đếm ngược xong
        textPrepare.text = "Start Game";
        textPrepare.rectTransform.localScale = Vector3.one * 6;
        textPrepare.rectTransform.DOLocalMoveX(700, 1f).SetEase(Ease.InElastic).OnComplete(Hide);
    }
}
