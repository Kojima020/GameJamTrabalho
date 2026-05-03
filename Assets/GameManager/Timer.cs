using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stopwatch;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
        stopwatch.text = time.ToString("F1");
    }
}
