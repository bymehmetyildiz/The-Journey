using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/ Dead State")]
public class D_DeadState : ScriptableObject
{  

    public float knockBackTime = 0.2f;
    public float knockBackSpeed = 4f;
    public Vector2 knockbackAngle;
}
