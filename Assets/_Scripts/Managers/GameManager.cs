using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _targetFramerate = 140;

    public State state = State.Playing;

    public static GameManager Inst;
    private State _oldState = State.Playing;

    [SerializeField] private Animator _blackScreenAnimator;
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

    public void EndGame()
    {

        StartCoroutine(DoSetScene(1));
    }
    public void StartGame()
    {
        StartCoroutine(DoSetScene(0));
    }

    private IEnumerator DoSetScene(int index)
    {
        FadeToBlack();
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(index);
        yield return new WaitForSeconds(1);
        FadeToShow();
        yield return new WaitForSeconds(1);
    }

    void FadeToBlack()
    {
        _blackScreenAnimator.Play("FadeToBlack");
    }

    void FadeToShow()
    {
        _blackScreenAnimator.Play("FadeToTransparent");
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
