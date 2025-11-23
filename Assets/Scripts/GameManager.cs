using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPlayMode = false;

    [Header("UI 참조")]
    public GameObject clearPanel;

    private Rigidbody2D[] physicsObjects;
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

        physicsObjects = FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

        initialPositions = new Vector3[physicsObjects.Length];
        initialRotations = new Quaternion[physicsObjects.Length];

        for (int i = 0; i < physicsObjects.Length; i++)
        {
            initialPositions[i] = physicsObjects[i].transform.position;
            initialRotations[i] = physicsObjects[i].transform.rotation;

            // [수정됨] simulated를 끄지 말고, Kinematic으로 바꿔서 멈춰둡니다.
            // 이렇게 해야 마우스 클릭(Raycast)이 감지됩니다.
            physicsObjects[i].bodyType = RigidbodyType2D.Kinematic;

            // 혹시 모를 속도 제거
            physicsObjects[i].linearVelocity = Vector2.zero;
            physicsObjects[i].angularVelocity = 0f;
        }
    }

    public void GameStart()
    {
        if (isPlayMode) return;
        isPlayMode = true;
        Time.timeScale = 1f;

        foreach (var rb in physicsObjects)
        {
            // [수정됨] 게임 시작 시 다시 Dynamic(일반 물리)으로 변경
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        PlayerController.Instance.Shoot();
    }

    public void GameReset()
    {
        isPlayMode = false;
        Time.timeScale = 1f;
        if (clearPanel != null) clearPanel.SetActive(false);

        for (int i = 0; i < physicsObjects.Length; i++)
        {
            physicsObjects[i].transform.position = initialPositions[i];
            physicsObjects[i].transform.rotation = initialRotations[i];

            physicsObjects[i].linearVelocity = Vector2.zero;
            physicsObjects[i].angularVelocity = 0f;

            // [수정됨] 리셋 시 다시 Kinematic으로 고정
            physicsObjects[i].bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void StageClear()
    {
        Debug.Log("Game Clear Logic Executed");
        if (clearPanel != null) clearPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}