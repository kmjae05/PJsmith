using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGetItemLogText : MonoBehaviour {


    private void Start()
    {
        StartCoroutine(dest());
    }

    IEnumerator dest()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

}
