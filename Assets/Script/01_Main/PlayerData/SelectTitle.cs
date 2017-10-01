using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectTitle : MonoBehaviour, IPointerDownHandler
{
    public int title_id;
    public void OnPointerDown(PointerEventData data)
    {
        TitleHandler.selected_title = title_id;
    }
}
