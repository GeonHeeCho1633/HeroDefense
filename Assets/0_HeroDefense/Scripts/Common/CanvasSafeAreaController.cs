using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomUI;


/// <summary>
/// 노치 영역 설정 클래스
/// </summary>

public class CanvasSafeAreaController : MonoBehaviour
{
    /// <summary>
    /// 현재 노치영역은 검은색 반투명 이미지나 그라데이션 사용함
    /// </summary>
    [System.Serializable]
    public class SafeAreaInfo
    {
        [Header("<<< EDITOR ALLOC >>>")]
        public bool IsShow = true;                              //노치영역 활성화 옵션
        public RawImage uiTexTextureArea;                       //노치영역 텍스쳐 설정(현재 텍스쳐 설정하는것이 없음 UI 시안 확인후 불필요하면 제거)
        public Color DefaultColor = new Color(0, 0, 0, .75f);   //노치영역 컬러 설정

        public void SetActive()
        {
            uiTexTextureArea.gameObject.SetActive(IsShow);
            uiTexTextureArea.color = DefaultColor;
        }
    }

    [SerializeField] public SafeAreaInfo Top = new SafeAreaInfo();
    [SerializeField] public SafeAreaInfo Bottom = new SafeAreaInfo();
    [SerializeField] public SafeAreaInfo Left = new SafeAreaInfo();
    [SerializeField] public SafeAreaInfo Right = new SafeAreaInfo();

    public float fPaddingLeft { get; private set; }
    public float fPaddingRight { get; private set; }
    public float fPaddingTop { get; private set; }
    public float fPaddingBottom { get; private set; }

    DrivenRectTransformTracker m_Tracker;
    bool isFirst = false;


    /// <summary>
    /// 노치영역 크기 설정함수 노치영역은 페이지 마다 고정이므로 어웨이크에서만 실행
    /// </summary>
	[ContextMenu("SafeArea")]
    private void Awake()
    {
        if (isFirst == true) return;
        isFirst = true;
        m_Tracker.Clear();
        var myTransform = GetComponent<RectTransform>();
        if (myTransform == null) return;

        Rect safeArea = CommonUtil.GetSafeArea();
        Vector2 screen = new Vector2(Screen.width, Screen.height);

        float minX = safeArea.x / screen.x;
        float minY = safeArea.y / screen.y;
        float maxX = (safeArea.x + safeArea.width) / screen.x;
        float maxY = (safeArea.y + safeArea.height) / screen.y;

        minX = Left.IsShow == false ? 0f : minX;
        maxX = Right.IsShow == false ? 1f : maxX;
        minY = Bottom.IsShow == false ? 0f : minY;
        maxY = Top.IsShow == false ? 1f : maxY;

        fPaddingLeft = minX * Screen.width;
        fPaddingRight = (1 - maxX) * Screen.width;
        fPaddingTop = (1 - maxY) * Screen.height;
        fPaddingBottom = minY * Screen.height;

        myTransform.anchorMin = new Vector2(minX, minY);
        myTransform.anchorMax = new Vector2(maxX, maxY);
        m_Tracker.Add(this, myTransform, DrivenTransformProperties.All);

        myTransform.localScale = Vector3.one;

        // Top
        AddImageBlock(ref Top.uiTexTextureArea, 0f, maxY, 1f, 1f, "CanvasSafeArea_Top");
        Top.SetActive();
        // Bottom

        AddImageBlock(ref Bottom.uiTexTextureArea, 0f, 0f, 1f, minY, "CanvasSafeArea_Bottom");
        Bottom.SetActive();

        // Left
        AddImageBlock(ref Left.uiTexTextureArea, 0f, 0f, minX, 1f, "CanvasSafeArea_Left");
        Left.SetActive();
        // Right
        AddImageBlock(ref Right.uiTexTextureArea, maxX, 0f, 1f, 1f, "CanvasSafeArea_Right");
        Right.SetActive();
    }

    /// <summary>
    /// Top/Bottom/Left/Right 오브젝트 텍스쳐 설정 함수 (현재 노치영역에 텍스쳐를 설정하지 않으므로 UI 시안 나오면 수정 필요할 수 있음)
    /// </summary>
    private RawImage AddImageBlock(ref RawImage uitexture, float _fminX, float _fminY, float _fmaxX, float _fmaxY, string _fobjname)
    {
        if (uitexture == null)
        {
            GameObject createObject = new GameObject(_fobjname);
            if (createObject == null) return null;
            createObject.transform.SetParent(gameObject.transform.parent, false);
            createObject.transform.localPosition = Vector3.zero;
            createObject.transform.localRotation = Quaternion.identity;
            createObject.transform.localScale = Vector3.one;
            CommonUtil.ChangeLayers(createObject, gameObject.layer);

            uitexture = createObject.AddComponent<RawImage>();
        }

        uitexture.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        uitexture.transform.localScale = Vector3.one;

        uitexture.rectTransform.anchorMin = new Vector2(_fminX, _fminY);
        uitexture.rectTransform.anchorMax = new Vector2(_fmaxX, _fmaxY);
        uitexture.rectTransform.anchoredPosition3D = Vector3.zero;
        uitexture.rectTransform.offsetMin = Vector3.zero;
        uitexture.rectTransform.offsetMax = Vector3.zero;

        m_Tracker.Add(this, uitexture.rectTransform, DrivenTransformProperties.All);

        return uitexture;
    }
}