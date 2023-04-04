using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Capture : MonoBehaviour
{

    public Camera[] cameras; // 0: depth, 1: reference, 2: query

    private string savePath = "D:/Data/Codes/UnitySyntheticData/Outs/";
    private string referenceDepthPath = "reference_depth/";
    private string referenceRGBPath = "reference_rgb/";
    private string queryRGBPath = "query_rgb/";
    private string referenceLabel = "reference_label/";
    private string imageFormat = ".png";

    private int width = 1280;
    private int height = 960;

    void Start()
    {
        Invoke("CaptureImages", 5.0f);
    }

    void Update()
    {
    }

    public void CaptureImages()
    {
        string timestamp = System.DateTime.Now.ToString("mmssfff");

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

            if (i==0)
            {
                string filename = savePath + referenceDepthPath + timestamp + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==1)
            {
                string filename = savePath + referenceRGBPath + timestamp + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==2)
            {
                string filename = savePath + queryRGBPath + timestamp + imageFormat;
                File.WriteAllBytes(filename, bytes);
            }
            else
            {
                Debug.Log("Error: Camera index out of range");
            }
        }

        CaptureLabels(timestamp);
    }

    public void CaptureLabels(string timestamp)
    {
        GameObject[] DefectsCube = GameObject.FindGameObjectsWithTag("DefectsCube");
    }
}
