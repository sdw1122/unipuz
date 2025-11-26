using UnityEngine;

// [변경] MonoBehaviour 대신 PhysicsObjectBase를 상속받아 물리 설정 기능을 가져옵니다.
public class PlayerController : PhysicsObjectBase
{
    public static PlayerController Instance;

    [Header("발사 설정")]
    public float powerMultiplier = 3f; // 드래그 거리 대비 힘의 배율
    public float maxPower = 15f;       // 최대 힘 제한

    [Header("참조")]
    public Transform arrowTransform;   // 화살표 (방향/크기 표시)
    public SpriteRenderer arrowSprite; // 화살표 이미지 (크기 조절용)


    private Vector2 startPoint;
    private Vector2 currentPoint;
    private bool isAiming = false;
    private bool canShoot = false;

    // [중요] 부모 클래스의 Awake를 오버라이드하여 실행
    protected override void Awake()
    {
        base.Awake(); // PhysicsObjectBase의 설정(재질 생성 등) 초기화
        Instance = this;
    }

    void Start()
    {
        // 시작 시 화살표 숨김
        if (arrowTransform != null) arrowTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (canShoot && isAiming)
        {
            UpdateArrow();
        }
    }

    // GameManager에서 호출
    public void SetReadyToShoot(bool state)
    {
        canShoot = state;
        if (state)
        {
            // 발사 준비: 물리 끄고 멈춤
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    // 게임 리셋 시 호출
    public void ResetPlayer()
    {
        canShoot = false;
        isAiming = false;
        if (arrowTransform != null) arrowTransform.gameObject.SetActive(false);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    // 마우스 클릭 시: [1] 오브젝트 선택(UI) + [2] 조준 시작
    private void OnMouseDown()
    {
        // 1. 편집 모드라면 UIManager에 "나를 선택해줘"라고 요청 (Obstacle의 기능)
        if (GameManager.Instance != null && !GameManager.Instance.isPlayMode)
        {
            UIManager.Instance.SelectObject(this);
        }

        // 2. 발사 가능 상태라면 조준 시작
        if (!canShoot) return;

        isAiming = true;
        startPoint = transform.position;
        if (arrowTransform != null) arrowTransform.gameObject.SetActive(true);
    }

    private void OnMouseDrag()
    {
        if (!canShoot || !isAiming) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPoint = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    }

    private void OnMouseUp()
    {
        if (!canShoot || !isAiming) return;

        Shoot();
        isAiming = false;
        if (arrowTransform != null) arrowTransform.gameObject.SetActive(false);
    }

    void UpdateArrow()
    {
        Vector2 pullDirection = startPoint - currentPoint;
        float distance = Vector2.Distance(startPoint, currentPoint);
        float power = Mathf.Clamp(distance * powerMultiplier, 0, maxPower);

        float angle = Mathf.Atan2(pullDirection.y, pullDirection.x) * Mathf.Rad2Deg;
        arrowTransform.rotation = Quaternion.Euler(0, 0, angle);

        float scaleX = power * 0.2f;
        arrowTransform.localScale = new Vector3(scaleX, 1f, 1f);
    }

    void Shoot()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 pullDirection = (startPoint - currentPoint).normalized;
        float distance = Vector2.Distance(startPoint, currentPoint);
        float finalPower = Mathf.Clamp(distance * powerMultiplier, 0, maxPower);

        rb.AddForce(pullDirection * finalPower, ForceMode2D.Impulse);
        canShoot = false;
    }
}