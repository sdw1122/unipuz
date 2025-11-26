using System;
using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    // 별 오브젝트의 Collider 2D에서 'Is Trigger'를 체크해야 합니다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 물체가 우리가 설정한 'TargetBox(네모)'인지 확인
        if (collision.CompareTag("Player"))
        {
            Debug.Log("스테이지 클리어!");

            // 게임 매니저에게 승리 사실을 알림
            GameManager.Instance.StageClear();
        }
        else if(collision.CompareTag("TargetBox"))
        {
            Debug.Log("스테이지 클리어!");

            // 게임 매니저에게 승리 사실을 알림
            GameManager.Instance.StageClear();
        }
    }
}