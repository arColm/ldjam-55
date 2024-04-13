using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlute : MonoBehaviour
{

    //events
    public static event Action<int> UpdateRemainingRats;

    //Rats


    [SerializeField] public int maxNumRats = 3;
    private int _numDroppedRats = 0;

    [Header("Rats")]
    [SerializeField] private Rat _packRats;
    [SerializeField] private BulletRat _bulletRat;


    private Rat[] _droppedRats;

    //Keys
    private char[] _keyList;

    private float _timeSinceLastKey = 0;
    [SerializeField] private float _timeToResetKeys = 2f;

    [Header("Sounds")]
    [SerializeField] private AudioClip _uSound;
    [SerializeField] private AudioClip _iSound;
    [SerializeField] private AudioClip _oSound;
    [SerializeField] private AudioClip _pSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _keyList = new char[4];
        _droppedRats = new Rat[maxNumRats];
        _audioSource = GetComponent<AudioSource>();
        CameraSwitcher.DoLevelSwitch += ResetRats;
    }

    private void OnDestroy()
    {
        CameraSwitcher.DoLevelSwitch -= ResetRats;
    }

    private void Update()
    {
        if(GameManager.Inst.state==GameManager.State.Playing)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                AddNewNode('u');
                _audioSource.PlayOneShot(_uSound);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                AddNewNode('i');
                _audioSource.PlayOneShot(_iSound);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                AddNewNode('o');
                _audioSource.PlayOneShot(_oSound);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                AddNewNode('p');
                _audioSource.PlayOneShot(_pSound);
            }
            if (_timeSinceLastKey < _timeToResetKeys)
            {
                _timeSinceLastKey += Time.deltaTime;
                if (_timeSinceLastKey >= _timeToResetKeys)
                {
                    ResetKeyList();
                }
            }
        }



    }
    
    public void UpgradeRats()
    {
        maxNumRats++;
        Rat[] newDroppedRats = new Rat[maxNumRats];
        for(int i=0;i<maxNumRats-1;i++)
        {
            newDroppedRats[i] = _droppedRats[i];
        }
        _droppedRats = newDroppedRats;

        UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);

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
        if (_keyList[3] == 'u' && _keyList[2] == 'i' && _keyList[1] == 'o' && _keyList[0] == 'p')
        {
            ResetRats();
            ResetKeyList();
        }
        if (_keyList[3] == 'u' && _keyList[2] == 'i' && _keyList[1] == 'u' && _keyList[0] == 'i')
        {
            SpawnPackRats();

            ResetKeyList();
        }
        if (_keyList[3] == 'o' && _keyList[2] == 'i' && _keyList[1] == 'u' && _keyList[0] == 'i')
        {
            SpawnBulletRat();
            ResetKeyList();
        }
    }
    private void SpawnBulletRat()
    {
        if (_numDroppedRats < maxNumRats)
        {
            Vector2 spawnPos = transform.position;
            if (Player.Inst.controller._isFacingRight)
            {
                spawnPos = new Vector2(spawnPos.x + 1, spawnPos.y);
            }
            else
            {
                spawnPos = new Vector2(spawnPos.x - 1, spawnPos.y);
            }

            BulletRat rat = Instantiate(_bulletRat, spawnPos, Quaternion.identity);
            rat.Instantiate(Player.Inst.controller._isFacingRight);
            _droppedRats[_numDroppedRats] = rat;
            _numDroppedRats++;
            UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);
        }
    }
    public void ResetRats()
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
        UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);

    }

    private void SpawnPackRats()
    {
        if(_numDroppedRats<maxNumRats)
        {
            Vector2 spawnPos = transform.position;
            if (Player.Inst.controller._isFacingRight)
            {
                spawnPos = new Vector2(spawnPos.x + 2, spawnPos.y+1);
            }
            else
            {
                spawnPos = new Vector2(spawnPos.x - 2, spawnPos.y+1);
            }

            Rat rat = Instantiate(_packRats, spawnPos, Quaternion.identity);
            _droppedRats[_numDroppedRats] = rat;
            _numDroppedRats++;
            UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);
        }

    }


}
