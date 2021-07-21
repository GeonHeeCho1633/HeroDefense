using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTile : MonoBehaviour
{
	private bool misActive;
	public bool IsActive
	{
		set { misActive = value; }
		get { return misActive; }
	}

	public Vector3 Pos => transform.position;
}
