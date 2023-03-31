using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Capture : MonoBehaviour
{
    public Camera[] cameras;
    public string savePath = "D:/Data/Codes/UnitySyntheticData/Outs/";

    private int width = 1280; // change this to match your desired capture resolution
    private int height = 960; // change this to match your desired capture resolution

    public void CaptureImage()
    {
        string timestamp = System.DateTime.Now.ToString("HHmmssfff");
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
                string filename = savePath + "reference_depth/" + timestamp + ".png";
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==1)
            {
                string filename = savePath + "reference_rgb/" + timestamp + ".png";
                File.WriteAllBytes(filename, bytes);
            }
            else if (i==2)
            {
                string filename = savePath + "queue_rgb/" + timestamp + ".png";
                File.WriteAllBytes(filename, bytes);
            }
            else
            {
                Debug.Log("Error: Camera index out of range");
            }

            Debug.Log("Done !");
        }
    }
}
