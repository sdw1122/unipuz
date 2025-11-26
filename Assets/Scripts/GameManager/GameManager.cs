using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isPlayMode = false;

    [Header("UI 참조")]
    public GameObject clearPanel;

    // [변경됨] 자식 클래스가 아닌 부모 클래스(Base) 배열로 선언
    private PhysicsObjectBase[] physicsObjects;

    // 초기 상태 저장용 구조체나 배열
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (clearPanel != null) clearPanel.SetActive(false);
        Time.timeScale = 1f;

        // [핵심 변경] 씬에 있는 모든 PhysicsObjectBase(자식들 포함)를 다 찾음
        physicsObjects = FindObjectsByType<PhysicsObjectBase>(FindObjectsSortMode.None);

        initialPositions = new Vector3[physicsObjects.Length];
        initialRotations = new Quaternion[physicsObjects.Length];

        for (int i = 0; i < physicsObjects.Length; i++)
        {
            initialPositions[i] = physicsObjects[i].transform.position;
            initialRotations[i] = physicsObjects[i].transform.rotation;

            var rb = physicsObjects[i].GetComponent<Rigidbody2D>();

            // 일단 모두 멈춤 상태로 시작
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void GameStart()
    {
        if (isPlayMode) return;
        isPlayMode = true;
        Time.timeScale = 1f;

        // 저장해둔 리스트 순회
        foreach (var obj in physicsObjects)
        {
            // [중요] '고정됨(isStatic)'이 체크된 물체(벽, 바닥 등)는 떨어지면 안 됨!
            if (obj.isStatic) continue;
            if (obj.CompareTag("Player")) continue;
            // 고정되지 않은 물체만 물리 연산 시작
            var rb = obj.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // 플레이어 발사 준비
        PlayerController.Instance.SetReadyToShoot(true);
    }

    public void GameReset()
    {
        isPlayMode = false;
        Time.timeScale = 1f;
        if (clearPanel != null) clearPanel.SetActive(false);

        for (int i = 0; i < physicsObjects.Length; i++)
        {
            // 위치/회전 복구
            physicsObjects[i].transform.position = initialPositions[i];
            physicsObjects[i].transform.rotation = initialRotations[i];

            var rb = physicsObjects[i].GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // 다시 Kinematic으로 고정 (편집 모드 상태)
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void StageClear()
    {
        Debug.Log("Game Clear Logic Executed");
        if (clearPanel != null) clearPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}