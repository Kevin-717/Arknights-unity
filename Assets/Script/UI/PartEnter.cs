using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PartEnter : MonoBehaviour 
{
    public void loadPart(string scene_name){
        SceneManager.LoadScene(scene_name);
    }
}