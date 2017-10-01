using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
    public Slider Progress;
	// Use this for initialization
	void Start () {
        StartCoroutine(Load());
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public IEnumerator Load()
    {
        var load = SceneManager.LoadSceneAsync("3DGame");

        while (!load.isDone)
        {
            Progress.value = load.progress*100;
            Debug.Log(load.progress*100);
            yield return null;
        }

        Debug.Log("Done!");
    }
}
