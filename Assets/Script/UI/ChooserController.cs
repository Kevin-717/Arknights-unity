using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChooserController : MonoBehaviour 
{
    public List<GameObject> btns;
    private GameObject battleInfo;
    public Sprite empty;
    public List<Sprite> char_images = new List<Sprite>();
    private void Start() {
        battleInfo = GameObject.FindGameObjectWithTag("data");
        foreach(CharData.Character character in CharData.instance.charList){
            char_images.Add(character.charHalf);
        }
        foreach(GameObject btn in btns) {
            btn.GetComponent<Image>().sprite = empty;
        }
        int i = 0;
        foreach(int ind in battleInfo.GetComponent<BattleInfo>().charInds){
            btns[i].GetComponent<Image>().sprite = char_images[ind];
            i++;
        }
    }
    public void EnterToList(){
        SceneManager.LoadScene("Scenes/CharList");
    }
    public void EnterToBattle(){
        SceneManager.LoadScene(battleInfo.GetComponent<BattleInfo>().scene_ind);
    }
}