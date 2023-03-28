using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateDefects : MonoBehaviour
{
    public GameObject Ball;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Instantiate()
    {
        int x = Random.Range(-440, 440);
        int z = Random.Range(-190, 190);
        Instantiate(Ball, new Vector3(x, -350, z), Quaternion.identity);
    }
}
