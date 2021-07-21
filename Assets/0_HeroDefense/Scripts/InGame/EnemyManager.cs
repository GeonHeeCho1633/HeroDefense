using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
	[SerializeField]
	private Transform[] mWayPoint;
	public Transform[] WayPoint => mWayPoint;

	[SerializeField]
	private Transform EnemyList;
	[SerializeField]
	private BaseEnemy objBaseEnemy;
	private bool isStart;
    private List<BaseEnemy> mActiveList = new List<BaseEnemy>(0);
	private bool isGameEnd;
	public bool IsGameEnd
	{
		get { return isGameEnd; }
		set { isGameEnd = value; }
	}

	IEnumerator Start()
	{
		while (true)
		{
			AutoCreateEnemy();
			yield return new WaitForSeconds(0.75f);
		}
	}

	private void AutoCreateEnemy()
	{
		if (!isStart) return;

		BaseEnemy _Enemy = ObjectPoolerManager.Instance.SpawnFromPool<BaseEnemy>("Enemy", WayPoint[0].position, EnemyList);
		_Enemy.Create(mWayPoint);
		mActiveList.Add(_Enemy);
	}

    public void RemoveEnemy(BaseEnemy _Enemy)
    {
		if (mActiveList.Contains(_Enemy))
		{
			mActiveList.Remove(_Enemy);
		}
    }

	public BaseEnemy GetTarget()
	{
		if (mActiveList.Count > 0)
			return mActiveList[0];

		return null;
	}

    public void CreateEnemy(int i)
	{
		BaseEnemy _Enemy = ObjectPoolerManager.Instance.SpawnFromPool<BaseEnemy>("Enemy1", WayPoint[0].position, EnemyList);
		_Enemy.Create(mWayPoint);
		mActiveList.Add(_Enemy);
	}

	public void OnClickStart() { isStart = true; }

}