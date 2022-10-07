using UnityEngine;

public class InterfacePart : MonoBehaviour
{
    bool _communicationStarted = false;
    int _myCode = -1;

    public void ResetPart()
    {
        _communicationStarted = false;
        _myCode = -1;
    }

    public void Tick()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward * 5, out RaycastHit hit, 5))
        {
            if (hit.collider.gameObject.layer != 7) return;
            if (!_communicationStarted)
            {
                CodeManager.Instance.Memory[5] = 1;
                _communicationStarted = CodeManager.Instance.Memory[6] == 1;
                return;
            }
            else
            {
                if (_myCode == -1)
                {
                    _myCode = Random.Range(10, 100);
                    CodeManager.Instance.Memory[5] = _myCode;
                }
                else
                {
                    if (CodeManager.Instance.Memory[6] == (_myCode * 13) / 2 + 98)
                    {
                        CodeManager.Instance.Memory[5] = 0;
                        hit.collider.GetComponent<Animation>().Play();
                        LevelManager.Instance.ResetDuration();
                    }
                }
            }
        }
        else ResetPart();
    }
}
