using UnityEngine;
using UnityEngine.EventSystems;

public class IPointerText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //밑에있는 성배와 이벤트칸 마우스 올렸을때 텍스트표시
    public GameObject mPointerText;

    void Start()
    {
        mPointerText.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mPointerText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mPointerText.SetActive(false);
    }

}
