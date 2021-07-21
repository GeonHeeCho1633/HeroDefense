using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 번들 다운로드 전에 실행되어야하는 메시지 박스에서 출력되어야하는 스트링 테이블 로드 클래스
/// 테이블이 언어별 컬럼에서 언어별 테이블로 분리되므로 추후 수정 필요
/// </summary>
public class SystemMessageTable
{
	public static SystemMessageTable Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new SystemMessageTable();
			}
			return instance;
		}
	}

	private static SystemMessageTable instance;

	private Dictionary<string, string> m_dataInfos = new Dictionary<string, string>();

	public SystemMessageTable()
	{
		InitDatas();
	}


	#region Data Management
	private void InitDatas()
	{
		ResetLocalization();
	}

	/// <summary>
	/// 빌드시 포함되는 Resources 폴더에서 해당 테이블 로드
	/// 테이블이 언어별 컬럼에서 언어별 테이블로 분리되므로 추후 수정 필요
	/// </summary>
	public void ResetLocalization()
	{
		m_dataInfos.Clear();
		TextAsset dataString = Resources.Load<TextAsset>("Table/SubStringTableXlsTblAsset");
		SubStringTableXlsTblAsset table = SubStringTableXlsTblAsset_Parser.Parsing(dataString.text);

		string country = CommonUtil.GetLanguage();
		Dictionary<int, SubStringTableXlsTblAsset.Param>.Enumerator enumerator = table.param.GetEnumerator();//
		while (enumerator.MoveNext())
		{
			int key = enumerator.Current.Key;
			SubStringTableXlsTblAsset.Param data = enumerator.Current.Value;
			if (data == null) continue;

			if (m_dataInfos.ContainsKey(data.string_id) == false)
			{
				var text = UIUtil.FindFieldNameToValueString(data, country);
				if (string.IsNullOrEmpty(text) == true)
					text = data.us;

				m_dataInfos.Add(data.string_id, text);
			}
		}
		enumerator.Dispose();
	}
	
	public string GetDataByKey(string _strKey)
	{
		if (m_dataInfos.ContainsKey(_strKey) == false)
			return _strKey;
		return m_dataInfos[_strKey];
	}

	public void SetUILabelLocalization(Text _uiLabel, string _strKey)
	{
		_uiLabel.text = GetDataByKey(_strKey);
	}

	public void SetUILabelLocalization(Text _uiLabel, string _strKey, params object[] _args)
	{
		var data = m_dataInfos.ContainsKey(_strKey) ? m_dataInfos[_strKey] : "";
		_uiLabel.text = string.IsNullOrEmpty(data) ? _strKey : string.Format(data, _args);
	}
	#endregion
}
