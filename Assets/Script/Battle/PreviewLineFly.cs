using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PreviewLineFly: MonoBehaviour 
{
    [HideInInspector]
    public List<GameObject> move_line;
    private int move_index = 0;
    public float speed = 8;
    private Rigidbody rb;
    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    private void Move(){
        if(move_index == move_line.Count){
            Destroy(gameObject);
            return;
        }
        Debug.Log(Vector3.Distance(move_line[move_index].transform.position,transform.position));
        if(Vector3.Distance(move_line[move_index].transform.position,transform.position) <= 0.6f){
            move_index++;
        }else{
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            float dx = move_line[move_index].transform.position.x;
            float dy = move_line[move_index].transform.position.y;
            float dz = move_line[move_index].transform.position.z;
            float mx = (Mathf.Abs(x-dx)<=0.1f)?0:(x>dx?-1:1);
            float my = (Mathf.Abs(y-dy)<=0.1f)?0:(y>dy?-1:1);
            float mz = (Mathf.Abs(z-dz)<=0.1f)?0:(z>dz?-1:1);
            if(mx < 0){
                transform.eulerAngles = new Vector3(30,180,0);
            }else{
                transform.eulerAngles = new Vector3(-30,0,0);
            }
            Vector3 movement = new Vector3(mx,my,mz);
            Debug.Log(movement);
            rb.transform.Translate(movement*speed*Time.deltaTime);
        }
    }
    void FixedUpdate(){
        Move();
    }
}