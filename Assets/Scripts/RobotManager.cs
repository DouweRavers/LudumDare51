using UnityEngine;


public class RobotManager : MonoBehaviour
{
    public void ResetRobot()
    {
        if (TryGetComponent(out MovePart movePart)) movePart.ResetPart();
        if (TryGetComponent(out DetectPart detectPart)) detectPart.ResetPart();
        if (TryGetComponent(out CutPart cutPart)) cutPart.ResetPart();
        if (TryGetComponent(out InterfacePart interfacePart)) interfacePart.ResetPart();
    }

    public void Tick()
    {
        if (TryGetComponent(out MovePart movePart)) movePart.Tick();
        if (TryGetComponent(out DetectPart detectPart)) detectPart.Tick();
        if (TryGetComponent(out CutPart cutPart)) cutPart.Tick();
        if (TryGetComponent(out InterfacePart interfacePart)) interfacePart.Tick();
    }
}
