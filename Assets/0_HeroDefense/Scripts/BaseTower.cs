using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stat
{
	public float Point;
	public float Speed;
}

public class BaseTower : MonoBehaviour
{
	[SerializeField]
	protected Bullet objBullet;

	[SerializeField]
	protected BaseEnemy objTarget;
	public BaseEnemy target => objTarget;

	[SerializeField]
	public string mTowerName;
	[SerializeField]
	protected Stat mStat;
	public Stat Status => mStat;
	[SerializeField]
	protected int indexTowerCode;
	[SerializeField]
	protected TowerTile mTile;
	[SerializeField]
	protected BaseEnemy[] mArrTargetMonster;
	[SerializeField]
	protected bool misActive;
	[SerializeField]
	protected float mDeltaTime = 0;
	public MeshRenderer mRenderer;
	Color _Color;
	public bool IsActive
	{
		set
		{
			misActive = value;

			if (!misActive)
			{
			}
		}
		get { return misActive; }
	}

	public void SetTile(TowerTile _Tile)
	{

		mTile = _Tile;
		mTile.IsActive = true;

		mStat.Point = 5;
		mStat.Speed = 0.5f;
		mRenderer = GetComponent<MeshRenderer>();
		mTowerName = (UnityEngine.Random.Range(0,1.0f) > 0.5f)? "Test1":"Test2";
		if(mTowerName == "Test1")
		{
			mRenderer.materials[0].color = Color.blue;
		}

		_Color = mRenderer.materials[0].color;

		mDeltaTime = Mathf.Infinity;

		transform.position = mTile.Pos;
		transform.position += Vector3.up * (transform.localScale.y * 0.5f);
		
		StartCoroutine(StartTower());
	}
	public void ResetColor()
    {
		mRenderer.materials[0].color = Color.blue;
	}
	public virtual void SearchTargets()
	{
		objTarget = EnemyManager.Instance.GetTarget();
	}
	public virtual void AttackTargets()
	{
		if (!target.IsActive)
		{
			objTarget = null;
			return;
		}
		else if (mDeltaTime >= mStat.Speed)
		{
			ObjectPoolerManager.Instance.SpawnFromPool<Bullet>("Bullet", transform.position).Init(this);
			mDeltaTime = 0;
		}
	}

	public virtual IEnumerator StartTower()
	{
		while (true)
		{
			mDeltaTime += Time.deltaTime;
			if (target == null) SearchTargets();
			else AttackTargets();
			yield return null;
		}
	}

	public virtual void OnDisable()
	{
        ObjectPoolerManager.Instance.ReturnToPool(gameObject);    // ?? ?????? ?????? 
		StopAllCoroutines();
		CancelInvoke();    // Monobehaviour?? Invoke?? ?????? 
	}
}
