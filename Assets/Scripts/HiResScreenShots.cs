using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 320;
    public int resHeight = 240;

    private bool takeHiResShot = false;
    private Camera cam;
    public Image image;
    public Image flash;


    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            image.enabled = false;
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            cam.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            ScanBird();
            StartCoroutine("Flash");
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
            image.enabled = true;

        }
    }

    void ScanBird()
    {

    }

    IEnumerator Flash()
    {
        flash.enabled = true;
        yield return new WaitForSeconds(0.2f);
        flash.enabled = false;
    }
}