using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    [SerializeField]
    CinemachineVirtualCamera _overviewCam, _roboCam;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetOverview(bool overviewMode)
    {
        if (overviewMode) SwitchToOverview();
        else SwitchToRoboCam();
    }

    public float SwitchToOverview()
    {
        _overviewCam.Priority = 20;
        _roboCam.Priority = 0;
        return Mathf.Clamp(Vector3.Distance(_overviewCam.transform.position, _overviewCam.transform.position) / 20f, 0, 1f);
    }

    public float SwitchToRoboCam()
    {
        _overviewCam.Priority = 0;
        _roboCam.Priority = 20;
        return Mathf.Clamp(Vector3.Distance(_overviewCam.transform.position, _overviewCam.transform.position) / 20f, 0, 1f);
    }
}
