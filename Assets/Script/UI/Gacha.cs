using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Gacha : MonoBehaviour 
{
    [System.Serializable]
    public class CharPool
    {
        public List<CharData.Character> one_star = new List<CharData.Character>();
        public List<CharData.Character> two_star = new List<CharData.Character>();
        public List<CharData.Character> three_star = new List<CharData.Character>();
        public List<CharData.Character> four_star = new List<CharData.Character>();
        public List<CharData.Character> five_star = new List<CharData.Character>();
        public List<CharData.Character> six_star = new List<CharData.Character>();
        
    }
    public CharPool pool;
    private Animator animator;
    private int trigger = Animator.StringToHash("clicked");
    private void Start() {
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate() {
        if(Input.GetMouseButtonDown(0)){
            animator.SetTrigger(trigger);
        }
    }
}