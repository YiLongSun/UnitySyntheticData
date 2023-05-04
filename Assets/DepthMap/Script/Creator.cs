using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject prefab_inductor_base;
    public GameObject prefab_alumni_base;
    public GameObject prefab_alumni_cube;
    public GameObject prefab_alumni_top;
    public GameObject prefab_light;

    void Start()
    {
        CreateDefects();
    }

    void Update()
    {

    }

    public void CreateDefects()
    {
        // int total_rot = Random.Range(0, 360);
        int total_rot = 0;

        int alumni_base_num = 24;

        int alumni_base_delta_x = 30;
        int alumni_top_delta_z = -142;

        float[] alumnni_cube_position_x = {-697.5f,-622.5f,622.5f,697.5f};
        float[] alumnni_cube_position_z = {-375f,-275f,275f,375f};

        // Create lights
        Instantiate(prefab_light, new Vector3(Random.Range(-800, 800), 500, Random.Range(-400, 400)), Quaternion.identity);
        Instantiate(prefab_light, new Vector3(Random.Range(-800, 800), 500, Random.Range(-400, 400)), Quaternion.identity);

        // Create inductor base
        GameObject inductor_base = Instantiate(prefab_inductor_base, new Vector3(0, 0, 0), Quaternion.Euler(0, total_rot, 0));

        // Create alumni cube
        for (int pos_x=0;pos_x<alumnni_cube_position_x.Length;pos_x++)
        {
            for (int pos_z=0;pos_z<alumnni_cube_position_z.Length;pos_z++)
            {
                if (Random.Range(1, 101)>95)
                {
                    Vector3 alumni_cube_pos = new Vector3(alumnni_cube_position_x[pos_x], 270, alumnni_cube_position_z[pos_z] + Random.Range(0, 25));
                    Quaternion alumni_cube_rot = Quaternion.Euler(Random.Range(-45, 45),Random.Range(-45, 45),Random.Range(-45, 45));
                    GameObject alumni_cube=Instantiate(prefab_alumni_cube, alumni_cube_pos, alumni_cube_rot);
                    alumni_cube.transform.RotateAround(inductor_base.transform.position, Vector3.up, total_rot);
                    alumni_cube.tag = "DefectsCube";
                }
                else
                {
                    Vector3 alumni_cube_pos = new Vector3(alumnni_cube_position_x[pos_x], 270, alumnni_cube_position_z[pos_z]);
                    GameObject alumni_cube=Instantiate(prefab_alumni_cube, alumni_cube_pos, Quaternion.Euler(0, 0, 0));
                    alumni_cube.transform.RotateAround(inductor_base.transform.position, Vector3.up, total_rot);
                }
            }
        }

        // Create alumni base and alumni top
        for (int count=0;count<alumni_base_num;count++)
        {
            Vector3 alumni_base_pos = new Vector3(0+alumni_base_delta_x*count, 0, 0);
            GameObject alumni_base = Instantiate(prefab_alumni_base, alumni_base_pos, Quaternion.identity);
            alumni_base.transform.RotateAround(inductor_base.transform.position, Vector3.up, total_rot);

            for (int slice=0;slice<5;slice++)
            {
                if (Random.Range(1,1001)>996)
                {
                    Vector3 alumni_top_pos = new Vector3(-467.812f+alumni_base_delta_x*count, 235, 284+alumni_top_delta_z*slice + Random.Range(0, 15));
                    Quaternion alumni_top_rot = Quaternion.Euler(Random.Range(60, 120),0,0);
                    GameObject alumni_top = Instantiate(prefab_alumni_top, alumni_top_pos, alumni_top_rot);
                    alumni_top.transform.RotateAround(inductor_base.transform.position, Vector3.up, total_rot);
                    alumni_top.tag = "DefectsTop";
                }
                else
                {
                    Vector3 alumni_top_pos = new Vector3(-467.812f+alumni_base_delta_x*count, 235, 284+alumni_top_delta_z*slice);
                    Quaternion alumni_top_rot = Quaternion.Euler(90,0,0);
                    GameObject alumni_top = Instantiate(prefab_alumni_top, alumni_top_pos, alumni_top_rot);
                    alumni_top.transform.RotateAround(inductor_base.transform.position, Vector3.up, total_rot);
                }
            }
        }
    }
}
