using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI 요소 (슬라이더)")]
    public Slider massSlider;
    public Slider gravitySlider;
    public Slider bouncinessSlider;
    public Slider frictionSlider;

    [Header("UI 요소 (텍스트)")]
    public TMP_Text massValueText;
    public TMP_Text gravityValueText;
    public TMP_Text bouncinessValueText;
    public TMP_Text frictionValueText;

    [Header("선택 정보")]
    public TMP_Text selectedObjectNameText; // 현재 선택된 물체 이름 표시

    // [변경됨] 기존 PhysicsObjectSettings 대신 부모 클래스인 PhysicsObjectBase를 참조
    private PhysicsObjectBase currentSelectedObject;

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
        ResetUIState();
    }

    // [변경됨] 매개변수 타입을 PhysicsObjectBase로 변경
    public void SelectObject(PhysicsObjectBase obj)
    {
        currentSelectedObject = obj;

        // 1. 선택된 물체의 현재 값으로 슬라이더 업데이트
        // (슬라이더 값 변경 시 이벤트가 발생하여 값이 덮어씌워지는 것을 방지하기 위해 일시적으로 리스너를 제거하거나 플래그를 쓸 수 있지만, 여기선 간단히 값만 대입)
        massSlider.value = obj.GetMass();
        gravitySlider.value = obj.GetGravity();
        bouncinessSlider.value = obj.GetBounciness();
        frictionSlider.value = obj.GetFriction();

        // 2. [추가됨] 조절 가능한 속성인지 확인하여 슬라이더 활성화/비활성화
        massSlider.interactable = obj.canEditMass;
        gravitySlider.interactable = obj.canEditGravity;
        bouncinessSlider.interactable = obj.canEditBounciness;
        frictionSlider.interactable = obj.canEditFriction;

        // 3. UI 텍스트 업데이트
        if (selectedObjectNameText != null)
            selectedObjectNameText.text = $"Selected: {obj.name}";

        UpdateUIDisplay();
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
            ResetUIState();
            return;
        }

        if (massValueText != null) massValueText.text = $"Mass: {massSlider.value:F1}";
        if (gravityValueText != null) gravityValueText.text = $"Gravity: {gravitySlider.value:F1}";
        if (bouncinessValueText != null) bouncinessValueText.text = $"Bounce: {bouncinessSlider.value:F2}";
        if (frictionValueText != null) frictionValueText.text = $"Friction: {frictionSlider.value:F2}";
    }

    // 선택 해제 혹은 초기화 상태
    private void ResetUIState()
    {
        if (selectedObjectNameText != null) selectedObjectNameText.text = "Select an Object";

        // 슬라이더 비활성화 (선택된 게 없으므로)
        massSlider.interactable = false;
        gravitySlider.interactable = false;
        bouncinessSlider.interactable = false;
        frictionSlider.interactable = false;
    }

    // 버튼 연결용
    public void OnClickStart() => GameManager.Instance.GameStart();
    public void OnClickReset() => GameManager.Instance.GameReset();
}