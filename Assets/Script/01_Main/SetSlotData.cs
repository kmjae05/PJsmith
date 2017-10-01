using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSlotData : MonoBehaviour {

    public static SetSlotData instance = null;

    static private List<SetSlot> setSlot;

    static private int repreSet = 1;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        setSlot = new List<SetSlot>();
        //생성
        setSlot.Add(new SetSlot(1));
        setSlot.Add(new SetSlot(2));

    }

    public void setSetSlot(List<SetSlot> setslot) { setSlot = setslot; }
    public List<SetSlot> getSetSlot() { return setSlot; }

    public void setRepreSet(int num) { repreSet = num; }
    public int getRepreSet() { return repreSet; }
}


public class SetSlot
{
    private int setNum;
    public string[] setChrName = null; //세트에 저장된 캐릭터 이름

    public SetSlot()
    {
        setChrName = new string[4];
    }
    public SetSlot(int num)
    {
        this.setNum = num; setChrName = new string[4];
    }

    public int getSetNum() { return setNum; }

}