using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseTower : MonoBehaviour
{
	[SerializeField]
	protected Bullet objBullet;

	[SerializeField]
	protected BaseEnemy objTarget;
	[SerializeField]
	protected BaseObject objCamp;
	public BaseEnemy target => objTarget;

	[SerializeField]
	public string mTowerName;
	[SerializeField]
	protected Stat mStat;
	public Stat Status => mStat;
	[SerializeField]
	protected int indexTowerCode;
	[HideInInspector]
	public TowerTile mTile;
	[SerializeField]
	protected BaseEnemy[] mArrTargetMonster;
	[SerializeField]
	protected bool misActive;
	[SerializeField]
	protected float mDeltaTime = 0;
	protected Tier mTier;
	public Tier IsTier
	{
		set { mTier = value; }
		get { return mTier; }
	}

	protected SkinnedMeshRenderer[] arrMaterials;
	protected List<Material> mListMaterial;
	protected Color[] arrBaseColor;
	protected BoxCollider mHitBox;

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
    private void Awake()
    {
		Initialized();
	}
    public virtual void Initialized()
	{
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
	}
	public void SetTile(TowerTile _Tile)
	{
		mTile = _Tile;
		mTile.IsActive = true;

		mStat.AttackPoint = 20;
		mStat.AttackSpeed = 0.25f;

		mDeltaTime = Mathf.Infinity;

		transform.position = mTile.Pos;
		transform.position += Vector3.up * (transform.localScale.y * 0.5f);

		objCamp = EnemyManager.Instance.GetBaseCamp();

		StartCoroutine(StartTower());
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
		//objTarget = EnemyManager.Instance.GetTarget();
	}
	public virtual void AttackTargets()
	{
		//if (target!=null && !target.IsActive)
		//{
		//	objTarget = null;
		//	mDeltaTime = 0;
		//}

		//if (mDeltaTime >= mStat.AttackSpeed)
		//{
		//	BaseEnemy temp = objTarget != null ? objTarget : objCamp;
		//	//ObjectPoolerManager.Instance.SpawnFromPool<Bullet>("Bullet", transform.position).Init(this, temp);
		//	mDeltaTime = 0;
		//}
	}
	public virtual IEnumerator StartTower()
	{
		while (true)
		{
			mDeltaTime += Time.deltaTime;
			if (target == null) SearchTargets();

			AttackTargets();

			yield return null;
		}
	}
	public virtual void Merge()
	{
		gameObject.SetActive(false);
	}
	public virtual void OnDisable()
	{
        ObjectPoolerManager.Instance.ReturnToPool(gameObject);
		StopAllCoroutines();
		CancelInvoke();    
	}
}
