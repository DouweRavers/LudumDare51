using UnityEngine;


public class RobotManager : MonoBehaviour
{

    public void Tick()
    {
        if (TryGetComponent(out MovePart movePart)) movePart.Tick();
    }
}
