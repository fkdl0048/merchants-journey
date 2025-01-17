using System;
using DG.Tweening;
using Scripts.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 월드맵 UI
/// 현재 사용자 데이터에 맞게, UI ICON을 활성화해야함 => SaveManager에서 데이터를 불러와야함
/// 현재 레벨에 따라 활성화
/// </summary>
public class WorldMapUI : MonoBehaviour
{
    [Header("World Map UI References")]
    [SerializeField] private Button UpgradesButton;
    [SerializeField] private Button NextStageButton;
    [SerializeField] private Button[] IslandButtons;  // 섬 버튼들의 배열
    [SerializeField] private Camera mainCamera;  // 메인 카메라 참조
    [SerializeField] private float cameraMoveSpeed = 1f;  // 카메라 이동 속도
    [SerializeField] private RectTransform worldMapContent;  // 월드맵 전체를 담고 있는 ScrollRect의 content

    public UnityAction<int> OnIslandSelected;  // 섬이 선택되었을 때 이벤트
    public UnityAction<int> OnNextStageButtonClicked;
    public UnityAction OnUpgradeButtonClicked;

    private int currentSelectedIsland = -1;  // 현재 선택된 섬의 인덱스 (-1은 선택되지 않음)

    private void Awake()
    {
        InitializeButtons();
        NextStageButton.interactable = false;  // 초기에는 Next Stage 버튼 비활성화
    }

    // 스테이지를 다녀온 후 호출되어 UI를 업데이트
    public void Initialized()
    {
        ResetSelection();  // 이전 선택 초기화
        UpdateIslandButtonsState();  // 현재 스테이지에 맞게 섬 버튼들 업데이트
    }

    private void UpdateIslandButtonsState()
    {
        if (!SaveManager.Instance)
        {
            Debug.LogError("SaveManager instance not found!");
            return;
        }

        int currentStage = SaveManager.Instance.CurrentStage;
        Debug.Log($"Current Stage: {currentStage}");

        // 각 섬 버튼의 활성화 상태 설정
        for (int i = 0; i < IslandButtons.Length; i++)
        {
            bool isEnabled = i < currentStage;  // 현재 스테이지보다 작거나 같은 번호의 섬만 활성화
            IslandButtons[i].interactable = isEnabled;

            // 비활성화된 버튼의 시각적 표시
            var buttonImage = IslandButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                Color color = buttonImage.color;
                color.a = isEnabled ? 1f : 0.5f;  // 비활성화된 섬은 반투명하게 표시
                buttonImage.color = color;
            }
        }
    }

    private void InitializeButtons()
    {
        // 업그레이드 버튼 초기화
        UpgradesButton.onClick.AddListener(() => { OnUpgradeButtonClicked?.Invoke(); });

        // Next Stage 버튼 초기화
        NextStageButton.onClick.AddListener(() =>
        {
            if (currentSelectedIsland != -1)
            {
                AudioManager.Instance.PlaySFX("Audio/SFX/Button");
                Debug.Log($"Moving to island {currentSelectedIsland}");
                OnNextStageButtonClicked?.Invoke(currentSelectedIsland);
            }
        });

        // 각 섬 버튼 초기화
        for (int i = 0; i < IslandButtons.Length; i++)
        {
            int islandIndex = i;  // 클로저를 위한 로컬 변수
            IslandButtons[i].onClick.AddListener(() => SelectIsland(islandIndex));
        }
    }

    private void SelectIsland(int index)
    {
        // 비활성화된 섬은 선택할 수 없음
        if (!IslandButtons[index].interactable) return;

        if (currentSelectedIsland == index) return;  // 이미 선택된 섬이면 무시

        // 이전에 선택된 섬의 버튼 상태 초기화
        if (currentSelectedIsland != -1)
        {
            SetIslandButtonState(currentSelectedIsland, false);
        }

        // 새로운 섬 선택
        currentSelectedIsland = index;
        SetIslandButtonState(currentSelectedIsland, true);
        NextStageButton.interactable = true;  // Next Stage 버튼 활성화
        
        // 선택된 섬으로 카메라 이동
        MoveCamera(IslandButtons[index].transform.position);
        
        OnIslandSelected?.Invoke(currentSelectedIsland);
    }

    private void MoveCamera(Vector3 targetPosition)
    {
        if (worldMapContent == null)
        {
            Debug.LogError("World Map Content가 설정되지 않았습니다!");
            return;
        }

        // 선택된 버튼의 RectTransform 가져오기
        RectTransform buttonRect = IslandButtons[currentSelectedIsland].GetComponent<RectTransform>();
        
        // 버튼의 월드 위치 구하기
        Vector3 buttonWorldPos = buttonRect.TransformPoint(Vector3.zero);
        
        // ScrollRect의 viewport 중심점 구하기
        RectTransform viewport = worldMapContent.parent as RectTransform;
        if (viewport == null)
        {
            Debug.LogError("Viewport를 찾을 수 없습니다!");
            return;
        }
        Vector3 viewportCenter = viewport.TransformPoint(viewport.rect.center);
        
        // 버튼과 viewport 중심점 간의 차이 계산
        Vector3 offset = buttonWorldPos - viewportCenter;
        
        // 현재 content의 anchoredPosition에서 offset만큼 이동
        Vector2 targetAnchoredPos = worldMapContent.anchoredPosition - (Vector2)worldMapContent.InverseTransformVector(offset);
        
        // DOTween을 사용하여 부드럽게 스크롤
        worldMapContent.DOAnchorPos(targetAnchoredPos, cameraMoveSpeed)
            .SetEase(Ease.InOutQuad);
    }

    private void SetIslandButtonState(int index, bool selected)
    {
        // 버튼의 시각적 상태 변경 (예: 색상 변경)
        var colors = IslandButtons[index].colors;
        colors.normalColor = selected ? Color.yellow : Color.white;
        IslandButtons[index].colors = colors;
    }

    // 외부에서 UI 초기화가 필요할 때 호출
    public void ResetSelection()
    {
        if (currentSelectedIsland != -1)
        {
            SetIslandButtonState(currentSelectedIsland, false);
        }
        currentSelectedIsland = -1;
        NextStageButton.interactable = false;
    }
}
