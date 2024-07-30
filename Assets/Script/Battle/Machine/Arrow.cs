using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Arrow : MonoBehaviour 
{
    public Vector3 movement;
    public Vector3 direction;
    public float speed = 10;

    public float atk = 100;
    private void Start() {
        transform.eulerAngles = direction;
    }
    private void Update() {
        transform.Translate(movement*speed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "attackTarget"){
            if(other.gameObject.GetComponentInParent<Char>().state != "Default"){
                other.gameObject.GetComponentInParent<Char>().TakeDamage(atk,BattleController.damageType.Physics);
                Destroy(gameObject);
            }
        }
    }
}