using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
partial class CharLoader : MonoBehaviour 
{
    private GameObject battleInfo;
    public GameObject btnPrefab;
    private RectTransform rectTransform;
    public Transform content;
    private List<GameObject> btns = new List<GameObject>();
    private void Start() {
        battleInfo = GameObject.Find("levelInfo");
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(207.2f*((CharData.instance.charList.Count/2)+1)+60+145,rectTransform.sizeDelta.y);
        foreach(CharData.Character c in CharData.instance.charList) {
            GameObject btn = Instantiate(btnPrefab);
            btn.GetComponent<Image>().sprite = c.charHalf;
            btn.GetComponent<BtnInfo>().cid = c.id;
            btn.transform.SetParent(content);
            if(battleInfo.GetComponent<BattleInfo>().charInds.IndexOf(btn.GetComponent<BtnInfo>().cid) >= 0){
                btn.GetComponent<BtnInfo>().OnClicked();
            }
            btns.Add(btn);
        }
    }
    public void close(){
        SceneManager.LoadScene("Scenes/CharChooser");
    }
    public void finish(){
        SceneManager.LoadScene("Scenes/CharChooser");
        int i = 0;
        battleInfo.GetComponent<BattleInfo>().charInds.Clear();
        while(battleInfo.GetComponent<BattleInfo>().charInds.Count < 13 && i < btns.Count){
            if(btns[i].GetComponent<BtnInfo>().is_selected && battleInfo.GetComponent<BattleInfo>().charInds.IndexOf(btns[i].GetComponent<BtnInfo>().cid) < 0){
                battleInfo.GetComponent<BattleInfo>().charInds.Add(btns[i].GetComponent<BtnInfo>().cid);
            }
            i++;
        }
    }
}