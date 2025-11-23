using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PhysicsObjectSettings : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private PhysicsMaterial2D mat; // 런타임에 생성할 독립적인 재질

    [Header("초기 값 설정")]
    public float defaultBounciness = 0.5f; // 탄성 (0~1)
    public float defaultFriction = 0.4f;   // 마찰력 (0~1)

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // 중요: 모든 물체가 같은 재질을 공유하지 않도록, 
        // 각 물체마다 새로운 물리 재질(PhysicsMaterial2D)을 생성해서 할당합니다.
        mat = new PhysicsMaterial2D(gameObject.name + "_Mat");
        mat.bounciness = defaultBounciness;
        mat.friction = defaultFriction;

        col.sharedMaterial = mat;
    }

    // 마우스로 이 물체를 클릭했을 때 호출됨
    void OnMouseDown()
    {
        // 1. 클릭 자체가 들어오는지 확인
        Debug.Log($"[클릭 감지] {gameObject.name} 클릭됨!");

        if (GameManager.Instance == null)
        {
            Debug.LogError("게임 매니저가 없습니다!");
            return;
        }

        // 2. 모드 확인
        if (!GameManager.Instance.isPlayMode)
        {
            Debug.Log("선택 로직 실행");
            UIManager.Instance.SelectObject(this);
        }
        else
        {
            Debug.Log("게임 플레이 중이라 선택할 수 없습니다.");
        }
    }

    // --- UIManager에서 호출할 함수들 ---

    public void SetMass(float value)
    {
        rb.mass = value;
    }

    public void SetGravity(float value)
    {
        rb.gravityScale = value;
    }

    public void SetBounciness(float value)
    {
        if (mat != null)
        {
            mat.bounciness = value;
            // 런타임에 물리 재질을 갱신하려면 콜라이더를 껐다 켜야 확실히 적용될 때가 있음
            col.enabled = false;
            col.enabled = true;
        }
    }

    public void SetFriction(float value)
    {
        if (mat != null)
        {
            mat.friction = value;
            col.enabled = false;
            col.enabled = true;
        }
    }

    // 현재 값을 UI에 보여주기 위해 반환하는 함수들
    public float GetMass() => rb.mass;
    public float GetGravity() => rb.gravityScale;
    public float GetBounciness() => mat != null ? mat.bounciness : 0f;
    public float GetFriction() => mat != null ? mat.friction : 0f;
}