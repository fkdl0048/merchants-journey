using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.UI;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Manager
{
    // 요거는 MVVM 패턴으로 사용하려고
    public class UIManager : Singleton<UIManager>
    {
        private Canvas mainCanvas;
        private Stack<PopupBase> popupStack = new Stack<PopupBase>();
        private Dictionary<string, GameObject> uiPrefabCache = new Dictionary<string, GameObject>();
        
        protected override void Awake()
        {
            base.Awake();
            InitializeMainCanvas();
        }

        private void InitializeMainCanvas()
        {
            mainCanvas = gameObject.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvas.sortingOrder = 1;

            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 1;

            gameObject.AddComponent<GraphicRaycaster>();
        }

        public T LoadUI<T>(string path) where T : UIBase
        {
            if (!uiPrefabCache.ContainsKey(path))
            {
                GameObject prefab = Resources.Load<GameObject>(path);
                if (prefab == null)
                {
                    Debug.LogError($"Failed to load UI prefab: {path}");
                    return null;
                }
                uiPrefabCache[path] = prefab;
            }

            GameObject instance = Instantiate(uiPrefabCache[path], mainCanvas.transform);
            return instance.GetComponent<T>();
        }

        public T ShowUI<T>(string path) where T : UIBase
        {
            T ui = LoadUI<T>(path);
            if (ui != null)
            {
                ui.Show();
            }
            return ui;
        }

        public T ShowPopup<T>(string path) where T : PopupBase
        {
            T popup = LoadUI<T>(path);
            if (popup != null)
            {
                ShowDimBackground();
                popupStack.Push(popup);
                popup.Show();
            }
            return popup;
        }

        public void ClosePopup(PopupBase popup)
        {
            if (popupStack.Count == 0) return;

            var topPopup = popupStack.Pop();
            if (popup != topPopup)
            {
                popupStack.Push(topPopup);
                return;
            }

            popup.Hide();
            Destroy(popup.gameObject);

            if (popupStack.Count == 0)
            {
                HideDimBackground();
            }
        }

        private GameObject dimBackground;

        private void ShowDimBackground()
        {
            if (dimBackground == null)
            {
                dimBackground = new GameObject("DimBackground");
                var image = dimBackground.AddComponent<Image>();
                image.color = new Color(0, 0, 0, 0.5f);
                image.raycastTarget = true;

                var rectTransform = dimBackground.GetComponent<RectTransform>();
                rectTransform.SetParent(mainCanvas.transform);
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.sizeDelta = Vector2.zero;
            }

            dimBackground.transform.SetAsLastSibling();
            dimBackground.SetActive(true);
        }

        private void HideDimBackground()
        {
            if (dimBackground != null)
            {
                dimBackground.SetActive(false);
            }
        }
    }
}