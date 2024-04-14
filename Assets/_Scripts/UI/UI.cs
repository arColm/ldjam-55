using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Inst;

    public RatsUI RatsUI;

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _packRatScroll;
    [SerializeField] private GameObject _resetRatScroll;
    [SerializeField] private GameObject _bulletRatScroll;
    [SerializeField] private GameObject _antiGravityRatScroll;
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

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeInHierarchy);
        if(Player.Inst.PackRatsFound)
        {
            EnablePackRatScroll();
        }
        if(Player.Inst.ResetRatsFound)
        {
            EnableResetRatScroll();
        }
        if(Player.Inst.BulletRatsFound)
        {
            EnableBulletRatScroll();
        }
        if(Player.Inst.AntiGravityRatFound)
        {
            EnableAntiGravityRatScroll();
        }
        if (_pauseMenu.activeSelf) GameManager.Inst.PauseGame();
        else GameManager.Inst.ResumeGame();
        
    }
    public void EnablePackRatScroll()
    {
        _packRatScroll.SetActive(true);
    }
    public void EnableResetRatScroll()
    {
        _resetRatScroll.SetActive(true);
    }
    public void EnableBulletRatScroll()
    {
        _bulletRatScroll.SetActive(true);
    }
    public void EnableAntiGravityRatScroll()
    {
        _antiGravityRatScroll.SetActive(true);
    }
}
