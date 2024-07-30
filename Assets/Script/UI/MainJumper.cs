using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainJumper : MonoBehaviour 
{
    public void Jump(string scene_name){
        SceneManager.LoadScene(scene_name);
    }
}