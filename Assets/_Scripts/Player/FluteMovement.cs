using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteMovement : MonoBehaviour
{
    private void Awake()
    {
        PlayerFlute.PlayNote += DoPlayNote;
    }
    private void OnDestroy()
    {
        PlayerFlute.PlayNote -= DoPlayNote;
    }

    private void DoPlayNote()
    {
        transform.localPosition = new Vector2(0.25f, 0);
        transform.localRotation = Quaternion.identity;
    }
    private void FixedUpdate()
    {
        Vector2 targetPosition = Vector2.zero;
        Quaternion targetRotation = Quaternion.identity;
        switch (Player.Inst.controller.state)
        {
            case PlayerController.State.Idle:
                targetPosition = new Vector2(0.25f, -0.25f);
                targetRotation = Quaternion.Euler(new Vector3(0, 0, -35f));
                break;
            case PlayerController.State.Walking:
                targetPosition = new Vector2(0f, -0.25f);
                targetRotation = Quaternion.Euler(new Vector3(0, 0, -35f));
                break;
            case PlayerController.State.Jumping:
                targetPosition = new Vector2(0.25f, -0.5f);
                targetRotation = Quaternion.Euler(new Vector3(0, 0, -75f));
                break;
            case PlayerController.State.Falling:
                targetPosition = new Vector2(0.5f, 0.25f);
                targetRotation = Quaternion.Euler(new Vector3(0, 0, 75f));
                break;
            case PlayerController.State.PlayingFlute:
                break;
        }

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPosition, Time.fixedDeltaTime);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.fixedDeltaTime*50);
    }
}
