using System.Collections;
using UnityEngine;

public class MovePart : MonoBehaviour
{
    float _timePassed = 0;
    Vector3 _moveDir = Vector3.zero;
    Vector3 _moveDest = Vector3.zero;

    public void ResetPart()
    {
        _moveDir = Vector3.zero;
        _moveDest = Vector3.zero;
    }

    public void Tick()
    {
        switch (CodeManager.Instance.Memory[0])
        {
            case 1: Move(Vector3.forward * 5); break;
            case 2: Move(Vector3.back * 5); break;
            case 3: Move(Vector3.right * 5); break;
            case 4: Move(Vector3.left * 5); break;
            default: break;
        }
        if (CodeManager.Instance.Memory[1] == 0) CodeManager.Instance.Memory[0] = 0;
    }


    private void Update()
    {
        _timePassed += Time.deltaTime;
        if (_moveDir == Vector3.zero || LevelManager.Instance.TickDuration <= _timePassed) return;
        transform.position = Vector3.Lerp(transform.position, _moveDest, _timePassed / LevelManager.Instance.TickDuration);
    }

    void Move(Vector3 dir)
    {
        _timePassed = 0;
        transform.position = _moveDest;
        transform.LookAt(_moveDest + dir);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 5)) _moveDir = Vector3.zero;
        else
        {
            _moveDir = dir;
            _moveDest += dir;
        }
    }

    void Move1(Vector3 dir)
    {
        transform.LookAt(transform.position + dir);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 5)) return;

        StartCoroutine(MoveCoroutine());
        IEnumerator MoveCoroutine()
        {
            var steps = 60;
            var stepTime = LevelManager.Instance.TickDuration / steps;
            var originalPos = transform.position;
            var destinationPos = transform.position + dir;
            GetComponent<Animator>().SetBool("Driving", true);
            for (int i = 1; i <= (int)steps; i++)
            {
                yield return new WaitForSeconds(stepTime);
                transform.position = Vector3.Lerp(originalPos, destinationPos, i / 30f);
            }
            GetComponent<Animator>().SetBool("Driving", false);
        }
    }
}
