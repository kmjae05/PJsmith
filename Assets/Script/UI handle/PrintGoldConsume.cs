using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrintGoldConsume : MonoBehaviour
{
    public GameObject Gold;
    public GameObject AnimationCanvas;
    public float gold_Horizontal_Position;
    public float gold_Vertical_Position;
    public float gold_FadeOutRate;
    public int gold_FontSize;
    public float gold_Y_axis_velocity;
    public float accelate;

    public IEnumerator goldAnimation(GameObject gold)
    {
        float d_velocity = gold_Y_axis_velocity;
        float d_accelate = accelate;
        gold = Instantiate(Gold);
        gold.GetComponent<RectTransform>().position = new Vector3(gold_Horizontal_Position, gold_Vertical_Position, 0);
        gold.SetActive(true);
        gold.GetComponent<Text>().text = "-"+OreSelect.SelectOre.price.ToString();
        gold.transform.SetParent(AnimationCanvas.transform, false);
        float k = 1.0f;
        while (d_velocity >= 0)
        {
            gold.GetComponent<RectTransform>().position += new Vector3(0, d_velocity, 0);
            gold.GetComponent<Text>().fontSize = gold_FontSize;
            gold.GetComponent<Text>().color = new Vector4(255, 255, 255, k);
            k -= gold_FadeOutRate;
            d_velocity += d_accelate;
            yield return null;
        }
        Destroy(gold);
    }
}
