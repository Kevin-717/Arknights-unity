using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
public class Begin : MonoBehaviour 
{
    public GameObject[] progressUI = {};
    public Text[] progressText = {};
    private bool loading = false;
    private float timer = 0;
    public Transform meshS;
    public RectTransform meshSTransfrom;
    public void BeginLoad() {
        progressUI[0].GetComponent<RectTransform>().DOLocalMoveX(-573.6676f,6f);
        progressUI[1].GetComponent<RectTransform>().DOLocalMoveX(590.6025f,6f);
        loading = true;
        meshSTransfrom.DOLocalMoveY(674,2f);
    }
    private void Update() {
        if (loading) {
            timer += Time.deltaTime;
            progressText[0].text = Math.Round(timer/6*100).ToString()+'%';
            progressText[1].text = Math.Round(timer/6*100).ToString()+'%';
            if(timer > 6){
                loading = false;
                Invoke("EnterToMain",2f);
            }
        }
        meshS.eulerAngles += new Vector3(0,1,0)*2*Time.deltaTime;
    }
    private void EnterToMain(){
        SceneManager.LoadScene("Scenes/Main");
    }
}