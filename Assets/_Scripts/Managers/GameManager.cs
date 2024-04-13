using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _targetFramerate = 140;

    public State state = State.Playing;

    public static GameManager Inst;
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

    public enum State
    {
        Playing,
        InCutscene,
        Paused
    }
}
