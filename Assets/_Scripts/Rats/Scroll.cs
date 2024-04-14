using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    //fix this later

    [SerializeField] private GameObject _scrollObject;
    [Header("Fix this later maybe")]

    [SerializeField] int scrollValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Player.Inst.tag))
        {
            switch(scrollValue)
            {
                case 0:
                    Player.Inst.PackRatsFound = true;
                    break;
                case 1:
                    Player.Inst.ResetRatsFound = true;
                    break;
                case 2:
                    Player.Inst.BulletRatsFound = true;
                    break;
                case 3:
                    Player.Inst.AntiGravityRatFound = true;
                    break;
            }
            _scrollObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.Inst.tag))
        {
            _scrollObject.SetActive(false);
        }
    }
}
