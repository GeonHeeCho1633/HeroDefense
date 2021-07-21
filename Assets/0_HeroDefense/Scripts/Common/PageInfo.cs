using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ADA_Manager
{
    //페이지 고유 키값
    public enum PageEnum
    {
        None = 0,
        MainLobby,
        Styling,
        Studio,
        MainPage_Customizing,
        SimpleCustomizing,
        DetailCustomizing,
    }

    /// <summary>
    /// 페이지 실제 정보를 가지고 있는 클래스
    /// </summary>
    public class PageInfo : MonoBehaviour
    {
        public int currDepth = -1;
        public PageEnum CurrPage = PageEnum.None;
        [HideInInspector] public Canvas canvas;                                             //페이지 UI캔버스
        [HideInInspector] public Canvas canvasEffect;
        [HideInInspector] public CanvasScaler canvasScale;                                  //페이지 Effect캔버스
        [HideInInspector] public CanvasScaler canvasScaleEffect;

        [HideInInspector] public bool isTween = false;                                      //트윈 플레이 확인 함수
        private bool misTweenShow = false;

        //UI open및hide 연출 tween 애니메이션 null일경우 연출없음(전체화면UI일 경우에 세팅)
        [Header("<<< EDITOR ALLOC >>>")]
        public DG.Tweening.DOTweenAnimation tweenPage_Show;                                 //페이지 오픈시 연출을 위한 트윈에니메이션
        public DG.Tweening.DOTweenAnimation tweenPage_Hide;                                 //페이지 닫기시 연출을 위한 트윈에니메이션
        [Space()]

        public bool isPrevPageShow = false;                                                 //이전 페이지 프리펩 하이드 구분 변수
        public bool isGameCameraShow = false;                                               //페이지 게임카메라 하이드 구분 변수

        private bool isShow
        {
            get
            {
                return IsShow;
            }
            set
            {
                IsShow = value;
            }
        }
        private bool IsShow = false;                                                      //페이지 오픈 확인 변수
        private string Command = "";                                                      //페이지 오픈관련 세팅 값 설정 변수 Json으로 파싱 후 사용
        public bool isIgnoreReOpen = false;                                                 //init 이벤트 중복 실행 방지 변수
        private PageManager PageManager = null;                                           //pagemangaer 설정 변수


        //켄버스 관련 세팅 및 오브잭트 생성 이벤트 호출
        private void Awake()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    Awake~");
            canvas = PageUtil.GetCanvas(gameObject);
            canvasEffect = PageUtil.GetCanvas(gameObject, PageUtil.PageType.Effect);
            canvasScale = PageUtil.GetCanvasScaler(canvas);
            canvasScaleEffect = PageUtil.GetCanvasScaler(canvasEffect);
            PageManager = PageManager.Instance;
            Debug.Log(PageManager);
            ViewInitEvent();
        }

        /// <summary>
        /// 어웨이크에서 실행되는 함수로 컨버스 뎁스 세팅을 함
        /// </summary>
        public virtual void ViewInitEvent()
        {
            if (canvas != null)
                canvas.sortingOrder = currDepth * 2;
            if (canvasEffect != null)
                canvasEffect.sortingOrder = (currDepth * 2) + 1;

            isIgnoreReOpen = true;
        }

        #region PageOpen관련함수
        /// <summary>
        /// 페이지가 오픈 될때마다 실행되어야하는 리오픈 이벤트 함수 호출
        /// </summary>
        private void OnEnable()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == false)
                return;
