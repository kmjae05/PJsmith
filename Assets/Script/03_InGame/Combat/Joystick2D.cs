using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Joystick2D : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image bgImg;
    private Image joystickImg;
    private Vector2 inputVector;

    //캐릭터 전투 스크립트
    private Combat combat;

    //조이스틱 누른 상태
    private bool joystick = false;
    //조이스틱 방향
    private bool front = true;

    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        combat = GameObject.Find("Chr_001_").GetComponent<Combat>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bgImg.rectTransform
            , ped.position
            , ped.pressEventCamera,
            out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);

            //pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0);// pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //Move Joystick Img
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 2.5f),
                                                              0);// inputVector.y * (bgImg.rectTransform.sizeDelta.y / 2.5f));
            if (joystickImg.rectTransform.anchoredPosition.x > 0)
                front = true;
            else front = false;

        }
    }

    //눌렀을 때
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
        joystick = true;
    }

    //손을 뗐을 때
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        joystick = false;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.y != 0)
            return inputVector.y;
        else
            return Input.GetAxis("Vertical");
    }

    public bool getJoystick()
    {
        return joystick;
    }
    public void setFalseJoystick()
    {
        joystick = false;
    }

    public bool getFront()
    {
        return front;
    }
}

