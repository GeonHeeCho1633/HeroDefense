using System;
using System.Collections.Generic;
using UnityEngine;


namespace ADA_Observer
{
	/// <summary>
	/// 변경되었을때의 액션 정보를 가진다.
	/// </summary>
	public class ObserverAction
	{
		private Action<object> m_action;


		/// <summary>
		/// 생성시 변경되었을때의 액션 설정한다.
		/// </summary>
		/// <param name="_action"></param>
		public ObserverAction(Action<object> _action)
		{
			m_action = _action;
		}

		/// <summary>
		/// 변경 액션 설정한다.
		/// </summary>
		/// <param name="_value"></param>
		public void OnAction(object _value)
		{
			m_action?.Invoke(_value);
		}
	}

	/// <summary>
	/// 변경 체크할 오브젝트를 가진다.
	/// </summary>
	public class ObserverFunc
	{
		private ObserverAction m_select;
		private Func<object, object> m_func;
		private bool m_isFirst = true;
		private object m_prevKey = default(object);

		/// <summary>
		/// 생성시 Observable과 변경 체크할 오브젝트 설정한다.
		/// </summary>
		/// <param name="_select">Observable</param>
		/// <param name="_func">변경 체크할 오브젝트</param>
		public ObserverFunc(ObserverAction _select, Func<object, object> _func)
		{
			m_select = _select;
			m_func = _func;
			m_prevKey = _func(m_prevKey);
		}

		/// <summary>
		/// IObserver OnNext : 매프레임 마다 ObserverInfo에 변경사항은 확인한다.
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
		/// 오브젝트가 변경 되었는지 확인한다.
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
	/// 옵저버 정보를 보관하고 관리한다.
	/// </summary>
	public class ObserverInfo
	{
		/// <summary>
		/// 변경되었을때의 액션 정보를 가진다.
		/// </summary>
		private ObserverAction m_observable;
		/// <summary>
		/// 변경 체크할 오브젝트를 가진다.
		/// </summary>
		private ObserverFunc m_observer;

		/// <summary>
		/// 생성시 정보를 가져와 Observable, Observer 를 설정한다.
		/// </summary>
		/// <param name="_func">변경 체크할 오브젝트</param>
		/// <param name="_action">변경되었을때의 액션</param>
		public ObserverInfo(Func<object, object> _func, Action<object> _action)
		{
			m_observable = new ObserverAction(_action);
			m_observer = new ObserverFunc(m_observable, _func);
		}

		/// <summary>
		/// 매프레임 마다 ObserverInfo에 변경사항은 확인한다.
		/// </summary>
		public void OnUpdate()
		{
			if (m_observable == null || m_observer == null) return;
			m_observer.OnFunc(default(object));
		}
	}


	/// <summary>
	/// 옵저버 관리 컴포넌트
	/// </summary>
	public class ObserverChange : MonoBehaviour
	{
		/// <summary>
		/// ObserverInfo 리스트
		/// </summary>
		private List<ObserverInfo> m_listObserver = new List<ObserverInfo>();


		/// <summary>
		/// ObserverInfo 리스트에 추가한다.
		/// </summary>
		/// <param name="_func">변경 체크할 오브젝트</param>
		/// <param name="_action">변경되었을때의 액션</param>
		public void AddObserver(Func<object, object> _func, Action<object> _action)
		{
			m_listObserver.Add(new ObserverInfo(_func, _action));
		}

		/// <summary>
		/// LateUpdate 매프레임 마다 ObserverInfo에 변경사항은 확인한다.
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
		/// 컴포넌트 확장 함수 : 오브젝트가 변경 됬는지 감지한다.
		/// </summary>
		/// <param name="_component"></param>
		/// <param name="_select">감지 할 오브젝트</param>
		/// <param name="_result">오브젝트가 변경 됬다면 결과 처리</param>
		public static void UpdateObserverChange(this Component _component, Func<object, object> _select, Action<object> _result)
		{
			if (_component == null || _component.gameObject == null) return;
			ObserverChange observer = CommonUtil.GetComponent<ObserverChange>(_component.gameObject);
			if (observer == null) return;
			observer.AddObserver(_select, _result);
		}

		//
		// CommonUtil.GetComponent 코드 리뷰 참고용 주석
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
