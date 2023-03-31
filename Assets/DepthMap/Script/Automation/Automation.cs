using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Func_1", 3.0f);
        Invoke("Func_2", 9.0f);
        Invoke("Func_3", 15.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Func_1()
    {
        this.GetComponent<Creator>().Create_Defects();
    }

    void Func_2()
    {
        this.GetComponent<Capture>().CaptureImage();
    }

    void Func_3()
    {
        this.GetComponent<Reset>().ResetEnvironment();
    }
}
