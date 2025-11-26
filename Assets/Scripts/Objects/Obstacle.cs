using UnityEngine;

public class Obstacle : PhysicsObjectBase
{
    // 부모의 Awake를 먼저 실행하고 추가 초기화가 필요하면 작성
    protected override void Awake()
    {
        base.Awake();
    }

    // 마우스 클릭 시 호출 (기존 로직 유지)
    void OnMouseDown()
    {
        // UI가 가려져 있거나 게임 중에는 클릭 무시 등의 예외처리는 필요에 따라 추가

        if (GameManager.Instance == null) return;

        // 편집 모드일 때만 선택 가능
        if (!GameManager.Instance.isPlayMode)
        {
            // 부모 타입(PhysicsObjectBase)으로 넘겨도 되고, 자신을 넘겨도 됨
            UIManager.Instance.SelectObject(this);
        }
    }
}