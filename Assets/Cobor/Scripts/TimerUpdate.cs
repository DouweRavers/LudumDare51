using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUpdate : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _textMeshProUGUI;

    void Update()
    {
        GetComponent<Slider>().value = LevelManager.Instance.RunningDuration;
        _textMeshProUGUI.text = LevelManager.Instance.RunningDuration.ToString("#.00") + " seconds";
    }
}
