using UnityEngine;

public class DetectPart : MonoBehaviour
{
    public void ResetPart()
    {
        CodeManager.Instance.Memory[2] = 0;
    }

    public void Tick()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward * 5, 5))
            CodeManager.Instance.Memory[2] = 1;
        else
            CodeManager.Instance.Memory[2] = 0;
    }
}
