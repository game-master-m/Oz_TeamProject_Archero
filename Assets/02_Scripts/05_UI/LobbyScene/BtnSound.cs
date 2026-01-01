using UnityEngine;
using UnityEngine.UI;

public class BtnSound : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(Button);
        }
    }
    private void Button()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.BtnSound();
        }
    }
}