#endif

            if (isIgnoreReOpen == false)
            {
                ViewReOpenEvent(Command);
            }
        }

        /// <summary>
        /// 페이지 프리팹 활성화 함수
        /// </summary>
        public void ViewOpen(string _strInOpenInfo, bool _isPrevOpen)
        {
            gameObject.SetActive(true);
            ViewOpenEvent(_strInOpenInfo, _isPrevOpen);
        }

        /// <summary>
        /// 페이지 프리팹 활성화 될때 실해되는함수
        /// </summary>
        protected bool ViewOpenEvent(string _strInOpenInfo, bool _isPrevOpen)
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    ViewOpenEvent~");
            ConvertOpenInfo(_strInOpenInfo);
            if (isShow == true && _isPrevOpen == false) return false;
            isShow = true;

            //TODO : JH재화 영역 UI업데이트 함수
            //if (TopMenuManager != null)
            //	TopMenuManager.SetUpdate();

            StopCoroutine(nameof(R_ViewOpenCoroutineEvent));
            StartCoroutine(nameof(R_ViewOpenCoroutineEvent));
            return true;
        }

        /// <summary>
        /// 실질적인 오픈 이벤트 함수로 pageinfo를 상속받는 함수에 오버라이드 함수를 작성하여 기능 구현
        /// </summary>
        public virtual IEnumerator R_ViewOpenCoroutineEvent()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    ViewOpenCoroutineEvent~");

            yield return null;
        }

        /// <summary>
        /// 페이지 오픈 시 디폴드로 실행되어야한는 함수로
        /// 뎁스설정과 UI연출 실행 기능을 함
        /// </summary>
        public void PageOpen(PageManager.PageDepth _page, bool _isPrevOpen)
        {
            currDepth = _page.indexPageDepth;
            CurrPage = _page.pageEnum;
            ViewOpen(_page.strPageOpenInfo, _isPrevOpen);
            //TODO : JH트레킹 코드
            //AccountManager.SendCustomViewEvent($"{CurrPage.ToString().ToLower()}_page");

            //TODO : JH바텀바 뉴아이콘 체크함수
            //BottomMenuManager?.NewCheck();

            if (TweenShow() == false)
                OnTweenEndEventShow();
        }

        /// <summary>
        /// 페이지가 오픈되어 있으나, 상위페이지가 오픈되어 다시 해당패이지가 오픈되어야 할경우 실행됨
        /// 오픈이벤트와 구성은 동일하나, 분리되어있는 이유는 오픈이벤트에서는 정보 갱신이 필요하나,
        /// 리오픈때는 정보갱신이 필요하지 않을 경우가 있기때문
        /// </summary>
        public void PageReOpen(PageManager.PageDepth _page, bool _isForceClose = false)
        {
            currDepth = _page.indexPageDepth;
            CurrPage = _page.pageEnum;
            ViewReOpen(_page.strPageOpenInfo);
            //TODO : JH페이지 전환시 토스트 팝업 스킵함수 필요
            //PageManager.Instance.SkipAllToastPopup();
            //TODO : JH트레킹 코드
            //AccountManager.SendCustomViewEvent($"{CurrPage.ToString().ToLower()}_page");

            //TODO : JH바텀바 뉴아이콘 체크함수
            //BottomMenuManager?.NewCheck();

            //if (TweenReShow() == false)
            if (_isForceClose == true)
                OnTweenEndEventShow();
        }

        /// <summary>
        /// 페이지 프리팹 활성화 함수
        /// </summary>
        public void ViewReOpen(string _strInOpenInfo)
        {
            gameObject.SetActive(true);
            ViewReOpenEvent(_strInOpenInfo);
        }

        /// <summary>
        /// 페이지 프리팹 활성화 될때 실해되는함수
        /// </summary>
        protected bool ViewReOpenEvent(string _strInOpenInfo)
        {
            ConvertOpenInfo(_strInOpenInfo);
            //if (isShow == true) return false;
            isShow = true;

            //TODO : JH재화 영역 UI업데이트 함수
            //if (TopMenuManager != null)
            //	TopMenuManager.SetUpdate();

            StopCoroutine(nameof(R_ViewReOpenCoroutineEvent));
            StartCoroutine(nameof(R_ViewReOpenCoroutineEvent));
            return true;
        }

        /// <summary>
        /// 실질적인 오픈 이벤트 함수로 pageinfo를 상속받는 함수에 오버라이드 함수를 작성하여 기능 구현
        /// </summary>
        public virtual IEnumerator R_ViewReOpenCoroutineEvent()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    ViewReOpenCoroutineEvent~");

            yield return null;
        }
        #endregion

        #region PageClose 관련함수

        /// <summary>
        /// 오브잭트 비활성 함수
        /// </summary>
        public void ViewHide()
        {
            isShow = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 오브잭트가 비활성때 실행되는 함수
        /// </summary>
        private void OnDisable()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == false)
                return;
