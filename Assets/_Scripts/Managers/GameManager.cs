using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _targetFramerate = 140;

    public State state = State.Playing;

    public static GameManager Inst;
    private State _oldState = State.Playing;
    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Inst = this;
        }

        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = _targetFramerate;
    }

    public void UpdateState(State s)
    {
        state = s;
    }
    public void PauseGame()
    {
        if (state != State.Paused)
        {
            _oldState = state;
            UpdateState(State.Paused);
            Time.timeScale = 0;
        }

    }
    public void ResumeGame()
    {
        UpdateState(_oldState);
        if (_oldState == State.Paused) UpdateState(State.Playing);
        Time.timeScale = 1;
    }
    public void PauseGame(float time)
    {
        if (state != State.Paused) StartCoroutine(DoPauseGame(time));
    }
    IEnumerator DoPauseGame(float time)
    {

        _oldState = state;
        UpdateState(State.Paused);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        UpdateState(_oldState);
    }

    public enum State
    {
        Playing,
        InCutscene,
        Paused
    }
}
