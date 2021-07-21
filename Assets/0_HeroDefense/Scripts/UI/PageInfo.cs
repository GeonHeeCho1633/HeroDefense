using UnityEngine.UI;
using UnityEngine;

namespace OVERMAN_UI
{
    public enum PageEnum
    {
        None,
        MainPage,
    }

    public class PageInfo : MonoBehaviour
    {
        public int indexCurrentDepth;
        public PageEnum CurrentPage = PageEnum.None;
        [HideInInspector] public Canvas myCanvas;
        public bool isTween = false;
        public bool isPrevPageShow = false;

        private bool isShow
        {
            get
            {
                return misShow;
            }
            set
            {
                misShow = value;
            }
        }
        private bool misShow = false;           
        private string mCommand = "";           
        public bool isIgnoreReOpen = false;
    }
}