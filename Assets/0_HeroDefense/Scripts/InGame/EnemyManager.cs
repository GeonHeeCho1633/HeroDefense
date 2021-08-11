using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
	[SerializeField]
	private BaseObject objCamp;
	[SerializeField]
	private Deck[] playDeck;

	[SerializeField]
	private Transform[] mWayPoint;
	public Transform[] WayPoint => mWayPoint;

	[SerializeField]
	private Transform EnemyList;
	private bool isStart;

	[SerializeField]
    private List<BaseMonster> mActiveList = new List<BaseMonster>(0);
	private bool isGameEnd;

	public bool IsGameEnd
	{
		get { return isGameEnd; }
		set { isGameEnd = value; }
	}

	public void StartGame(Deck _Camp, List<Deck> _playDeck, int _Mode)
	{
		objCamp.InitObject();
		playDeck = _playDeck.ToArray();
		objCamp.SetObject(_Camp);
		isGameEnd = false;
		StartCoroutine(CheckCamp());
	}

	private IEnumerator CheckCamp()
	{
		while (!isGameEnd)
		{
			if (objCamp.GetObjStat().HP <= 0)
				isGameEnd = true;
			yield return null;
		}
		StopAllCoroutines();
	}

	public void CreateEnemy(int _number)
	{
		if (isGameEnd) return;

		BaseMonster _Enemy = ObjectPoolerManager.Instance.SpawnFromPool<BaseMonster>(playDeck[_number].key, WayPoint[0].position, EnemyList);
		_Enemy.SetWayPoint(mWayPoint);
		_Enemy.SetObject(playDeck[_number]);
		mActiveList.Add(_Enemy);
	}

    public void RemoveEnemy(BaseMonster _Enemy)
    {
		if (mActiveList.Contains(_Enemy))
		{
			mActiveList.Remove(_Enemy);
		}
    }

	public BaseObject GetTarget()
	{
		if (mActiveList.Count > 0)
			return mActiveList[0];

		return null;
	}

	public BaseObject GetBaseCamp()
	{
		return objCamp;
	}
}