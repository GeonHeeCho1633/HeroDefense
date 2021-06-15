using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectPoolerManager))]
public class ObjectPoolerEditor : Editor
{
	const string INFO = "풀링한 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
		"    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox(INFO, MessageType.Info);
		base.OnInspectorGUI();
	}
}
#endif

public class ObjectPoolerManager : MonoSingleton<ObjectPoolerManager>
{
	[Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	[SerializeField] Pool[] pools;
	List<GameObject> spawnObjects;
	Dictionary<string, Queue<GameObject>> poolDictionary;
	readonly string INFO = " 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
		"    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";


	public GameObject SpawnFromPool(string tag, Vector3 position, Transform parent) =>
		Instance._SpawnFromPool(tag, position, Quaternion.identity, parent);
	public GameObject SpawnFromPool(string tag, Vector3 position) =>
		Instance._SpawnFromPool(tag, position, Quaternion.identity);

	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
		Instance._SpawnFromPool(tag, position, rotation);

	public T SpawnFromPool<T>(string tag, Vector3 position, Transform parent = null) where T : Component
	{
		GameObject obj = Instance._SpawnFromPool(tag, position, Quaternion.identity, parent);
		if (obj.TryGetComponent(out T component))
			return component;
		else
		{
			obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}


	public T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
	{
		GameObject obj = Instance._SpawnFromPool(tag, position, rotation);
		if (obj.TryGetComponent(out T component))
			return component;
		else
		{
			obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}

	public List<GameObject> GetAllPools(string tag)
	{
		if (!Instance.poolDictionary.ContainsKey(tag))
			throw new Exception($"Pool with tag {tag} doesn't exist.");

		return Instance.spawnObjects.FindAll(x => x.name == tag);
	}

	public List<T> GetAllPools<T>(string tag) where T : Component
	{
		List<GameObject> objects = GetAllPools(tag);

		if (!objects[0].TryGetComponent(out T component))
			throw new Exception("Component not found");

		return objects.ConvertAll(x => x.GetComponent<T>());
	}

	public void ReturnToPool(GameObject obj)
	{
		if (!Instance.poolDictionary.ContainsKey(obj.name))
			throw new Exception($"Pool with tag {obj.name} doesn't exist.");

		//obj.transform.parent = transform;
		Instance.poolDictionary[obj.name].Enqueue(obj);
	}

	[ContextMenu("GetSpawnObjectsInfo")]
	void GetSpawnObjectsInfo()
	{
		foreach (var pool in pools)
		{
			int count = spawnObjects.FindAll(x => x.name == pool.tag).Count;
			Debug.Log($"{pool.tag} count : {count}");
		}
	}

	GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null)
	{
		if (!poolDictionary.ContainsKey(tag))
			throw new Exception($"Pool with tag {tag} doesn't exist.");

		// 큐에 없으면 새로 추가
		Queue<GameObject> poolQueue = poolDictionary[tag];
		if (poolQueue.Count <= 0)
		{
			Pool pool = Array.Find(pools, x => x.tag == tag);
			var obj = CreateNewObject(pool.tag, pool.prefab);
			ArrangePool(obj);
		}

		// 큐에서 꺼내서 사용
		GameObject objectToSpawn = poolQueue.Dequeue();
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;
		objectToSpawn.SetActive(true);
		if (parent != null) objectToSpawn.transform.SetParent(parent);

		return objectToSpawn;
	}

	void Start()
	{
		spawnObjects = new List<GameObject>();
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		// 미리 생성
		foreach (Pool pool in pools)
		{
			poolDictionary.Add(pool.tag, new Queue<GameObject>());
			for (int i = 0; i < pool.size; i++)
			{
				var obj = CreateNewObject(pool.tag, pool.prefab);
				ArrangePool(obj);
			}

			// OnDisable에 ReturnToPool 구현여부와 중복구현 검사
			if (poolDictionary[pool.tag].Count <= 0)
				Debug.LogError($"{pool.tag}{INFO}");
			else if (poolDictionary[pool.tag].Count != pool.size)
				Debug.LogError($"{pool.tag}에 ReturnToPool이 중복됩니다");
		}
	}

	GameObject CreateNewObject(string tag, GameObject prefab)
	{
		var obj = Instantiate(prefab, transform);
		obj.name = tag;
		obj.SetActive(false); // 비활성화시 ReturnToPool을 하므로 Enqueue가 됨
		return obj;
	}

	void ArrangePool(GameObject obj)
	{
		// 추가된 오브젝트 묶어서 정렬
		bool isFind = false;
		for (int i = 0; i < transform.childCount; i++)
		{
			if (i == transform.childCount - 1)
			{
				obj.transform.SetSiblingIndex(i);
				spawnObjects.Insert(i, obj);
				break;
			}
			else if (transform.GetChild(i).name == obj.name)
				isFind = true;
			else if (isFind)
			{
				obj.transform.SetSiblingIndex(i);
				spawnObjects.Insert(i, obj);
				break;
			}
		}
	}
}
