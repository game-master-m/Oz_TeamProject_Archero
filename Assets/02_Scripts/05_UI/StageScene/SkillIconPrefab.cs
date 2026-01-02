using UnityEngine;
using UnityEngine.UI;

public class SkillIconPrefab : MonoBehaviour
{
    [SerializeField] private Image mFrameImage;
    [SerializeField] private Image mIconImage;

    public Image FrameImage => mFrameImage;
    public Image IconImage => mIconImage;
}
