using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroManager : MonoSingleton<HeroManager>
{
    [SerializeField]
    private TileManager managerTile;
    [SerializeField]
    private GameObject[] objTempTower;
    private List<BaseTower> mActiveList = new List<BaseTower>(0);

    public void BuildTower()
    {
        if (!(managerTile.EmptyTileCount > 0))
            return;

        BaseTower temp;
        string _towerType = (UnityEngine.Random.Range(0,1.0f) > 0.5f)? "Kang" : "Nanky";
        temp = ObjectPoolerManager.Instance.SpawnFromPool<BaseTower>(_towerType, transform.position, transform);
        temp.SetTile(managerTile.GetEmptyTile());
        temp.mTowerName = _towerType;
        mActiveList.Add(temp);
    }

    public void SeleteTower(string _name)
    {
        for (int i = 0; i < mActiveList.Count; i++)
        {
            if (mActiveList[i].mTowerName != _name)
                mActiveList[i].ChangColor();
        }
    }

    public void ResetSelect()
    {
        for (int i = 0; i < mActiveList.Count; i++)
        {
            mActiveList[i].ResetColor();
        }
    }

    public void MergeTower(BaseTower _tower)
    {
        managerTile.ReturnTile(_tower.mTile);
        _tower.Merge();
    }
}