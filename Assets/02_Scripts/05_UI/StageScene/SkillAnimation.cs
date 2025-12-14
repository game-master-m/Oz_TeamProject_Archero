using UnityEngine;
using DG.Tweening;
using TMPro;

public class SkillAnimation : MonoBehaviour
{
    [Header("Modules")]
    [SerializeField] private SkillSlotEffect slotEffect; // 위에서 만든 스크립트 연결

    [Header("UI Elements")]
    [SerializeField] private RectTransform gradeTextRect;
    [SerializeField] private CanvasGroup infoCanvasGroup; // Name + Desc
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text gradeText;

    [Header("Settings")]
    [SerializeField] private float spinDuration = 1.5f; // 몇 초 동안 돌릴지

    private Vector2 initGradePos;

    private void Awake()
    {
        initGradePos = gradeTextRect.anchoredPosition;
    }

    public void ShowSkill(Sprite icon, string name, string grade)
    {
        // 1. UI 초기화
        ResetUI();
        nameText.text = name;
        gradeText.text = grade;

        // 2. 슬롯 머신 시작
        slotEffect.PlaySpin(icon);

        // 3. 일정 시간 뒤에 멈춤 명령 + 끝나면 텍스트 연출 실행
        DOVirtual.DelayedCall(spinDuration, () =>
        {
            slotEffect.StopSpin(); // 이제 그만 돌고 결과 내려보내!
        });


        // (위의 RealSlotEffect 코드에 onComplete를 넣었으므로 아래처럼 사용)
        slotEffect.PlaySpin(icon, () =>
        {
            // 슬롯이 탁! 멈춘 직후 실행됨
            ExpandUI();
        });

    }

    private void ExpandUI()
    {
        Sequence seq = DOTween.Sequence();

        // Grade 텍스트 위로 이동
        seq.Append(gradeTextRect.DOAnchorPosY(initGradePos.y + 150f, 0.5f).SetEase(Ease.OutBack));

        // 설명창 페이드 인 + 살짝 올라오기
        seq.Join(infoCanvasGroup.DOFade(1f, 0.5f));
        seq.Join(infoCanvasGroup.transform.DOLocalMoveY(infoCanvasGroup.transform.localPosition.y + 30f, 0.5f).From(true));
    }

    private void ResetUI()
    {
        gradeTextRect.anchoredPosition = initGradePos;
        infoCanvasGroup.alpha = 0f;
        // 필요한 경우 infoCanvasGroup 위치도 리셋
    }
}