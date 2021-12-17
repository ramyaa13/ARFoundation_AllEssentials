using TMPro;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    private ARPlacement aRPlacement;
    public Color c;

    private void Start()
    {
       aRPlacement = FindObjectOfType<ARPlacement>();
        GetComponent<Renderer>().material.SetColor("_Color", c);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Cube")
        {
            aRPlacement.CollisionDetection();
           
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        aRPlacement.CollisionFinished();
        GetComponent<Renderer>().material.SetColor("_Color", c);
    }

}