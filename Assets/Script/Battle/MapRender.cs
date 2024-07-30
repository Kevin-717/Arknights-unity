using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapRender : MonoBehaviour 
{
    public GameObject attackRange;
    public GameObject canPlace;
    private bool show_range = false;
    public bool can_place = false;
    public enum LandType
    {
        highLand,
        lowLand
    }
    public LandType type;
    private void Start() {
        attackRange.SetActive(false);
        canPlace.SetActive(false);
    }
    private void Update() {
        if(BattleController.Instance.is_place && type == (BattleController.Instance.is_lowland ? LandType.lowLand : LandType.highLand) && (!show_range) && can_place){
            canPlace.SetActive(true);
        }else{
            canPlace.SetActive(false);
        }
        Ray ray = new Ray(transform.position-new Vector3(0,0,-5),transform.forward*-100);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.gameObject.tag == "attackRange" &&
             hit.collider.gameObject.GetComponentInParent<Char>().state == "Default" &&
             hit.collider.gameObject.GetComponentInParent<Char>().can_put){
                show_range = true;
                attackRange.SetActive(true);
            }else{
                show_range = false;
                attackRange.SetActive(false);
            }
        }
    }
}