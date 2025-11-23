using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // 싱글톤 추가 (어디서든 접근 가능하게)

    [Header("UI 요소 (슬라이더)")]
    public Slider massSlider;
    public Slider gravitySlider;
    public Slider bouncinessSlider; // [추가됨] 탄성
    public Slider frictionSlider;   // [추가됨] 마찰

    [Header("UI 요소 (텍스트)")]
    public TMP_Text massValueText;
    public TMP_Text gravityValueText;
    public TMP_Text bouncinessValueText; // [추가됨]
    public TMP_Text frictionValueText;   // [추가됨]

    [Header("선택 정보")]
    public TMP_Text selectedObjectNameText; // 현재 선택된 물체 이름 표시용

    // 현재 선택된 오브젝트를 저장하는 변수
    private PhysicsObjectSettings currentSelectedObject;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // 슬라이더 리스너 연결
        massSlider.onValueChanged.AddListener(OnMassChanged);
        gravitySlider.onValueChanged.AddListener(OnGravityChanged);
        bouncinessSlider.onValueChanged.AddListener(OnBouncinessChanged);
        frictionSlider.onValueChanged.AddListener(OnFrictionChanged);

        // 초기 상태: 아무것도 선택 안 된 상태 처리
        UpdateUIDisplay();
    }

    // PhysicsObjectSettings 스크립트에서 호출함
    public void SelectObject(PhysicsObjectSettings obj)
    {
        currentSelectedObject = obj;

        // 선택된 물체의 현재 값으로 슬라이더 업데이트
        // (슬라이더 값을 변경할 때 OnValueChanged가 호출되어 다시 설정되는 것을 방지하기 위해 리스너 일시 무시 로직이 필요할 수도 있으나, 간단히 구현)
        massSlider.value = obj.GetMass();
        gravitySlider.value = obj.GetGravity();
        bouncinessSlider.value = obj.GetBounciness();
        frictionSlider.value = obj.GetFriction();

        if (selectedObjectNameText != null)
            selectedObjectNameText.text = $"Selected: {obj.name}";

        UpdateUIDisplay(); // 텍스트 갱신
    }

    // 슬라이더 조작 시 호출되는 함수들
    public void OnMassChanged(float value)
    {
        if (currentSelectedObject != null) currentSelectedObject.SetMass(value);
        UpdateUIDisplay();
    }

    public void OnGravityChanged(float value)
    {
        if (currentSelectedObject != null) currentSelectedObject.SetGravity(value);
        UpdateUIDisplay();
    }

    public void OnBouncinessChanged(float value)
    {
        if (currentSelectedObject != null) currentSelectedObject.SetBounciness(value);
        UpdateUIDisplay();
    }

    public void OnFrictionChanged(float value)
    {
        if (currentSelectedObject != null) currentSelectedObject.SetFriction(value);
        UpdateUIDisplay();
    }

    // 텍스트 UI 일괄 갱신
    private void UpdateUIDisplay()
    {
        if (currentSelectedObject == null)
        {
            if (selectedObjectNameText != null) selectedObjectNameText.text = "Select an Object";
            return;
        }

        if (massValueText != null) massValueText.text = $"Mass: {massSlider.value:F1}";
        if (gravityValueText != null) gravityValueText.text = $"Gravity: {gravitySlider.value:F1}";
        if (bouncinessValueText != null) bouncinessValueText.text = $"Bounce: {bouncinessSlider.value:F2}";
        if (frictionValueText != null) frictionValueText.text = $"Friction: {frictionSlider.value:F2}";
    }

    // 기존 버튼 연결용
    public void OnClickStart() => GameManager.Instance.GameStart();
    public void OnClickReset() => GameManager.Instance.GameReset();
}