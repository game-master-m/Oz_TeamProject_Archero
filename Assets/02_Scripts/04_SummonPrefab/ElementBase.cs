using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementBase : MonoBehaviour
{
    public abstract void OnHitTarget(EnemyBase target);
}
