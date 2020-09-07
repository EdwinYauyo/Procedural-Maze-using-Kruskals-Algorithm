using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    private GeneratingFrame generatingFrame;
    private CameraController cameraControl;
    public Text Width;
    public Text Height;
    public Text MazeScale;
    public Text DelayTime;
    public Slider SliderWidth;
    public Slider SliderHeight;
    public Slider SliderMazeScale;
    public Slider SliderDelayTime;

    // Start is called before the first frame update
    private void Awake()
    {
        generatingFrame = FindObjectOfType<GeneratingFrame>();
        cameraControl = FindObjectOfType<CameraController>();
        Time.timeScale = 0;
    }
    void Start()
    {
        


        
    }

    // Update is called once per frame
    void Update()
    {
        Width.text = "" + (int)SliderWidth.value+"m";
        Height.text = "" + (int)SliderHeight.value+"m";
        //MazeScale.text = "x" + SliderMazeScale.value;
        DelayTime.text = "" + (float)Math.Round(SliderDelayTime.value, 2) + "s";
    }


    void SetupValues()
    {
        generatingFrame.width= (int)SliderWidth.value;
        generatingFrame.height = (int)SliderHeight.value;
        generatingFrame.scaleMaze = 1;
        generatingFrame.delayTime =(float)Math.Round(SliderDelayTime.value,2);
    }
    void DeleteSliders()
    {
        SliderWidth.interactable = false;
        SliderHeight.interactable = false;
        SliderMazeScale.interactable = false;
        SliderDelayTime.interactable = false;
    }

    public void StartGenerate()
    {
        Time.timeScale = 1f;
        SetupValues();
        DeleteSliders();
        cameraControl.SetupCamera();
        generatingFrame.active = true;
        generatingFrame.Execution(generatingFrame.delayTime);
    }

    public void Play()
    {
        Time.timeScale = 1f;
        generatingFrame.active = true;

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        generatingFrame.active = false;
    }

    public void Step()
    {
        if(generatingFrame.edges.Count>0)generatingFrame.RemoveEdges();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Kruskal");
    }

}
