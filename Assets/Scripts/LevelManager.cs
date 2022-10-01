using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int _level;

    const float c_normalSpeedTickDuration = 0.25f;
    const float c_slowSpeedTickDuration = 1f;

    public static LevelManager Instance { get; private set; }
    public bool[] Challenges { get { return CheckChallenges(); } }
    public float RunningDuration { get; private set; }
    public float TickDuration = 0.5f;

    public UnityEvent OnFinished, OnTimeOver;

    [SerializeField]
    RobotManager _activeRobot;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RunNormal()
    {
        TickDuration = c_normalSpeedTickDuration;
        Run();
    }

    public void RunSlow()
    {
        TickDuration = c_slowSpeedTickDuration;
        Run();
    }

    void Run()
    {
        _activeRobot.transform.position = Vector3.zero;
        _activeRobot.transform.eulerAngles = Vector3.zero;
        CodeManager.Instance.Reset();
        CodeManager.Instance.CompileCode();
        if (CodeManager.Instance.ErrorLine != -1) return;

        StartCoroutine(runCode());
        IEnumerator runCode()
        {
            bool interate = CodeManager.Instance.Commands.Length > 0;
            while (interate)
            {
                if (10 < RunningDuration + c_normalSpeedTickDuration)
                {
                    OnTimeOver.Invoke();
                    break;
                }
                yield return new WaitForSeconds(TickDuration);
                RunningDuration += c_normalSpeedTickDuration;
                interate = CodeManager.Instance.RunCommand();
                _activeRobot.Tick();
            }
        }
    }

    public void Tick()
    {
        TickDuration = c_slowSpeedTickDuration;
        CodeManager.Instance.CompileCode();
        if (CodeManager.Instance.ErrorLine != -1) return;
        if (CodeManager.Instance.Commands.Length == 0) return;
        bool endProgram = CodeManager.Instance.RunCommand();
        _activeRobot.Tick();
        RunningDuration += c_normalSpeedTickDuration;

        if (!endProgram) Stop();
    }

    public void Stop()
    {
        RunningDuration = 0;
        StopAllCoroutines();
        _activeRobot.transform.position = Vector3.zero;
        _activeRobot.transform.eulerAngles = Vector3.zero;
        CodeManager.Instance.Reset();
    }

    #region Challenges
    bool[] CheckChallenges()
    {
        if (_level == 1) return CheckChallengesLevel1();
        return new bool[0];
    }

    public bool[] CheckChallengesLevel1()
    {
        bool[] challenges = new bool[3];
        challenges[0] = CodeManager.Instance.Memory[0] == 1 &&
            CodeManager.Instance.Memory[2] == 2 &&
            CodeManager.Instance.Memory[5] == 3;

        challenges[1] = CodeManager.Instance.Commands.Length > 0 &&
            CodeManager.Instance.Commands.Where((c) => c.Type == CommandType.ADD).ToArray().Length > 0 &&
            CodeManager.Instance.Memory[0] == 1 &&
            CodeManager.Instance.Memory[1] == 3 &&
            CodeManager.Instance.Memory[2] == 2;

        challenges[2] = CodeManager.Instance.Commands.Length > 0 &&
            CodeManager.Instance.Commands.Where((c) => c.Type == CommandType.SKIP).ToArray().Length > 0;
        if (challenges.Where(c => !c).ToArray().Length == 0) OnFinished.Invoke();
        return challenges;
    }
    #endregion
}
