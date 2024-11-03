using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider imgFill;
    [SerializeField] private TextMeshProUGUI txtPercent;
    private float targetValue;

    public void Init()
    {
        imgFill.value = targetValue = 0;
    }

    private void Update()
    {
        if (imgFill.value < targetValue)
        {
            imgFill.value += Time.deltaTime / 5f;
        }

        txtPercent.text = $"{(int)(imgFill.value * 100)}%";
    }

    public void UpdateTarget(float updateValue)
    {
        if (targetValue + updateValue > 1)
        {
            targetValue = 1;
            return;
        }

        targetValue += updateValue;
    }
}