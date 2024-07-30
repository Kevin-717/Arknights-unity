using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType
{
    normal,
    wait,
}
public class PointInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public PointType isWait;
    public int waitTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}