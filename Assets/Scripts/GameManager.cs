using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ����(���� ��� vs ���� ���)�� �����ϴ� �ٽ� �Ŵ����Դϴ�.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ���� ������ ���� ��(���� ���� ��)���� Ȯ���ϴ� ����
    public bool isPlayMode = false;

    // �� ���� ��� ���� ������Ʈ(��, ��ֹ� ��)�� ������ �迭
    private Rigidbody2D[] physicsObjects;
    // ������Ʈ���� �ʱ� ��ġ�� ȸ������ ������ ����ü �迭
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;

    void Awake()
    {
        // �̱��� ����: ��𼭵� GameManager.Instance�� ���� �����ϰ� ����
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // ���� �ִ� ��� Rigidbody2D ������Ʈ�� ã�� �迭�� ����
        physicsObjects = FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

        // ������ ���� �ʱ� ��ġ ����� �迭 �ʱ�ȭ
        initialPositions = new Vector3[physicsObjects.Length];
        initialRotations = new Quaternion[physicsObjects.Length];

        for (int i = 0; i < physicsObjects.Length; i++)
        {
            initialPositions[i] = physicsObjects[i].transform.position;
            initialRotations[i] = physicsObjects[i].transform.rotation;

            // ���� �ÿ��� ���� ������ ���� (���� ���)
            physicsObjects[i].simulated = false;
        }
    }

    // UI�� ���� ��ư�� ������ �Լ�
    public void GameStart()
    {
        if (isPlayMode) return;

        isPlayMode = true;

        // ��� ������Ʈ�� ���� ������ Ȱ��ȭ
        foreach (var rb in physicsObjects)
        {
            rb.simulated = true;
        }

        // �÷��̾�(��)���� ���� ���ϴ� ���� ����
        PlayerController.Instance.Shoot();
    }

    // UI�� ���� ��ư�� ������ �Լ�
    public void GameReset()
    {
        isPlayMode = false;

        // ��ġ�� ���� ���� �ʱ�ȭ
        for (int i = 0; i < physicsObjects.Length; i++)
        {
            physicsObjects[i].transform.position = initialPositions[i];
            physicsObjects[i].transform.rotation = initialRotations[i];
            physicsObjects[i].linearVelocity = Vector2.zero;
            physicsObjects[i].angularVelocity = 0f;
            physicsObjects[i].simulated = false;
        }
    }
}