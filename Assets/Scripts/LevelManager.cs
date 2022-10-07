using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int _level;

    const float c_normalSpeedTickDuration = 0.3f;
    const float c_slowSpeedTickDuration = 1f;

    public static LevelManager Instance { get; private set; }
    public bool[] Challenges { get { return CheckChallenges(); } }
    public float RunningDuration { get; private set; }
    public float TickDuration = 0.3f;

    public UnityEvent OnFinished, OnTimeOver;

    [SerializeField]
    RobotManager _activeRobot;
    [SerializeField]
    PositionMarker[] _positionMarkers;
    [SerializeField]
    Transform _wheatField;
    [SerializeField]
    Transform _bridge;

    bool _won = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResetDuration() { RunningDuration = 0f; }

    #region Controls
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

    public void Tick()
    {
        CodeManager.Instance.CompileCode();
        if (CodeManager.Instance.ErrorLine != -1) return;
        if (CodeManager.Instance.Commands.Length == CodeManager.Instance.Line) { _activeRobot.Tick(); return; }
        if (CodeManager.Instance.Commands.Length + 1 == CodeManager.Instance.Line)
        {
            ResetLevel();
            return;
        }
        if (CodeManager.Instance.Commands.Length == 0) return;

        TickDuration = c_slowSpeedTickDuration;
        RunningDuration += c_normalSpeedTickDuration;

        CodeManager.Instance.RunCommand();
        _activeRobot.Tick();
    }

    public void Stop()
    {
        StopAllCoroutines();
        ResetLevel();
    }

    public void ResetLevel()
    {
        _won = false;
        RunningDuration = 0;
        _activeRobot.transform.position = Vector3.zero;
        _activeRobot.transform.eulerAngles = Vector3.zero;
        _activeRobot.ResetRobot();
        CodeManager.Instance.ResetMemory();
        CodeManager.Instance.CompileCode();
        foreach (var marker in _positionMarkers) marker.Checked = false;
        ResetWheatField();
        if (_bridge != null)
        {
            if (_bridge.TryGetComponent(out Collider collider)) collider.enabled = true;
            _bridge.GetChild(0).eulerAngles = new Vector3(-90, 0, 0);
        }
    }

    void Run()
    {
        ResetLevel();
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
                if (_won) { break; }

                yield return new WaitForSeconds(TickDuration);
                RunningDuration += c_normalSpeedTickDuration;

                interate = CodeManager.Instance.RunCommand();
                _activeRobot.Tick();
            }
        }
    }
    #endregion

    #region Challenges
    bool[] CheckChallenges()
    {
        if (_level == 1) return CheckChallengesLevel1();
        if (_level == 2) return CheckChallengesLevel2();
        if (_level == 3) return CheckChallengesLevel3();
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

        if (!_won && challenges.Where(c => !c).ToArray().Length == 0)
        {
            _won = true;
            if (SceneManager.Instance.LastUnlockedLevel < _level)
                SceneManager.Instance.LastUnlockedLevel = _level;
            OnFinished.Invoke();
        }
        return challenges;
    }

    public bool[] CheckChallengesLevel2()
    {
        bool[] challenges = new bool[3];
        challenges[0] = _positionMarkers[0].Checked;

        challenges[1] = _positionMarkers[1].Checked;

        challenges[2] = _positionMarkers[2].Checked;

        if (!_won && challenges.Where(c => !c).ToArray().Length == 0)
        {
            _won = true;
            if (SceneManager.Instance.LastUnlockedLevel < _level)
                SceneManager.Instance.LastUnlockedLevel = _level;
            OnFinished.Invoke();
        }
        return challenges;
    }

    public bool[] CheckChallengesLevel3()
    {
        bool[] challenges = new bool[3] { false, false, false };

        challenges[0] = 2 < CodeManager.Instance.Memory[4];
        challenges[1] = !_bridge.GetComponent<Collider>().enabled;
        challenges[2] = _positionMarkers[0].Checked;

        if (!_won && challenges.Where(c => !c).ToArray().Length == 0)
        {
            _won = true;
            if (SceneManager.Instance.LastUnlockedLevel < _level)
                SceneManager.Instance.LastUnlockedLevel = _level;
            OnFinished.Invoke();
        }
        return challenges;
    }
    #endregion

    void ResetWheatField()
    {
        if (_wheatField == null) return;
        for (int i = 0; i < _wheatField.childCount; i++)
        {
            var child = _wheatField.GetChild(i);
            child.localScale = Vector3.one;
            if (child.TryGetComponent(out Collider collider)) collider.enabled = true;
            var renderer = child.GetComponentInChildren<SpriteRenderer>();
            if (renderer != null) renderer.enabled = true;
        }
    }
}
