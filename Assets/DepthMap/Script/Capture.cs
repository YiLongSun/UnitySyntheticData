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

        GameObject[] defects = GameObject.FindGameObjectsWithTag("Defects");
        Texture2D maskTexture = new Texture2D(1280, 960);

        foreach (GameObject defect in defects)
        {
            Vector3 defectScale = defect.transform.localScale;
            Vector3[] scales_x = new Vector3[2];
            Vector3[] scales_y = new Vector3[2];
            Vector3[] scales_z = new Vector3[2];
            scales_x[0] = new Vector3(defectScale.x/2, 0, 0);
            scales_x[1] = new Vector3(-defectScale.x/2, 0, 0);
            scales_y[0] = new Vector3(0, defectScale.y/2, 0);
            scales_y[1] = new Vector3(0, -defectScale.y/2, 0);
            scales_z[0] = new Vector3(0, 0, defectScale.z/2);
            scales_z[1] = new Vector3(0, 0, -defectScale.z/2);

            Vector3[] edges = new Vector3[8];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        edges[i*4 + j*2 + k] = new Vector3(defect.transform.position.x + scales_x[i].x, defect.transform.position.y + scales_y[j].y , defect.transform.position.z + scales_z[k].z);
                        Debug.Log(edges[i*4 + j*2 + k]);
                    }
                }
            }

            Vector3[] screenpositions = new Vector3[8];
            for (int l=0; l<8; l++)
            {
                screenpositions[l] = cameras[1].WorldToScreenPoint(edges[l]);
            }




            // Set all pixels to black
            for (int x = 0; x < maskTexture.width; x++)
            {
                for (int y = 0; y < maskTexture.height; y++)
                {
                    maskTexture.SetPixel(x, y, Color.black);
                }
            }

            // Draw white pixels at line points
            foreach (Vector3 screenposition in screenpositions)
            {
                maskTexture.SetPixel((int)screenposition.x, (int)screenposition.y, Color.white);
            }
        }

        byte[] bytes_label = maskTexture.EncodeToPNG();
        string filename_label = savePath + referenceLabel + timestamp + imageFormat;
        File.WriteAllBytes(filename_label, bytes_label);
    }
}
