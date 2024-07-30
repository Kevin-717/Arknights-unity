using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BtnInfo : MonoBehaviour 
{
    public int cid;
    public GameObject selected;
    public bool is_selected = false;
    public void OnClicked(){
        is_selected = !is_selected;
        selected.SetActive(is_selected);
    }
}