using UnityEngine;
/// <summary>
/// Ŭ���̾�Ʈ ���� ���� ����� ��ũ��Ʈ
/// </summary>
public static class LocalDataController
{
	public static readonly string strBGMPlay = "BGMPlay";
	public static readonly string strEffectPlay = "EffectPlay";
	public static readonly string strBGMVolume = "BGMVolume";
	public static readonly string strEffectVolume = "EffectVolum";

	/// <summary>
	/// ���õ����� ( �÷��̾� ������ ) ����.
	/// </summary>
	#region Default
	public static int GetLocalDataValue_Int(string _strkey, int _defaultValue = -1)
	{
		return PlayerPrefs.GetInt(_strkey, _defaultValue);
	}
	public static float GetLocalDataValue_Float(string _strkey, float _defaultValue = -1)
	{
		return PlayerPrefs.GetFloat(_strkey, _defaultValue);
	}
	public static string GetLocalDataValue_String(string _strkey, string _strdefaultValue = "")
	{
		return PlayerPrefs.GetString(_strkey, _strdefaultValue);
	}

	public static int SetLocalDataValue_Int(string _strkey, int _value)
	{
		PlayerPrefs.SetInt(_strkey, _value);
		return PlayerPrefs.GetInt(_strkey);
	}
	public static float SetLocalDataValue_Float(string _strkey, float value)
	{
		PlayerPrefs.SetFloat(_strkey, value);
		return PlayerPrefs.GetFloat(_strkey);
	}
	public static string SetLocalDataValue_String(string _strkey, string value)
	{
		PlayerPrefs.SetString(_strkey, value);
		return PlayerPrefs.GetString(_strkey);
	}
	#endregion
}
