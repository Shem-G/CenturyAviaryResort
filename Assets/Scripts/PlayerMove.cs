using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 15;
    public float accelValue = 0.5f;
    public float decelValue = 1;
    public float curSpeed;
    public float rotSpeed = 10;
    public float xPos;

    public float gravityMultiplier = 3;

    public Vector3 point;
    Vector2 mousePos;
    Vector3 finalMove;

    public CharacterController cc;
    public float zoom = 20;
    public float normal = 60;
    public float smooth = 5;
    public float scanDist = 15;

    public GameObject bino;
    public bool isZoomed = false;
    public Camera cam;

    public Vector3 rot;

    public Vector2 rotationRange = new Vector3(70, 70);
    public float rotationSpeed = 10;
    public float dampingTime = 0.2f;
    public bool autoZeroVerticalOnMobile = true;
    public bool autoZeroHorizontalOnMobile = false;
    public bool relative = true;


    private Vector3 m_TargetAngles;
    private Vector3 m_FollowAngles;
    private Vector3 m_FollowVelocity;
    private Quaternion m_OriginalRotation;
    private Quaternion orgRot;
    private Quaternion zeroRot;

    public int zoomLevel = 0;

    private bird targetBird;
    public Text birdName;

    private bool takeHiResShot = false;
    public Canvas canvas;
    public Transform flash;
    public int resWidth = 320;
    public int resHeight = 240;

    public RenderTexture gameRender;

    // Use this for initialization
    void Start()
    {
        zeroRot = new Quaternion(0, 0, 0, 0);
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        m_OriginalRotation = transform.localRotation;
        bino.SetActive(false);
        birdName.text = "";
    }

    // Update is called once per frame
    void Update()
    {


        if(Input.GetMouseButton(0))
        {
            moveSpeed = moveSpeed * 4;
        }
        else
        {
            moveSpeed = 15;
        }

        if (Input.GetMouseButtonDown(2))
        {
            switch (zoomLevel)
            {
                case 0:
                    isZoomed = true;
                    zoom = 50;
                    scanDist = 8;
                    zoomLevel++;
                    break;
                case 1:
                    isZoomed = true;
                    zoom = 40;
                    scanDist = 10;
                    zoomLevel++;
                    break;
                case 2:
                    isZoomed = true;
                    zoom = 20;
                    scanDist = 20;
                    zoomLevel++;
                    break;
                case 3:
                    isZoomed = true;
                    zoom = 10;
                    scanDist = 35;
                    zoomLevel++;
                    break;
                case 4:
                    isZoomed = false;
                    zoom = normal;
                    scanDist = 8;
                    zoomLevel = 0;
                    break;
            }
           
        }

        if (isZoomed)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * smooth);
            bino.SetActive(true);
            MouseLook();
            DetectBird();
        }

        else
        {
            //birdName.text = "";
            m_TargetAngles = Vector3.zero;
            m_FollowAngles = Vector3.zero;
            cam.transform.localRotation = zeroRot;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normal, Time.deltaTime * smooth);
            bino.SetActive(false);
            birdName.text = "";
            Move();
        }
        Debug.DrawRay(cam.transform.position, cam.transform.forward * scanDist, Color.red);
    }

    void DetectBird()
    {
        Vector3 fwd = cam.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, scanDist))
        {
            if (hit.collider.tag == "Bird")
            {
                Debug.Log("Bird");
                float dist = Vector3.Distance(cam.transform.position, hit.transform.position);
                Debug.Log("Distance: " + dist);
                targetBird = hit.transform.GetComponent<bird>();
                birdName.text = targetBird.BirdData.BirdName;
            }
            else
            {
                Debug.Log("Not bird");
                birdName.text = "";
            }
        }
        else
        {
            birdName.text = "";
        }

    }

    void MouseLook()
    {
        // we make initial calculations from the original local rotation
        
        cam.transform.localRotation = m_OriginalRotation;

        // read input from mouse or mobile controls
        float inputH;
        float inputV;
        if (relative)
        {
            inputH = CrossPlatformInputManager.GetAxis("Mouse X");
            inputV = CrossPlatformInputManager.GetAxis("Mouse Y");

            // wrap values to avoid springing quickly the wrong way from positive to negative
            if (m_TargetAngles.y > 180)
            {
                m_TargetAngles.y -= 360;
                m_FollowAngles.y -= 360;
            }
            if (m_TargetAngles.x > 180)
            {
                m_TargetAngles.x -= 360;
                m_FollowAngles.x -= 360;
            }
            if (m_TargetAngles.y < -180)
            {
                m_TargetAngles.y += 360;
                m_FollowAngles.y += 360;
            }
            if (m_TargetAngles.x < -180)
            {
                m_TargetAngles.x += 360;
                m_FollowAngles.x += 360;
            }

#if MOBILE_INPUT
            // on mobile, sometimes we want input mapped directly to tilt value,
            // so it springs back automatically when the look input is released.
			if (autoZeroHorizontalOnMobile) {
				m_TargetAngles.y = Mathf.Lerp (-rotationRange.y * 0.5f, rotationRange.y * 0.5f, inputH * .5f + .5f);
			} else {
				m_TargetAngles.y += inputH * rotationSpeed;
			}
			if (autoZeroVerticalOnMobile) {
				m_TargetAngles.x = Mathf.Lerp (-rotationRange.x * 0.5f, rotationRange.x * 0.5f, inputV * .5f + .5f);
			} else {
				m_TargetAngles.x += inputV * rotationSpeed;
			}
#else
            // with mouse input, we have direct control with no springback required.
            m_TargetAngles.y += inputH * rotationSpeed;
            m_TargetAngles.x += inputV * rotationSpeed;
#endif

            // clamp values to allowed range
            m_TargetAngles.y = Mathf.Clamp(m_TargetAngles.y, -rotationRange.y * 0.5f, rotationRange.y * 0.5f);
            m_TargetAngles.x = Mathf.Clamp(m_TargetAngles.x, -rotationRange.x * 0.5f, rotationRange.x * 0.5f);
        }
        else
        {
            inputH = Input.mousePosition.x;
            inputV = Input.mousePosition.y;

            // set values to allowed range
            m_TargetAngles.y = Mathf.Lerp(-rotationRange.y * 0.5f, rotationRange.y * 0.5f, inputH / Screen.width);
            m_TargetAngles.x = Mathf.Lerp(-rotationRange.x * 0.5f, rotationRange.x * 0.5f, inputV / Screen.height);
        }

        // smoothly interpolate current values to target angles
        m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, m_TargetAngles, ref m_FollowVelocity, dampingTime);

        // update the actual gameobject's rotation
        cam.transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
    }

    void Move()
    {
        

        if (Input.GetAxisRaw ("Vertical") > 0)
        {
            
         
            curSpeed += accelValue * Time.deltaTime;
        }
        else if (curSpeed > 0)
        {
            curSpeed -= decelValue * Time.deltaTime;
        }
        else
        {
            curSpeed = 0;
        }

        finalMove = transform.forward * curSpeed;
        //finalMove.y = finalMove.y - (gravityMultiplier * Time.deltaTime);
        cc.Move(finalMove * Time.deltaTime);
        cc.Move(-transform.up * gravityMultiplier * Time.deltaTime);

        rot = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        transform.Rotate(rot * rotSpeed * Time.deltaTime);

        //transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0 ));
        curSpeed = Mathf.Clamp(curSpeed, 0, moveSpeed);
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
        if (isZoomed)
        {
            takeHiResShot |= Input.GetKeyDown("k");
            if (takeHiResShot)
            {
                canvas.enabled = false;
                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                cam.targetTexture = rt;
                Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                cam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
                cam.targetTexture = gameRender;
                RenderTexture.active = gameRender; // JC: added to avoid errors
                Destroy(rt);
                StartCoroutine("Flash");
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = ScreenShotName(resWidth, resHeight);
                System.IO.File.WriteAllBytes(filename, bytes);
                Debug.Log(string.Format("Took screenshot to: {0}", filename));
                takeHiResShot = false;
                canvas.enabled = true;

            }
        }
        else
        {

        }
    }


    IEnumerator Flash()
    {
        flash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flash.gameObject.SetActive(false);
    }
}
