using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject Ball;
    public GameObject Light;

    public void Create_Defects()
    {
        Instantiate(Ball, new Vector3(Random.Range(-670, 670), 400, Random.Range(-290, 290)), Quaternion.identity);
        Instantiate(Ball, new Vector3(Random.Range(-670, 670), 400, Random.Range(-290, 290)), Quaternion.identity);
        Instantiate(Light, new Vector3(Random.Range(-800, 800), 500, Random.Range(-400, 400)), Quaternion.identity);
    }
}
