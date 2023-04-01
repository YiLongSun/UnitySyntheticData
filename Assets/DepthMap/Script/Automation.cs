using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Automation : MonoBehaviour
{
    public Camera[] cameras; // 0: depth, 1: reference, 2: query

    public GameObject prefab_object;
    public GameObject prefab_ball;
    public GameObject prefab_light;

    private string savePath = "D:/Data/Codes/UnitySyntheticData/Outs/";
    private string referenceDepthPath = "reference_depth/";
    private string referenceRGBPath = "reference_rgb/";
    private string queryRGBPath = "query_rgb/";
    private string referenceDepthPathOrigin = "reference_depth_origin/";
    private string referenceRGBPathOrigin = "reference_rgb_origin/";
    private string queryRGBPathOrigin = "query_rgb_origin/";
    private string imageFormat = ".png";

    private int count = 0;

    private int width = 1280;
    private int height = 960;

    void Start()
    {
        StartCoroutine(Run());
    }

    public void Capture()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            // create a new render texture
            RenderTexture rt = new RenderTexture(width, height, 24);
            // set the camera's target texture to the render texture
            cameras[i].targetTexture = rt;
            // render the camera to the texture
            cameras[i].Render();
            // activate the render texture so it can be read from
            RenderTexture.active = rt;

            // create a new texture with the same dimensions as the render texture
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            // read the pixels from the render texture into the texture
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            // apply the pixels to the texture
            texture.Apply();

            // reset the camera's target texture
            cameras[i].targetTexture = null;
            // deactivate the render texture
            RenderTexture.active = null;
            // destroy the render texture
            Destroy(rt);

            // save the texture as a PNG file
            byte[] bytes = texture.EncodeToPNG();
            string count_string = count.ToString();

            if (i==0)
            {
                string filename = savePath + referenceDepthPath + count_string + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==1)
            {
                string filename = savePath + referenceRGBPath + count_string + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==2)
            {
                string filename = savePath + queryRGBPath + count_string + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else
            {
                Debug.Log("Error: Camera index out of range");
            }
        }
    }

    public void CaptureOrigin()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            // create a new render texture
            RenderTexture rt = new RenderTexture(width, height, 24);
            // set the camera's target texture to the render texture
            cameras[i].targetTexture = rt;
            // render the camera to the texture
            cameras[i].Render();
            // activate the render texture so it can be read from
            RenderTexture.active = rt;

            // create a new texture with the same dimensions as the render texture
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            // read the pixels from the render texture into the texture
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            // apply the pixels to the texture
            texture.Apply();

            // reset the camera's target texture
            cameras[i].targetTexture = null;
            // deactivate the render texture
            RenderTexture.active = null;
            // destroy the render texture
            Destroy(rt);

            // save the texture as a PNG file
            byte[] bytes = texture.EncodeToPNG();
            string count_string = count.ToString();

            if (i==0)
            {
                string filename = savePath + referenceDepthPathOrigin + count_string + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==1)
            {
                string filename = savePath + referenceRGBPathOrigin + count_string + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==2)
            {
                string filename = savePath + queryRGBPathOrigin + count_string + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else
            {
                Debug.Log("Error: Camera index out of range");
            }
        }
    }

    public void CreateDefects()
    {
        Instantiate(prefab_object, new Vector3(0, 5, 0), Quaternion.Euler(0, Random.Range(0, 360), 0));
        Instantiate(prefab_ball, new Vector3(Random.Range(-670, 670), 400, Random.Range(-290, 290)), Quaternion.identity);
        Instantiate(prefab_light, new Vector3(Random.Range(-800, 800), 500, Random.Range(-400, 400)), Quaternion.identity);
    }

    public void RemoveDefects()
    {
        Destroy(GameObject.Find("Object_Inductor(Clone)"));
        Destroy(GameObject.Find("Ball(Clone)"));
        Destroy(GameObject.Find("Light(Clone)"));
    }

    IEnumerator Run()
    {
        while (true)
        {
            count += 1;
            Invoke("CreateDefects", 0);
            Invoke("CaptureOrigin", 1);
            Invoke("Capture", 6);
            Invoke("RemoveDefects", 7);
            yield return new WaitForSeconds(10);
        }
    }
}
