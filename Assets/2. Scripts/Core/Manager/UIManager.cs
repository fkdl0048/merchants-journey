using System.Collections.Generic;
using Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        private Canvas mainCanvas;
        private Stack<PopupBase> popupStack = new Stack<PopupBase>();
        private Dictionary<string, GameObject> popupPrefabCache = new Dictionary<string, GameObject>();

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

        public T ShowPopup<T>(string path) where T : PopupBase
        {
            if (!popupPrefabCache.ContainsKey(path))
            {
                GameObject prefab = Resources.Load<GameObject>(path);
                if (prefab == null)
                {
                    Debug.LogError($"Failed to load popup prefab: {path}");
                    return null;
                }
                popupPrefabCache[path] = prefab;
            }

            GameObject instance = Instantiate(popupPrefabCache[path], mainCanvas.transform);
            T popup = instance.GetComponent<T>();
            
            if (popup != null)
            {
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
        }
    }
}