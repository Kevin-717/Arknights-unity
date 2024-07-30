using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Detail : MonoBehaviour 
{
    public Text level_num;
    public Text level_name;
    public Text level_description;
    public Text level_difficult;
    public GameObject[] storyBtn;
    public GameObject storyBoard;
    public string parentScene;
    [HideInInspector]
    public string level_ind;
    [HideInInspector]
    public string level_num_str;
    [HideInInspector]
    public string level_name_str;
    [HideInInspector]
    public string level_description_str;
    [HideInInspector]
    public string level_difficult_str;
    [HideInInspector]
    public string[] story_paths;
    void Start(){
    }
    void Update(){
        if(Input.GetMouseButtonDown(1)){
            gameObject.SetActive(false);
        }
    }
    public void Show(){
        level_name.text = level_name_str;
        level_description.text = level_description_str;
        level_num.text = level_num_str;
        level_difficult.text = level_difficult_str;
        gameObject.SetActive(true);
        storyBoard.SetActive(false);
        storyBtn[0].SetActive(false);
        storyBtn[1].SetActive(false);
        if(story_paths.Length > 0){
            storyBoard.SetActive(true);
            for(int i = 0;i<story_paths.Length;i++){
                storyBtn[i].SetActive(true);
                storyBtn[i].GetComponentInChildren<Text>().text = level_num_str+(i==0?"行动前":"行动后");
            }
        }
    }
    public void EnterChooser(){
        BattleInfo.instance.scene_ind = level_ind;
        SceneManager.LoadScene("Scenes/CharChooser");
    }
    public void EnterStory(int i){
        string sp = story_paths[i];
        StoryJumper.Instance.storyPath = sp;
        StoryJumper.Instance.parentScene = parentScene;
        StoryJumper.Instance.JumpToStory();
    }
}