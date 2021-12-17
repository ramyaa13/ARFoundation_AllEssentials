using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARPlacement : MonoBehaviour
{

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject Red, Green, Blue;
    [SerializeField]
    Button RedButton, GreenButton, BlueButton;


    GameObject spawnablePrefab;
    Camera arCam;
    GameObject spawnedObject;
    public Text DebugText;

    private Renderer r;
    private Color c;

    private void Awake()
    {
        spawnablePrefab = Red;
        RedButton.onClick.AddListener(() => SetPrefabType(Red));
        GreenButton.onClick.AddListener(() => SetPrefabType(Green));
        BlueButton.onClick.AddListener(() => SetPrefabType(Blue));
    }

    private void Start()
    {
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        RaycastHit Hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                if (Physics.Raycast(ray, out Hit))
                {
                    if (Hit.collider.gameObject.tag == "Cube")
                    {
                        spawnedObject = Hit.collider.gameObject;
                        c = spawnedObject.GetComponent<Renderer>().material.GetColor("_Color");
                        spawnedObject.GetComponent<BoxCollider>().isTrigger = true;
                    }
                    else
                    {
                        SpawnPrefab(m_Hits[0].pose.position);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = m_Hits[0].pose.position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject.GetComponent<BoxCollider>().isTrigger = false;
                DebugText.text = "clear";
                spawnedObject = null;
            }
        }
    }



    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
    }

    public void SetPrefabType(GameObject PrefabType)
    {
        spawnablePrefab = PrefabType;
    }

    public void CollisionDetection()
    {
        DebugText.text = "collides";
        r = spawnedObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", Color.red);
    }

    public void CollisionFinished()
    {
        DebugText.text = "collide exits";
        r = spawnedObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", c);
    }
}
