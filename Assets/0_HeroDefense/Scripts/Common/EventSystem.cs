///
/// 참고
/// https://stackoverflow.com/questions/42034245/unity-eventmanager-with-delegate-instead-of-unityevent
///
using System.Collections.Generic;


namespace ADA_EventSystem
{
	public class EventSystem
	{
		public delegate void OnEvent();
		private Dictionary<string, OnEvent> dicEvent = new Dictionary<string, OnEvent>();



		public void AddEvent(string _eventName, OnEvent _onEvent)
		{
			if (dicEvent.ContainsKey(_eventName) == true)
				dicEvent[_eventName] += _onEvent;
			else
				dicEvent.Add(_eventName, _onEvent);
		}

		public void RemoveEvent(string _eventName, OnEvent _onEvent)
		{
			if (dicEvent.ContainsKey(_eventName) == true)
				dicEvent[_eventName] -= _onEvent;
		}

		public void ActionEvent(string _eventName)
		{
			if (dicEvent.ContainsKey(_eventName) == true)
				dicEvent[_eventName].Invoke();
		}
	}

	public class EventSystemGeneric<T>
	{
		public delegate void OnEvent(T _value);
		private Dictionary<string, OnEvent> dicEvent = new Dictionary<string, OnEvent>();



		public void AddEvent(string _eventName, OnEvent _onEvent)
		{
			if (dicEvent.ContainsKey(_eventName) == true)
				dicEvent[_eventName] += _onEvent;
			else
				dicEvent.Add(_eventName, _onEvent);
		}

		public void RemoveEvent(string _eventName, OnEvent _onEvent)
		{
			if (dicEvent.ContainsKey(_eventName) == true)
				dicEvent[_eventName] -= _onEvent;
		}

		public void ActionEvent(string _eventName, T _value)
		{
			if (dicEvent.ContainsKey(_eventName) == true)
				dicEvent[_eventName]?.Invoke(_value);
		}
	}
}