#endif
            ViewHideEvent();
        }

        /// <summary>
        /// 실질적인 오브잭트 하이드 이벤트 함수로 pageinfo를 상속받는 함수에 오버라이드 함수를 작성하여 기능 구현
        /// </summary>
        public virtual bool ViewHideEvent()
        {
            if (isShow == false) return false;
            isShow = false;
            return true;
        }

        /// <summary>
        /// 실질적인 오브잭트 하이드 이벤트 함수로 pageinfo를 상속받는 함수에 오버라이드 함수를 작성하여 기능 구현
        /// </summary>
        public virtual void OnClick_Close()
        {
            if (PageManager == null) return;

            StopCoroutine(nameof(R_ViewOpenCoroutineEvent));
            StopCoroutine(nameof(R_ViewReOpenCoroutineEvent));
            PageManager.PagePrev();
        }

        /// <summary>
        /// 오브잭트와 이벤트 호출를 호출하는 함수 보통 해당 함수는 UI 닫기 버튼을 통해 호출됨
        /// </summary>
        public void ViewClose()
        {
            if (gameObject.activeInHierarchy == true)
            {
                Debug.Log($"[PageEnum]    {gameObject.name}    ViewClose~");
                ViewHide();
                ViewCloseEvent();
            }
        }


        /// <summary>
        /// 페이지 닫기 시 실행되는 함수
        /// </summary>
        public void PageClose()
        {
            if (IsTweenPossible() == true)
            {
                TweenHide();
            }
            else
            {
                CurrPage = PageEnum.None;
                ViewClose();
            }
        }


        /// <summary>
        /// 페이지 닫힘시 처리되어야하는 가상함수
        /// </summary>
        public virtual void ViewCloseEvent()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    ViewCloseEvent~");
        }

        /// <summary>
        /// 페이지 뒤로가기 시 실행되는 함수
        /// </summary>
        public virtual void OnClick_PrevPage()
        {
            OnClick_Close();
        }

        #endregion

        #region page설정 관련함수
        /// <summary>
        /// 페이지 오픈 체크 리턴 함수
        /// </summary>
        public bool GetIsActive()
        {
            return isShow;
        }

        /// <summary>
        /// 페이지 옵션 리턴함수
        /// </summary>
        public string GetOpenInfo()
        {
            return Command;
        }

        /// <summary>
        /// 스트링 데이터 컨버트 함수
        /// </summary>
        public virtual void ConvertOpenInfo(string _strOpenInfo)
        {
            Command = _strOpenInfo;
        }

        /// <summary>
        /// 제이슨 파일 변환함수
        /// </summary>
        public static string MakeCommand<T>(T _InT)
        {
            return JsonUtility.ToJson(_InT);
        }

        /// <summary>
        /// 제이슨 파일 변환함수
        /// </summary>
        public T ConvertCommand<T>()
        {
            return JsonUtility.FromJson<T>(Command);
        }
        #endregion

        #region Tween
        /// <summary>
        /// 트윈 실행 이벤트 체크 함수
        /// </summary>
        protected bool IsTweenPossible()
        {
            if (tweenPage_Show == null && tweenPage_Hide == null) return false;
            if (isTween == true) return false;
            return true;
        }

        /// <summary>
        /// 페이지 오픈 트윈이벤트 실행 함수
        /// </summary>
        public virtual bool TweenShow()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    TweenShow~");
            if (IsTweenPossible() == false) return false;
            isTween = true;
            misTweenShow = true;
            if (tweenPage_Show != null)
            {
                tweenPage_Show.DOPlayForward();
            }
            return true;
        }

        /// <summary>
        /// 페이지 리오픈 트윈이벤트 실행 함수
        /// </summary>
        public virtual bool TweenReShow()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    TweenReShow~");
            if (IsTweenPossible() == false) return false;
            isTween = true;
            misTweenShow = true;
            if (tweenPage_Show != null)
            {
                tweenPage_Show.DOPlayForward();
            }
            return true;
        }

        /// <summary>
        /// 페이지 하이드 트윈이벤트 실행 함수
        /// </summary>
        public virtual void TweenHide()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    TweenHide~");
            if (IsTweenPossible() == false) return;
            isTween = true;
            misTweenShow = false;
            if (tweenPage_Hide != null)
            {
                tweenPage_Hide.DOPlayBackwards();
            }
        }

        /// <summary>
        /// 트윈 이벤트 종료
        /// </summary>
        public virtual void OnTweenEnd()
        {
            Debug.Log($"[PageEnum]    {gameObject.name}    TweenEnd~");
            isTween = false;
            if (misTweenShow == true)
                OnTweenEndEventShow();
            else
                OnTweenEndEventHide();
        }

        /// <summary>
        /// 트윈 플레이 종료 후 기존 페이지 하이드 처리함수
        /// </summary>
        public virtual void OnTweenEndEventShow()
        {
            //TODO : JH게임유틸에서 게임카메라 제어함수 추가 해야함
            //GameUtil.ActiveGameSceneCam(PageManager.Instance.IsGameCameraActive(CurrPage), false);
        }

        /// <summary>
        /// 트윈 플레이 종료 후 기존 페이지 하이드 처리함수
        /// </summary>
        public virtual void OnTweenEndEventHide()
        {
            CurrPage = PageEnum.None;
            ViewClose();
            //TODO : JH텍스처 메니져에 텍스쳐 텍스처 딜리트 함수 추가해야함
            //PageManager.Instance.UnloadTexture();
        }
        #endregion
    }
}
