using UnityEngine;
using UnityEngine.EventSystems;

public class MergeEvent : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    private HeroManager myHeroManager;
    private bool isDrag;
    private RaycastHit mHit;

    private BaseHero seletTower;

    private void Awake()
    {
        myHeroManager = HeroManager.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out mHit))
        {
            seletTower = mHit.transform.GetComponent<BaseHero>();
            if (seletTower == null)
                return;

            myHeroManager.SeleteTower(seletTower.strObjName);
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
            BaseHero objectHit = mHit.transform.GetComponent<BaseHero>();

            if (seletTower.strObjName.Equals(objectHit.strObjName) && seletTower!=objectHit)
            {
                myHeroManager.MergeTower(seletTower);
            }
        }
    }
}
