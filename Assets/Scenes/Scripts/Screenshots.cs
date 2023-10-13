using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshots : MonoBehaviour
{
    //public int FileCounter = 0;
    [SerializeField]
    Camera cam;

    public string file;
    public RenderTexture rt;


    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveScreenshots();
            Debug.Log("Texture was saved from Render Texture.");
        }
    }

    //https://gamedev.stackexchange.com/questions/184785/saving-png-from-render-texture-results-in-much-darker-image
    public void SaveScreenshots()
    {
        RenderTexture mRt = new RenderTexture(rt.width, rt.height, rt.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = rt.antiAliasing;

        var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
        cam.targetTexture = mRt;
        cam.Render();
        RenderTexture.active = mRt;

        tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
        tex.Apply();

        var path = file;
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        Debug.Log("Saved file to: " + path);

        DestroyImmediate(tex);

        cam.targetTexture = rt;
        cam.Render();
        cam.targetTexture = null; //get the camera's rendertexture without disable its renderring to the screen https://forum.unity.com/threads/anyway-to-get-the-cameras-rendertexture-without-disable-its-renderring-to-the-screen.520391/
        RenderTexture.active = rt;

        DestroyImmediate(mRt);
    }

    //void CamCapture()
    //{
    //    Camera Cam = GetComponent<Camera>();
    //    Debug.Log(Cam);

    //    RenderTexture currentRT = RenderTexture.active;
    //    RenderTexture.active = Cam.targetTexture;

    //    Cam.Render();
    //    Debug.Log(Cam.targetTexture);

    //    Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
    //    Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
    //    Image.Apply();
    //    RenderTexture.active = currentRT;

    //    var Bytes = Image.EncodeToPNG();
    //    Destroy(Image);

    //    File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + FileCounter + ".png", Bytes);
    //    FileCounter++;
    //}

}
