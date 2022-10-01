using UnityEngine;
using UnityEngine.UI;

public class TimerUpdate : MonoBehaviour
{
    void Update()
    {
        GetComponent<Slider>().value = LevelManager.Instance.RunningDuration;
    }
}
