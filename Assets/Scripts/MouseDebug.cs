using UnityEngine;

public class MouseDebug : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 해당 위치에 있는 모든 2D 콜라이더 검사
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log($"[디버그] 감지된 물체: {hit.collider.gameObject.name}");
            }
            else
            {
                Debug.Log("[디버그] 아무것도 감지되지 않음 (허공 클릭)");
            }
        }
    }
}