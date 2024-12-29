using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newKnockBackStateData", menuName = "Data/State Data/ KnockBack State")]
public class D_KnockBackState : ScriptableObject
{
    public Vector2 knockBackSpeed;

    public float knockBackTime;
    public float knockBackDuration = 1f;

}
