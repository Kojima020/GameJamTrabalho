using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] float remaingTime;

    // Update is called once per frame
    void Update()
    {
        if (remaingTime < 0.0f) {
            Application.Quit();
        }

        remaingTime -= Time.deltaTime;
        timerText.text = remaingTime.ToString();

        int minutes = Mathf.FloorToInt(remaingTime / 60);
        int seconds = Mathf.FloorToInt(remaingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
