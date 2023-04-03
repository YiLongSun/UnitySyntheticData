using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject inductor_base;
    public GameObject alumni_base;
    public GameObject alumni_cube;
    public GameObject alumni_top;
    public GameObject prefab_light;

    private int alumni_base_count = 24;
    private int alumni_delta_x = 30;
    private int alumni_delta_z = -142;
    private float[] alumnni_cube_position_x = {-697.5f,-622.5f,622.5f,697.5f};
    private float[] alumnni_cube_position_z = {-375f,-275f,275f,375f};

    void Start()
    {
        CreateInductor();
    }

    void Update()
    {

    }

    public void CreateInductor()
    {
        Instantiate(prefab_light, new Vector3(Random.Range(-800, 800), 500, Random.Range(-400, 400)), Quaternion.identity);
        Instantiate(prefab_light, new Vector3(Random.Range(-800, 800), 500, Random.Range(-400, 400)), Quaternion.identity);
        Instantiate(inductor_base, new Vector3(0, 0, 0), Quaternion.identity);
        for (int pos_x=0;pos_x<alumnni_cube_position_x.Length;pos_x++)
        {
            for (int pos_z=0;pos_z<alumnni_cube_position_z.Length;pos_z++)
            {
                int randomInt = Random.Range(1, 101);
                if (randomInt > 95)
                {
                    Vector3 temp_pos = new Vector3(alumnni_cube_position_x[pos_x], 270, alumnni_cube_position_z[pos_z] + Random.Range(0, 25));
                    Quaternion temp_rot = Quaternion.Euler(Random.Range(-45, 45),Random.Range(-45, 45),Random.Range(-45, 45));
                    GameObject cube = Instantiate(alumni_cube, temp_pos, temp_rot);
                    cube.tag = "Defects";
                    // Debug.Log("ALumni Cube Position : " + temp_pos + "   ALumni Cube Rotation : " + temp_rot.eulerAngles);
                }
                else
                {
                    Instantiate(alumni_cube, new Vector3(alumnni_cube_position_x[pos_x], 270, alumnni_cube_position_z[pos_z]), Quaternion.identity);
                }
            }
        }
        for (int count=0;count<alumni_base_count;count++)
        {
            Instantiate(alumni_base, new Vector3(0+alumni_delta_x*count,0,0), Quaternion.identity);
            for (int slice=0;slice<5;slice++)
            {
                int randomInt = Random.Range(1, 1001);
                if (randomInt > 996)
                {
                    Vector3 temp_pos = new Vector3(-467.812f+alumni_delta_x*count, 235, 284+alumni_delta_z*slice + Random.Range(0, 15));
                    Quaternion temp_rot = Quaternion.Euler(Random.Range(60, 120),0,0);
                    GameObject top = Instantiate(alumni_top, temp_pos, temp_rot);
                    top.tag = "Defects";
                    // Debug.Log("ALumni Top Position : " + temp_pos + "   ALumni Top Rotation : " + temp_rot.eulerAngles);
                }
                else
                {
                    Instantiate(alumni_top, new Vector3(-467.812f+alumni_delta_x*count, 235, 284+alumni_delta_z*slice), Quaternion.Euler(90,0,0));
                }
            }
        }
    }
}
