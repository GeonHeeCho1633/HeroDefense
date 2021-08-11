using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHero : BaseObject
{
	[SerializeField]
	protected BaseObject objTarget;
	public BaseObject target => objTarget;
	public TowerTile myTile;
	protected float fDeltaTime = 0;
	protected EnemyManager managerMonster;

	public override void InitObject()
	{
		managerMonster = EnemyManager.Instance;
		mHitBox = gameObject.GetComponent<BoxCollider>();
		mListMaterial = new List<Material>();
		arrMaterials = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < arrMaterials.Length; i++)
		{
			mListMaterial.AddRange(arrMaterials[i].materials);
		}
		arrBaseColor = new Color[mListMaterial.Count];
		for (int i = 0; i < mListMaterial.Count; i++)
		{
			arrBaseColor[i] = mListMaterial[i].color;
		}
		base.InitObject();
	}

	public override IEnumerator R_UpdateObject()
	{
		while (true)
		{
			fDeltaTime += Time.deltaTime;
			if (target == null)
				SearchTargets();

			AttackTargets();
			yield return null;
		}
	}

	public void SetTile(TowerTile _Tile)
	{
		myTile = _Tile;
		myTile.IsActive = true;
		fDeltaTime = Mathf.Infinity;
		objCamp = managerMonster.GetBaseCamp();
		transform.position = myTile.Pos;
		transform.position += Vector3.up * (transform.localScale.y * 0.5f);
	}
	public void ResetColor()
	{
		for (int i = 0; i < mListMaterial.Count; i++)
		{
			mListMaterial[i].color = arrBaseColor[i];
		}
		mHitBox.enabled = true;
	}
	public void ChangColor()
	{
		for (int i = 0; i < mListMaterial.Count; i++)
		{
			mListMaterial[i].color = Color.black;
		}
		mHitBox.enabled = false;
	}
	public virtual void SearchTargets()
	{
		objTarget = managerMonster.GetTarget();
	}
	public virtual void AttackTargets()
	{
		if (target != null && !target.IsActive)
		{
			objTarget = null;
		}

		if (fDeltaTime >= GetObjStat().AttackSpeed)
		{
			BaseObject temp = objTarget != null ? objTarget : objCamp;
			ObjectPoolerManager.Instance.SpawnFromPool<Bullet>("Bullet", transform.position).Init(this, temp);
			fDeltaTime = 0;
		}
	}

	public virtual void Merge()
	{
		ReturnObject();
	}
}
