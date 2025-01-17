using System;
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
        
        OnIslandSelected?.Invoke(currentSelectedIsland);
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
