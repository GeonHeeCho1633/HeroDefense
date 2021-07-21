using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : BaseEnemy
{
    public override void Create(Transform[] _WayPoint)
    {
        base.Create(_WayPoint);
    }

    public override void DeadEnemy()
    {
        base.DeadEnemy();
    }

    public override void Hit(int _Damage)
    {
        base.Hit(_Damage);
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override IEnumerator StateEnemy()
    {
        return base.StateEnemy();
    }
}
