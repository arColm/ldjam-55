using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneRestart : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Inst.StartGame();
        }
    }
}
