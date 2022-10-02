using UnityEngine;


public class RobotManager : MonoBehaviour
{

    public void Tick()
    {
        if (TryGetComponent(out MovePart movePart)) movePart.Tick();
        if (TryGetComponent(out DetectPart detectPart)) detectPart.Tick();
        if (TryGetComponent(out CutPart cutPart)) cutPart.Tick();
        if (TryGetComponent(out InterfacePart interfacePart)) interfacePart.Tick();
    }
}
