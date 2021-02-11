using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using System;

public class FoodCollectorSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] Agents;
    [HideInInspector]
    public FoodCollectorArea[] AreaList;
    [HideInInspector]
    public float TotalScore;

    public Text ScoreText;

    StatsRecorder Recorder;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        Recorder = Academy.Instance.StatsRecorder;
    }

    private void Start()
    {
        AreaList = FindObjectsOfType<FoodCollectorArea>(); 
    }

    void EnvironmentReset()
    {
        //ClearObjects(GameObject.FindGameObjectsWithTag("food"));
        //Agents = GameObject.FindGameObjectsWithTag("agent");
        print("Reset!");

        foreach (var area in AreaList)
        {
            area.Reset();
        }

        TotalScore = 0;
    }

    public void Update()
    {
        if (Time.frameCount % 30 == 0 && ScoreText)
            ScoreText.text = $"Score: {Math.Round(TotalScore, 3)}";

        // Send stats via SideChannel so that they'll appear in TensorBoard.
        // These values get averaged every summary_frequency steps, so we don't
        // need to send every Update() call.
        if (Time.frameCount % 100 == 0)
        {
            Recorder.Add("TotalScore", TotalScore);
        }
    }
}
