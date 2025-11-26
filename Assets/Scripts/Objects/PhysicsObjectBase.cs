using UnityEngine;

// 모든 물리 오브젝트의 공통 기능을 정의하는 부모 클래스입니다.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PhysicsObjectBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected PhysicsMaterial2D mat; // 런타임에 생성할 독립적인 재질
    [Header("0. 오브젝트 타입 설정")]
    public bool isStatic = false; // [추가됨] 체크하면 게임 시작해도 안 떨어짐 (벽, 바닥용)

    [Header("1. 조절 가능한 물리량 선택 (체크하면 활성화)")]
    public bool canEditMass = true;
    public bool canEditGravity = true;
    public bool canEditBounciness = true;
    public bool canEditFriction = true;

    [Header("2. 기본 물리 속성 값")]
    [SerializeField] protected float currentMass = 1f;
    [SerializeField] protected float currentGravity = 1f;
    [SerializeField] protected float currentBounciness = 0.5f;
    [SerializeField] protected float currentFriction = 0.4f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // 각 오브젝트마다 독립적인 물리 재질 생성
        mat = new PhysicsMaterial2D(gameObject.name + "_Mat");
        mat.bounciness = currentBounciness;
        mat.friction = currentFriction;

        col.sharedMaterial = mat;

        // 초기값 적용
        ApplyPhysicsSettings();
    }

    // 변경된 값을 실제 물리 엔진에 적용하는 함수
    public virtual void ApplyPhysicsSettings()
    {
        if (rb == null || mat == null) return;

        rb.mass = currentMass;
        rb.gravityScale = currentGravity;
        mat.bounciness = currentBounciness;
        mat.friction = currentFriction;

        // 물리 재질 갱신을 위해 콜라이더 리프레시
        if (col != null && col.enabled)
        {
            col.enabled = false;
            col.enabled = true;
        }
    }

    // --- 값 설정 함수들 (UIManager에서 호출) ---
    // 만약 해당 속성이 '수정 불가(canEdit == false)'라면 값을 변경하지 않도록 방어 로직 추가 가능

    public void SetMass(float value)
    {
        if (!canEditMass) return;
        currentMass = value;
        ApplyPhysicsSettings();
    }

    public void SetGravity(float value)
    {
        if (!canEditGravity) return;
        currentGravity = value;
        ApplyPhysicsSettings();
    }

    public void SetBounciness(float value)
    {
        if (!canEditBounciness) return;
        currentBounciness = value;
        ApplyPhysicsSettings();
    }

    public void SetFriction(float value)
    {
        if (!canEditFriction) return;
        currentFriction = value;
        ApplyPhysicsSettings();
    }

    // --- 값 가져오기 함수들 ---
    public float GetMass() => currentMass;
    public float GetGravity() => currentGravity;
    public float GetBounciness() => currentBounciness;
    public float GetFriction() => currentFriction;
}