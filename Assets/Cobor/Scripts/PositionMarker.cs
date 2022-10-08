using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PositionMarker : MonoBehaviour
{
    public bool Checked = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!Checked) LevelManager.Instance.ResetDuration();
        Checked = true;
    }
}
