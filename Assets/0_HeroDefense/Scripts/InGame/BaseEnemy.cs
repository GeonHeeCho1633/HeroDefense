using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
	protected Tier mTier;
	protected Stat mStat;
	protected Vector3[] mWayPoint;
	protected Vector3 mDirection;
	protected int iWayCount;
	protected float fDistance;
	protected bool misActive;
	public bool IsActive
	{
		set {
			misActive = value;

			if (!misActive)
			{
				DeadEnemy();
			}
		}
		get { return misActive; }
	}
	public Tier IsTier
	{
		set { mTier = value; }
		get { return mTier; }
	}

	public virtual void Create(Transform[] _WayPoint)
	{
		mWayPoint = new Vector3[_WayPoint.Length];
		mDirection = Vector3.zero;
		fDistance = Mathf.Infinity;
		mStat.Speed = 5.0f;
		for (int i = 0; i < _WayPoint.Length; i++)
		{
			mWayPoint[i] = _WayPoint[i].position;
		}
		IsActive = true;

		StartCoroutine(StateEnemy());
	}

	public virtual void DeadEnemy()
	{
		misActive = false;
		gameObject.SetActive(false);
	}

	public void SearchWayPoint()
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
				// Todo : ������ ������
				DeadEnemy();
			}
		}
		fDistance = (mWayPoint[iWayCount] - transform.position).sqrMagnitude;
		mDirection = (mWayPoint[iWayCount]-transform.position).normalized;
		transform.position += mDirection * mStat.Speed * Time.deltaTime;
	}

	public virtual IEnumerator StateEnemy()
	{
		while (IsActive)
		{
			SearchWayPoint();
			yield return null;
		}
	}

    public virtual void OnDisable()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        ObjectPoolerManager.Instance.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� 
        CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� 
    }

	public virtual void Hit(int _Damage)
	{
		mStat.Point -= _Damage;

		if (mStat.Point <= 0)
		{
			DeadEnemy();
		}
	}
}
