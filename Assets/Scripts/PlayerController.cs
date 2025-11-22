using UnityEngine;

// 공(Player) 오브젝트에 붙여서 힘을 가하고 화살표를 제어하는 스크립트입니다.
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("설정 값")]
    public float power = 10f; // 발사 힘의 크기

    [Header("참조")]
    public Transform arrowTransform; // 힘의 방향을 표시할 화살표 오브젝트
    private Rigidbody2D rb;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 게임이 시작되지 않았을 때(편집 모드)만 화살표 회전 가능
        if (!GameManager.Instance.isPlayMode)
        {
            RotateArrow();
        }
    }

    // 마우스 위치를 바라보도록 화살표 회전
    void RotateArrow()
    {
        if (arrowTransform == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 화살표가 공을 중심으로 회전
        arrowTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // GameManager에서 호출하여 실제로 힘을 가하는 함수
    public void Shoot()
    {
        if (arrowTransform != null)
        {
            // 화살표의 오른쪽 방향(빨간 축)을 기준으로 힘을 가함
            Vector2 forceDirection = arrowTransform.right;
            rb.AddForce(forceDirection * power, ForceMode2D.Impulse);
        }
    }
}