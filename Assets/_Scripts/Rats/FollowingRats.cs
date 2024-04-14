using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingRats : MonoBehaviour
{

    [SerializeField] private FollowingRat[] _rats;


    private void Awake()
    {
        PlayerFlute.UpdateRemainingRats += DoUpdateRemainingRats;
    }

    private void OnDestroy()
    {
        PlayerFlute.UpdateRemainingRats -= DoUpdateRemainingRats;
    }


    private void DoUpdateRemainingRats(int rats)
    {
        if (_rats.Length < rats) return;
        for(int i=0;i<rats;i++)
        {
            if(!_rats[i].gameObject.activeSelf)
            {
                _rats[i].Clear();
                _rats[i].transform.position = Player.Inst.controller.transform.position;
            }
            _rats[i].gameObject.SetActive(true);
        }
        for(int i=rats;i<_rats.Length;i++)
        {
            if (_rats[i].gameObject.activeSelf)
            {
                Player.Inst.EmitDeathParticles(_rats[i].transform.position, 3);
            }
            _rats[i].gameObject.SetActive(false);
        }
    }
}
