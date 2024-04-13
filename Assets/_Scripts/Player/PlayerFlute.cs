using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlute : MonoBehaviour
{
    //Rats


    [SerializeField] public int maxNumRats;
    private int _numDroppedRats = 0;

    [Header("Rats")]
    [SerializeField] private Rat _packRats;


    private Rat[] _droppedRats;

    //Keys
    private char[] _keyList;

    private float _timeSinceLastKey = 0;
    [SerializeField] private float _timeToResetKeys = 2f;

    private void Awake()
    {
        _keyList = new char[4];
        _droppedRats = new Rat[maxNumRats];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddNewNode('u');
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddNewNode('i');
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddNewNode('o');
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddNewNode('p');
        }

        if(_timeSinceLastKey<_timeToResetKeys)
        {
            _timeSinceLastKey += Time.deltaTime;
            if(_timeSinceLastKey>=_timeToResetKeys)
            {
                ResetKeyList();
            }
        }
    }
    
    private void ResetKeyList()
    {
        for(int i=0;i<_keyList.Length;i++)
        {
            _keyList[i] = '0';
        }
    }

    private void AddNewNode(char key)
    {
        //shift keys down list
        for(int i= _keyList.Length-1; i>=1;i--)
        {
            _keyList[i] = _keyList[i - 1];
        }
        _keyList[0] = key;

        //PrintKeys();

        _timeSinceLastKey = 0;
        CheckForMatch();
    }
    private void PrintKeys()
    {
        for(int i=0;i<_keyList.Length;i++)
        {
            print(_keyList[i]);
        }
    }

    private void CheckForMatch()
    {
        if(_keyList[3]=='u' && _keyList[2] == 'i' && _keyList[1] == 'o' && _keyList[0] == 'p')
        {
            ResetRats();
            ResetKeyList();
        }
        if (_keyList[3] == 'u' && _keyList[2] == 'i' && _keyList[1] == 'u' && _keyList[0] == 'i')
        {
            SpawnPackRats();

            ResetKeyList();
        }
    }

    private void ResetRats()
    {
        print("RESET RATS");

        for(int i=0;i<_numDroppedRats;i++)
        {
            Rat rat = _droppedRats[i];
            if(rat!=null)
            {
                _droppedRats[i] = null;
                rat.Reset();
            }
        }

        _numDroppedRats = 0;

    }

    private void SpawnPackRats()
    {
        if(_numDroppedRats<maxNumRats)
        {
            Vector2 spawnPos = transform.position;
            if (Player.Inst.controller._isFacingRight)
            {
                spawnPos = new Vector2(spawnPos.x + 2, spawnPos.y);
            }
            else
            {
                spawnPos = new Vector2(spawnPos.x - 2, spawnPos.y);
            }

            Rat rat = Instantiate(_packRats, spawnPos, Quaternion.identity);
            _droppedRats[_numDroppedRats] = rat;
            _numDroppedRats++;
        }

    }


}
