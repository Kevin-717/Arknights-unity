using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class StoryJumper : MonoBehaviour 
{
    public static StoryJumper Instance;
    public string storyPath;
    public string parentScene;
    private void Start() {
        Instance = this;
    }
    public void JumpToStory(){
        SceneManager.LoadScene(2);
    }
    public void JumpToParent(){
        SceneManager.LoadScene(parentScene);
    }
}