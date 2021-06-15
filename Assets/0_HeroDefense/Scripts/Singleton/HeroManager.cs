using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoSingleton<HeroManager>
{
    [SerializeField]
    private TileManager managerTile;
    [SerializeField]
    private GameObject objTempTower;

    private List<BaseTower> mActiveList = new List<BaseTower>(0);

    public void BuildTower()
    {
        if (!(managerTile.EmptyTileCount > 0)) 
            return;

        //GameObject temp = GameObject.Instantiate(objTempTower);
        BaseTower temp = ObjectPoolerManager.Instance.SpawnFromPool<BaseTower>("Tower", transform.position, transform);
        temp.SetTile(managerTile.GetEmptyTile());
        mActiveList.Add(temp);
    }
}
