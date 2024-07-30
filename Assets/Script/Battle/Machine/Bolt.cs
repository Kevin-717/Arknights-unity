using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Bolt : MonoBehaviour 
{
    public float waitTime;
    private float countdown;
    public Transform bar;
    public GameObject ArrowPrefab;
    public Transform createPos;
    public GameObject line;
    private LineRenderer lineRenderer;
    public enum D{
        left,
        right,
        up,
        down,
    }
    public D dir;
    private void CreateArrow(){
        Vector3 direction = new Vector3(0,0,0);
        Vector3 movement = new Vector3(0,0,0);
        if(dir == D.left){
            direction.z = 0;
            movement.x = -1;
        }else if(dir == D.right){
            direction.z = 180;
            movement.x = -1;
        }else if(dir == D.up){
            direction.z = 270;
            movement.x = -1;
        }else{
            direction.z = 90;
            movement.x = -1;
        }
        GameObject arrow = Instantiate(ArrowPrefab, createPos.position,Quaternion.identity);
        arrow.GetComponent<Arrow>().direction = direction;
        arrow.GetComponent<Arrow>().movement = movement;

    }
    private void Start() {
        countdown = 0;
        lineRenderer = line.GetComponent<LineRenderer>();
    }
    private void Update() {
        countdown += Time.deltaTime;
        bar.localScale = new Vector3(countdown/waitTime,1,1);
        lineRenderer.SetPosition(1,new Vector3((countdown/waitTime)*0.00003f,0,0.00004f));
        if(countdown >= waitTime){
            countdown = 0;
            CreateArrow();
        }
    }
}