using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : BaseObject
{
	protected Vector3[] mWayPoint;
	protected Vector3 mDirection;
	protected int iWayCount;
	protected float fDistance;
	protected float fDeltaTime;
	protected HeroManager heroManager;
	[SerializeField]
	protected bool isAttack;
	[SerializeField]
	protected BaseObject heroCamp;

	public override void InitObject()
	{
		heroManager = HeroManager.Instance;
		heroCamp = heroManager.GetBaseCamp();
		base.InitObject();
	}
	public override void SetObject(Deck _deck)
	{
		isAttack = false;
		base.SetObject(_deck);
	}
	public override IEnumerator R_UpdateObject()
	{
		while (IsActive)
		{
			if (isAttack)
				AttackHeroCamp();
			else
				SearchWayPoint();
			yield return null;
		}
	}
	public void SetWayPoint(Transform[] _WayPoint)
	{
		mWayPoint = new Vector3[_WayPoint.Length];
		mDirection = Vector3.zero;
		fDistance = Mathf.Infinity;
		for (int i = 0; i < _WayPoint.Length; i++)
		{
			mWayPoint[i] = _WayPoint[i].position;
		}
	}
	private void SearchWayPoint()
	{
		if (mDirection.Equals(Vector3.zero))
		{
			iWayCount = 0;
			transform.position = mWayPoint[iWayCount];
			iWayCount++;
			transform.LookAt(mWayPoint[iWayCount]);
		}
		else if (fDistance < 0.01f)
		{
			if (iWayCount < mWayPoint.Length - 1)
			{
				iWayCount++;
				fDistance = Mathf.Infinity;
				transform.LookAt(mWayPoint[iWayCount]);
			}
			else if (iWayCount == mWayPoint.Length - 1)
			{
				// Todo : 끝까지 도달함
				isAttack = true;
				return;
			}
		}
		fDistance = (mWayPoint[iWayCount] - transform.position).sqrMagnitude;
		mDirection = (mWayPoint[iWayCount] - transform.position).normalized;
		transform.position += mDirection * GetObjStat().MoveSpeed * Time.deltaTime;
	}
	protected void AttackHeroCamp()
	{
		if (!heroCamp.IsActive)
		{
			return;
			//fDeltaTime = 0;
		}
		else
		{
			fDeltaTime += Time.deltaTime;
		}

		if (fDeltaTime >= GetObjStat().AttackSpeed)
		{
			ObjectPoolerManager.Instance.SpawnFromPool<Bullet>("Bullet", transform.position).Init(this, heroCamp);
			fDeltaTime = 0;
		}
	}

	public override void ReturnObject()
	{
		EnemyManager.Instance.RemoveEnemy(this);
		base.ReturnObject();
	}
}
