using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RatsUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;


    private void Awake()
    {
        PlayerFlute.UpdateRemainingRats += DoUpdateRemainingRats;
    }

    private void OnDestroy()
    {
        PlayerFlute.UpdateRemainingRats -= DoUpdateRemainingRats;
    }
    private void Start()
    {
        
    }

    private void DoUpdateRemainingRats(int rats)
    {
        SetRemainingRats(rats);
    }

    public void SetRemainingRats(int rats)
    {
        text.text = "Remaining Rats: " + rats;
    }
}
