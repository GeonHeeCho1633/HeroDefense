using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
	public int index;
	public string strObjName;
	[SerializeField]
	protected BaseObject objCamp;
	protected ObjectType myType;
	[SerializeField]
	protected Deck myDeck;
	protected IEnumerator crtUpdate;
	protected IEnumerator crtActive;
	[SerializeField]
	protected bool misActive;

	protected SkinnedMeshRenderer[] arrMaterials;
	protected List<Material> mListMaterial;
	protected Color[] arrBaseColor;
	protected BoxCollider mHitBox;

	public bool IsActive
	{
		set { misActive = value; }
		get { return misActive; }
	}

	public virtual void InitObject()
	{
		crtUpdate = R_UpdateObject();
	}

	public virtual void SetObject(Deck _deck)
	{
		if (crtUpdate == null)
			InitObject();

		gameObject.SetActive(true);
		myDeck.Set(_deck);
		IsActive = true;
		StartCoroutine(crtUpdate);
	}

	public virtual IEnumerator R_UpdateObject()
	{
		yield return new WaitForEndOfFrame();
	}

	public virtual void ReturnObject()
	{
		misActive = false;
		CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
		StopCoroutine(crtUpdate);
		if (gameObject.name.Contains("Camp"))
			return;
		ObjectPoolerManager.Instance.ReturnToPool(gameObject);    // 한 객체에 한번만 
		gameObject.SetActive(false);
	}

	public void Hit(float _damage = 0)
	{
		myDeck.statChar.HP -= _damage;
		if (GetObjStat().HP <= 0)
		{
			ReturnObject();
		}
	}

	public Stat GetObjStat() { return myDeck.statChar; }
}
