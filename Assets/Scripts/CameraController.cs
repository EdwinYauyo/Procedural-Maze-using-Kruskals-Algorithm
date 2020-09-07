using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GeneratingFrame generatingFrame;
    // Start is called before the first frame update
    private float Y;
    private void Awake()
    {
        generatingFrame = FindObjectOfType<GeneratingFrame>();
    }
    void Start()
    {
        

        
    }

    public void SetupCamera()
    {
        float X = generatingFrame.width * 10 * generatingFrame.scaleMaze + 3;
        float Z = generatingFrame.height * 12.5f * generatingFrame.scaleMaze + 3;

        if (generatingFrame.width > generatingFrame.height)
        {
            Y = generatingFrame.width * generatingFrame.scaleMaze * 35 / 3;
        }
        else Y = generatingFrame.height * generatingFrame.scaleMaze * 35 / 3;

        transform.position = new Vector3(X / 2, Y, Z / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
