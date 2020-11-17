using UnityEngine;
using TMPro;

public class TextFadeAway : MonoBehaviour
{
    private float duration = 1;
    private float startingTime;
    
    private TextMeshProUGUI text;

    public float Duration {set => duration = value; }

    private void Start()
    {
        startingTime = Time.time;

        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Time.time - startingTime > duration)
        {
            text.alpha = 1f;
            ObjectPool.ReturnPooledObject(ObjectPoolEnum.NodeText, gameObject);
            Destroy(this);
            return;
        }

        float ratio = (Time.time - startingTime) / duration;
        text.alpha = Mathf.Lerp(1, 0, ratio);
    }
}