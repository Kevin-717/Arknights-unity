using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelEnter : MonoBehaviour 
{
    public string level_name;
    public string level_description;
    public string level_num;
    public string level_difficult;
    public string sceneInd;
    public GameObject d;
    private Detail Detail;
    public string[] stories;
    private void Start() {
        Detail = d.GetComponent<Detail>();
    }
    public void OnClicked(){
        Detail.level_name_str = level_name;
        Detail.level_description_str=level_description;
        Detail.level_num_str=level_num;
        Detail.level_difficult_str = level_difficult;
        Detail.level_ind = sceneInd;
        Detail.story_paths = stories;
        Detail.Show();
    }
}