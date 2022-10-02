using UnityEngine;

public class CutPart : MonoBehaviour
{
    public void Tick()
    {
        if (CodeManager.Instance.Memory[3] == 0) return;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward * 5, out RaycastHit hit, 5))
        {
            if (hit.collider.gameObject.layer != 6) return;
            hit.collider.GetComponent<Animation>().Play();
            CodeManager.Instance.Memory[3] = 0;
            CodeManager.Instance.Memory[4]++;
        }
    }
}