using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutscene : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x - Time.fixedDeltaTime * 3, transform.position.y);
    }
}
