using UnityEngine;
using UnityEngine.UI; // Slider는 여전히 UnityEngine.UI에 포함됩니다
using TMPro;          // TextMeshPro 기능을 사용하기 위해 필수

public class UIManager : MonoBehaviour
{
    [Header("대상 오브젝트")]
    public Rigidbody2D targetRb;

    [Header("UI 요소")]
    public Slider massSlider;
    public Slider gravitySlider;

    // 기존 Text 대신 TMP_Text를 사용합니다
    public TMP_Text massValueText;
    public TMP_Text gravityValueText;

    void Start()
    {
        massSlider.onValueChanged.AddListener(OnMassChanged);
        gravitySlider.onValueChanged.AddListener(OnGravityChanged);

        OnMassChanged(massSlider.value);
        OnGravityChanged(gravitySlider.value);
    }

    public void OnMassChanged(float value)
    {
        if (targetRb != null)
        {
            targetRb.mass = value;
            if (massValueText != null)
                massValueText.text = $"Mass: {value:F1}";
        }
    }

    public void OnGravityChanged(float value)
    {
        if (targetRb != null)
        {
            targetRb.gravityScale = value;
            if (gravityValueText != null)
                gravityValueText.text = $"Gravity: {value:F1}";
        }
    }

    public void OnClickStart()
    {
        GameManager.Instance.GameStart();
    }

    public void OnClickReset()
    {
        GameManager.Instance.GameReset();
    }
}