using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	[SerializeField]
	private List<TowerTile> mEmptyTile;
	private List<TowerTile> mActiveTile = new List<TowerTile>(0);  // 초기화 함수 구성할 것.
	public int EmptyTileCount => mEmptyTile.Count;

	public TowerTile GetEmptyTile()
	{
		TowerTile result = null;
		if (EmptyTileCount > 0)
		{
			result = mEmptyTile[Random.Range(0, EmptyTileCount)];
			mEmptyTile.Remove(result);
			mActiveTile.Add(result);
		}
		return result;
	}

	public void ReturnTile(TowerTile _tile)
	{
		if (mActiveTile.Contains(_tile))
		{
			mActiveTile.Remove(_tile);
			mEmptyTile.Add(_tile);
		}
	}
}
