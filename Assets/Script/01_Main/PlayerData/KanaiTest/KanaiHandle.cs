using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KanaiHandle : MonoBehaviour {
    public GameObject KanaiSlotPanel;

    public KanaiSlot[] kanaiArray;

    public List<GameObject> KanaiList = new List<GameObject>();

    public class Combine
    {
        public int blade_number;        //검신 번호
        public int meterial_number;     //재료 번호
        public bool special_meterial;   //특수 재료 필요 여부
        public int special_meterial_number; //특수 재료 번호
        public int result_type;     //결과 컬렉션 종류
        public int result_number;   //결과 컬렉션 번호

        public Combine(int blade_number, int meterial_number, bool special_meterial, int special_meterial_number, int result_type, int result_number)
        {
            this.blade_number = blade_number;
            this.meterial_number = meterial_number;
            this.special_meterial = special_meterial;
            this.special_meterial_number = special_meterial_number;
            this.result_type = result_type;
            this.result_number = result_number;
        }
        public Combine()
        {

        }
    }
    List<Combine> combineList = new List<Combine>();

    public void CombineItems()
    {
        //초기화
        kanaiArray = KanaiSlotPanel.GetComponentsInChildren<KanaiSlot>();
        int bladeNumber = -1;       //사용한 검신 번호
        int meterial = -1;          //사용한 일반 재료
        int specialMeterial = -1;   //사용한 특수 재료
        List<Combine> combines = new List<Combine>();   //검신에 해당하는 조합식
        Combine Result = new Combine();                 //결과 저장
        bool combine_Success = false;

        //테스트 용 조합식
        combineList.Add(new Combine(0, 0, false, -1, 0, 0));    //일반 합성1
        combineList.Add(new Combine(0, 1, false, -1, 0, 1));    //일반 합성2   
        combineList.Add(new Combine(0, 2, false, -1, 0, 2));    //일반 합성3
        combineList.Add(new Combine(0, 0, true, 1, 0, 3));      //특수 합성1


        //검신 찾기
        for (int i = 0; i < kanaiArray.Length; i++) 
        {
            if (kanaiArray[i].Item_type == 0)   //0번은 검신, 1번은 일반재료, 2번은 특수재료
            {
                bladeNumber = kanaiArray[i].Item_number;    //검신을 찾아서 번호 저장
            }
            else if(kanaiArray[i].Item_type == 1)
            {
                meterial = kanaiArray[i].Item_number;       //일반 재료 저장
            }
            else if (kanaiArray[i].Item_type == 2)
            {
                specialMeterial = kanaiArray[i].Item_number;
            }
        }

        //검신에 해당하는 조합식들 검색
        for (int i = 0; i < combineList.Count; i++)
        {
            if (combineList[i].blade_number == bladeNumber)
            {
                combines.Add(combineList[i]);   //조합식 저장
            }
        }

        //저장된 조합식 중 부합하는 조합식 검색
        for (int i = 0; i < combines.Count; i++)
        {
            if (combines[i].meterial_number == meterial)
            {
                if (!combines[i].special_meterial)  //일반 조합식일 때
                {
                    Result = combines[i];
                }
                else if(combines[i].special_meterial)    //특수 조합식일 때
                {
                    if (combines[i].special_meterial_number == specialMeterial)
                    {
                        Result = combines[i];
                    }
                }
                combine_Success = true;
            }
        }

        //결과
        if (combine_Success)
        {
            //합성 성공
            Debug.Log("합성 성공");
            Debug.Log("결과 : " + Result.result_type + "," + Result.result_number);
        }
        else
        {
            //합성 실패
            Debug.Log("합성 실패");
        }
    }
}
