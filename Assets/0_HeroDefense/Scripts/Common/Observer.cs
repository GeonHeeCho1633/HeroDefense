using System;
using System.Collections.Generic;
using UnityEngine;


namespace ADA_Observer
{
	/// <summary>
	/// ����Ǿ������� �׼� ������ ������.
	/// </summary>
	public class ObserverAction
	{
		private Action<object> m_action;


		/// <summary>
		/// ������ ����Ǿ������� �׼� �����Ѵ�.
		/// </summary>
		/// <param name="_action"></param>
		public ObserverAction(Action<object> _action)
		{
			m_action = _action;
		}

		/// <summary>
		/// ���� �׼� �����Ѵ�.
		/// </summary>
		/// <param name="_value"></param>
		public void OnAction(object _value)
		{
			m_action?.Invoke(_value);
		}
	}

	/// <summary>
	/// ���� üũ�� ������Ʈ�� ������.
	/// </summary>
	public class ObserverFunc
	{
		private ObserverAction m_select;
		private Func<object, object> m_func;
		private bool m_isFirst = true;
		private object m_prevKey = default(object);

		/// <summary>
		/// ������ Observable�� ���� üũ�� ������Ʈ �����Ѵ�.
		/// </summary>
		/// <param name="_select">Observable</param>
		/// <param name="_func">���� üũ�� ������Ʈ</param>
		public ObserverFunc(ObserverAction _select, Func<object, object> _func)
		{
			m_select = _select;
			m_func = _func;
			m_prevKey = _func(m_prevKey);
		}

		/// <summary>
		/// IObserver OnNext : �������� ���� ObserverInfo�� ��������� Ȯ���Ѵ�.
		/// </summary>
		/// <param name="value"></param>
		public void OnFunc(object value)
		{
			if (IsChange(m_func(m_prevKey)) == true)
			{
				m_select.OnAction(m_prevKey);
			}
		}

		/// <summary>
		/// ������Ʈ�� ���� �Ǿ����� Ȯ���Ѵ�.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool IsChange(object value)
		{
			object currentKey;
			try
			{
				currentKey = value;
			}
			catch (Exception exception)
			{
				Debug.LogError($"Observer IsChange : {exception.Message}");
				return false;
			}

			bool sameKey = false;
			if (m_isFirst)
			{
				m_isFirst = false;
			}
			else
			{
				try
				{
					sameKey = Equals(currentKey, m_prevKey);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Observer IsChange : {ex.Message}");
					return false;
				}
			}

			if (!sameKey)
			{
				m_prevKey = currentKey;
				return true;
			}
			else
				return false;
		}
	}


	/// <summary>
	/// ������ ������ �����ϰ� �����Ѵ�.
	/// </summary>
	public class ObserverInfo
	{
		/// <summary>
		/// ����Ǿ������� �׼� ������ ������.
		/// </summary>
		private ObserverAction m_observable;
		/// <summary>
		/// ���� üũ�� ������Ʈ�� ������.
		/// </summary>
		private ObserverFunc m_observer;

		/// <summary>
		/// ������ ������ ������ Observable, Observer �� �����Ѵ�.
		/// </summary>
		/// <param name="_func">���� üũ�� ������Ʈ</param>
		/// <param name="_action">����Ǿ������� �׼�</param>
		public ObserverInfo(Func<object, object> _func, Action<object> _action)
		{
			m_observable = new ObserverAction(_action);
			m_observer = new ObserverFunc(m_observable, _func);
		}

		/// <summary>
		/// �������� ���� ObserverInfo�� ��������� Ȯ���Ѵ�.
		/// </summary>
		public void OnUpdate()
		{
			if (m_observable == null || m_observer == null) return;
			m_observer.OnFunc(default(object));
		}
	}


	/// <summary>
	/// ������ ���� ������Ʈ
	/// </summary>
	public class ObserverChange : MonoBehaviour
	{
		/// <summary>
		/// ObserverInfo ����Ʈ
		/// </summary>
		private List<ObserverInfo> m_listObserver = new List<ObserverInfo>();


		/// <summary>
		/// ObserverInfo ����Ʈ�� �߰��Ѵ�.
		/// </summary>
		/// <param name="_func">���� üũ�� ������Ʈ</param>
		/// <param name="_action">����Ǿ������� �׼�</param>
		public void AddObserver(Func<object, object> _func, Action<object> _action)
		{
			m_listObserver.Add(new ObserverInfo(_func, _action));
		}

		/// <summary>
		/// LateUpdate �������� ���� ObserverInfo�� ��������� Ȯ���Ѵ�.
		/// </summary>
		private void LateUpdate()
		{
			if (m_listObserver == null || m_listObserver.Count == 0) return;

			foreach (ObserverInfo info in m_listObserver)
			{
				if (info != null)
					info.OnUpdate();
			}
		}
	}

	public static class ObserverExtensions
	{
		/// <summary>
		/// ������Ʈ Ȯ�� �Լ� : ������Ʈ�� ���� ����� �����Ѵ�.
		/// </summary>
		/// <param name="_component"></param>
		/// <param name="_select">���� �� ������Ʈ</param>
		/// <param name="_result">������Ʈ�� ���� ��ٸ� ��� ó��</param>
		public static void UpdateObserverChange(this Component _component, Func<object, object> _select, Action<object> _result)
		{
			if (_component == null || _component.gameObject == null) return;
			ObserverChange observer = CommonUtil.GetComponent<ObserverChange>(_component.gameObject);
			if (observer == null) return;
			observer.AddObserver(_select, _result);
		}

		//
		// CommonUtil.GetComponent �ڵ� ���� ����� �ּ�
		//
		//public static T GetComponent<T>(GameObject _object) where T : Component
		//{
		//	T component = _object.GetComponent<T>();
		//	if (component != null)
		//		return component;
		//
		//	return _object.AddComponent<T>();
		//}
	}
}
