using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 1;
    public float Duration = 0.1f;

    public void Init(BaseTower owner,BaseEnemy target)
    {
        transform.position = owner.transform.position;
        gameObject.SetActive(true);
        StartCoroutine(Fire(owner, target));
    }

    IEnumerator Fire(BaseTower owner, BaseEnemy target)
    {
        float fDistance = Mathf.Infinity;
        Vector3 vecDirection = Vector3.zero;
        while (fDistance > 0.01f)
        {
            if ((target == null) || (!target.gameObject.activeSelf))
            {
                gameObject.SetActive(false);
                yield break;
            }

            transform.LookAt(target.transform);
            fDistance = (target.transform.position - transform.position).sqrMagnitude;
            vecDirection = (target.transform.position - transform.position).normalized;
            transform.position += vecDirection * 15 * Time.deltaTime;

            yield return null;
        }
        target.Hit((int)owner.Status.AttackPoint);
        gameObject.SetActive(false);
    }

    public virtual void OnDisable()
    {
        ObjectPoolerManager.Instance.ReturnToPool(gameObject);    // 한 객체에 한번만 
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }
}