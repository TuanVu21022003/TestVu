using TMPro;
using UnityEngine;

public class SlotLoading : SlotWaiting
{
    [SerializeField] private TMP_Text loadingText;

    public void SetProgress(float progress)
    {
        loadingText.text = (int)(progress * 100) + "%";
    }
}