using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 1;
    public float Duration = 0.1f;

    public void Init(BaseTower owner)
    {
        transform.position = owner.transform.position;
        gameObject.SetActive(true);
        StartCoroutine(Fire(owner));
    }

    IEnumerator Fire(BaseTower owner)
    {
        float fDistance = Mathf.Infinity;
        Vector3 vecDirection = Vector3.zero;
        while (fDistance > 0.01f)
        {
            if ((owner.target == null) || (!owner.target.gameObject.activeSelf))
            {
                gameObject.SetActive(false);
                yield break;
            }

            transform.LookAt(owner.target.transform);
            fDistance = (owner.target.transform.position - transform.position).sqrMagnitude;
            vecDirection = (owner.target.transform.position - transform.position).normalized;
            transform.position += vecDirection * 10 * Time.deltaTime;

            yield return null;
        }
        owner.target.Hit((int)owner.Status.Point);
        gameObject.SetActive(false);
    }

    public virtual void OnDisable()
    {
        ObjectPoolerManager.Instance.ReturnToPool(gameObject);    // 한 객체에 한번만 
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }
}