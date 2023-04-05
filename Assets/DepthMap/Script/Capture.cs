using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Capture : MonoBehaviour
{

    public Camera[] cameras; // 0: depth, 1: reference, 2: query

    private string savePath = "D:/Data/Codes/UnitySyntheticData/Outs/";
    private string referenceDepthPath = "reference_depth/";
    private string referenceRGBPath = "reference_rgb/";
    private string queryRGBPath = "query_rgb/";
    private string referenceLabelPath = "reference_label/";
    private string imageFormat = ".png";

    private int width = 1280;
    private int height = 960;

    void Start()
    {
        Invoke("CaptureImages", 5.0f);
        Invoke("ResetScene", 8.0f);
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
        GameObject[] DefectsTop = GameObject.FindGameObjectsWithTag("DefectsTop");
        Vector3[] DefectsTopPixelPos = GetObjectPosition(DefectsTop, "DefectsTop");
        Vector3[] DefectsCubePixelPos = GetObjectPosition(DefectsCube, "DefectsCube");

        // Save as txt file
        string filename = savePath + referenceLabelPath + timestamp + ".json";
        DefectsData data = new DefectsData();
        data.DefectsTopPixelPos = DefectsTopPixelPos;
        data.DefectsCubePixelPos = DefectsCubePixelPos;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filename, json);
    }

    public Vector3[] GetObjectPosition(GameObject[] objects, string category)
    {
        Vector3[] screenpositions = new Vector3[objects.Length*8];

        if (category == "DefectsCube")
        {
            for(int i=0;i<objects.Length;i++)
            {
                Vector3[] edges = new Vector3[8];
                edges[0] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, -objects[i].transform.localScale.y / 2, -objects[i].transform.localScale.z / 2);
                edges[1] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, -objects[i].transform.localScale.y / 2, objects[i].transform.localScale.z / 2);
                edges[2] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, objects[i].transform.localScale.y / 2, -objects[i].transform.localScale.z / 2);
                edges[3] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, objects[i].transform.localScale.y / 2, objects[i].transform.localScale.z / 2);
                edges[4] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, -objects[i].transform.localScale.y / 2, -objects[i].transform.localScale.z / 2);
                edges[5] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, -objects[i].transform.localScale.y / 2, objects[i].transform.localScale.z / 2);
                edges[6] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, objects[i].transform.localScale.y / 2, -objects[i].transform.localScale.z / 2);
                edges[7] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, objects[i].transform.localScale.y / 2, objects[i].transform.localScale.z / 2);

                for (int j = 0; j < edges.Length; j++)
                {
                    edges[j] = RotatePointAroundPivot(edges[j], objects[i].transform.position, objects[i].transform.eulerAngles);
                    screenpositions[i*8+j] = cameras[1].WorldToScreenPoint(edges[j]);
                    Debug.Log(screenpositions[i * 8 + j]);
                }
            }
        }
        else if (category == "DefectsTop")
        {
            for(int i=0;i<objects.Length;i++)
            {
                Vector3[] edges = new Vector3[8];
                edges[0] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, -(objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), -objects[i].transform.localScale.z / 2);
                edges[1] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, -(objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), objects[i].transform.localScale.z / 2);
                edges[2] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, (objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), -objects[i].transform.localScale.z / 2);
                edges[3] = objects[i].transform.position + new Vector3(-objects[i].transform.localScale.x / 2, (objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), objects[i].transform.localScale.z / 2);
                edges[4] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, -(objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), -objects[i].transform.localScale.z / 2);
                edges[5] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, -(objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), objects[i].transform.localScale.z / 2);
                edges[6] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, (objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), -objects[i].transform.localScale.z / 2);
                edges[7] = objects[i].transform.position + new Vector3(objects[i].transform.localScale.x / 2, (objects[i].transform.localScale.y / 2+objects[i].transform.localScale.z*1.2f), objects[i].transform.localScale.z / 2);

                for (int j = 0; j < edges.Length; j++)
                {
                    edges[j] = RotatePointAroundPivot(edges[j], objects[i].transform.position, objects[i].transform.eulerAngles);
                    screenpositions[i*8+j] = cameras[1].WorldToScreenPoint(edges[j]);
                    Debug.Log(screenpositions[i * 8 + j]);
                }
            }
        }
        else
        {;}

        return screenpositions;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    [System.Serializable]
    public class DefectsData
    {
        public Vector3[] DefectsTopPixelPos;
        public Vector3[] DefectsCubePixelPos;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}
