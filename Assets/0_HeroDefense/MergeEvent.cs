using UnityEngine;
using UnityEngine.EventSystems;

public class MergeEvent : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    private HeroManager myHeroManager;
    private bool isDrag;
    private RaycastHit mHit;

    private BaseTower seletTower;

    private void Awake()
    {
        myHeroManager = HeroManager.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out mHit))
        {
            seletTower = mHit.transform.GetComponent<BaseTower>();
            if (seletTower == null)
                return;

            myHeroManager.SeleteTower(seletTower.mTowerName);
            isDrag = true;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 캐릭터 포지션 이동.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag)
            return;

        isDrag = false;
        myHeroManager.ResetSelect();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out mHit))
        {
            BaseTower objectHit = mHit.transform.GetComponent<BaseTower>();

            if (seletTower.mTowerName.Equals(objectHit.mTowerName) && seletTower!=objectHit)
            {
                myHeroManager.MergeTower(seletTower);
            }
        }
    }
}
