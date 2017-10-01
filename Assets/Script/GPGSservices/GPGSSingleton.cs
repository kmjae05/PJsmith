using UnityEngine;
using System.Collections;


public class GPGSSingleton : MonoBehaviour
{
    protected static GPGSMng instance = null;
    public static GPGSMng GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new GPGSMng();
            }
            return instance;
        }
    }
}
