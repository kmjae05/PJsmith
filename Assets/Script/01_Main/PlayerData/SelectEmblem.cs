using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectEmblem : MonoBehaviour, IPointerDownHandler{
    public int emblem_id;
    public void OnPointerDown(PointerEventData data)
    {
        EmblemHandle.selected_Emblem = emblem_id;
    }
}
