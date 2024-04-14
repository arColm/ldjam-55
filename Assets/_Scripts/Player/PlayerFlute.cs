using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerFlute : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem _spawnRatParticles;
    [SerializeField] private ParticleSystem _musicNoteParticles;
    //events
    public static event Action<int> UpdateRemainingRats;
    public static event Action PlayNote;
    public static event Action ResetRatsEvent;

    //Rats


    
    [SerializeField] public int maxNumRats = 3;
    private int _numDroppedRats = 0;

    [Header("Rats")]
    [SerializeField] private Rat _packRats;
    [SerializeField] private BulletRat _bulletRat;
    [SerializeField] private Rat _antiGravityRat;

    [SerializeField] private LayerMask _groundLayer;


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
                CreateMusicNoteParticle('u', transform.position);
                PlayNote?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                AddNewNode('i');
                _audioSource.PlayOneShot(_iSound);
                CreateMusicNoteParticle('i', transform.position);
                PlayNote?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                AddNewNode('o');
                _audioSource.PlayOneShot(_oSound);
                CreateMusicNoteParticle('o', transform.position);
                PlayNote?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                AddNewNode('p');
                _audioSource.PlayOneShot(_pSound);
                CreateMusicNoteParticle('p', transform.position);
                PlayNote?.Invoke();
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

    private void CreateMusicNoteParticle(char key, Vector2 position)
    {
        EmitParams emitParams = new EmitParams();
        switch(key)
        {
            case 'u':
                emitParams.startColor = Color.red;
                break;
            case 'i':
                emitParams.startColor = Color.cyan;
                break;
            case 'o':
                emitParams.startColor = Color.green;
                break;
            case 'p':
                emitParams.startColor = Color.yellow;
                break;
        }
        emitParams.position = position;
        _musicNoteParticles.Emit(emitParams, 1);
    }

    private void CreateSpawnRatParticles(Vector2 position)
    {
        EmitParams emitParams = new EmitParams();
        emitParams.position = position;
        _spawnRatParticles.Emit(emitParams, 1);
    }

    private void CreateResetRatParticles(Vector2 position)
    {
        EmitParams emitParams = new EmitParams();
        emitParams.position = position;
        emitParams.startSize = 2;
        _spawnRatParticles.Emit(emitParams, 1);
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
            if(Player.Inst.ResetRatsFound)
            {
                ResetRats();
                CreateResetRatParticles(transform.position);
                //UI.Inst.EnableResetRatScroll();
                ResetKeyList();
            }

        }
        if (_keyList[3] == 'u' && _keyList[2] == 'i' && _keyList[1] == 'u' && _keyList[0] == 'i')
        {
            if(Player.Inst.PackRatsFound)
            {
                SpawnPackRats();
                //UI.Inst.EnablePackRatScroll();

                ResetKeyList();
            }

        }
        if (_keyList[3] == 'o' && _keyList[2] == 'i' && _keyList[1] == 'u' && _keyList[0] == 'i')
        {
            if(Player.Inst.BulletRatsFound)
            {
                SpawnBulletRat();
                //UI.Inst.EnableBulletRatScroll();
                ResetKeyList();
            }

        }
        if (_keyList[3] == 'p' && _keyList[2] == 'o' && _keyList[1] == 'i' && _keyList[0] == 'u')
        {
            if(Player.Inst.AntiGravityRatFound)
            {
                SpawnAntiGravityRat();
                //UI.Inst.EnableAntiGravityRatScroll();
                ResetKeyList();
            }

        }
    }
    private void SpawnBulletRat()
    {
        if (_numDroppedRats < maxNumRats)
        {
            PlayerController controller = Player.Inst.controller;
            bool shootRight = (controller._isFacingRight && !controller._inAntiGravity) || (!controller._isFacingRight && controller._inAntiGravity);
            Vector2 spawnPos = transform.position;
            if (shootRight)
            {
                spawnPos = new Vector2(spawnPos.x + 1, spawnPos.y);
            }
            else
            {
                spawnPos = new Vector2(spawnPos.x - 1, spawnPos.y);
            }
            CreateSpawnRatParticles(spawnPos);
            BulletRat rat = Instantiate(_bulletRat, spawnPos, Quaternion.identity);
            rat.Instantiate(shootRight);
            _droppedRats[_numDroppedRats] = rat;
            _numDroppedRats++;
            UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);
        }
    }
    public void ResetRats()
    {

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
        ResetRatsEvent?.Invoke();
        UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);

    }

    private void SpawnPackRats()
    {
        if (_numDroppedRats < maxNumRats)
        {
            Vector2 spawnPos = transform.position;
            if ((Player.Inst.controller._isFacingRight && !Player.Inst.controller._inAntiGravity) ||
                (!Player.Inst.controller._isFacingRight && Player.Inst.controller._inAntiGravity))
            {
                spawnPos = new Vector2(spawnPos.x + 2, spawnPos.y);
            }
            else
            {
                spawnPos = new Vector2(spawnPos.x - 2, spawnPos.y);
            }

            if(Player.Inst.controller._inAntiGravity)
            {
                spawnPos = new Vector2(spawnPos.x, spawnPos.y - 1);
            }
            else
            {
                spawnPos = new Vector2(spawnPos.x, spawnPos.y + 1);
            }

            Vector3 direction = (Vector3)spawnPos - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, _groundLayer);

            if(hit)
            {
                spawnPos = hit.point;
                if((Player.Inst.controller._isFacingRight && !Player.Inst.controller._inAntiGravity) ||
                    (!Player.Inst.controller._isFacingRight && Player.Inst.controller._inAntiGravity))
                {
                    spawnPos = new Vector2(spawnPos.x - 1, spawnPos.y);
                }
                else
                {
                    spawnPos = new Vector2(spawnPos.x + 1, spawnPos.y);
                }

                if (Player.Inst.controller._inAntiGravity)
                {
                    spawnPos = new Vector2(spawnPos.x, spawnPos.y - 0.3f);
                }
                else
                {
                    spawnPos = new Vector2(spawnPos.x, spawnPos.y + 0.3f);
                }
            }

            CreateSpawnRatParticles(spawnPos);
            Rat rat = Instantiate(_packRats, spawnPos, Quaternion.identity);
            _droppedRats[_numDroppedRats] = rat;
            _numDroppedRats++;
            UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);
        }

    }

    private void SpawnAntiGravityRat()
    {
        if (_numDroppedRats < maxNumRats)
        {
            Vector2 spawnPos = transform.position;

            CreateSpawnRatParticles(spawnPos);
            Rat rat = Instantiate(_antiGravityRat, spawnPos, Quaternion.identity);
            _droppedRats[_numDroppedRats] = rat;
            _numDroppedRats++;
            UpdateRemainingRats?.Invoke(maxNumRats - _numDroppedRats);
        }
    }


}
