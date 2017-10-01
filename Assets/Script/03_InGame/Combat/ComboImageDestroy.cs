using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboImageDestroy : MonoBehaviour {

    Animator anim;

	void Start () {
        anim = transform.GetComponent<Animator>();
        StartCoroutine(destroy());

    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(2.0f);
        anim.Play("ComboFadeOut");
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }


}
