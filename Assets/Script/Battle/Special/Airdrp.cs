using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
public class Airdrp : MonoBehaviour 
{
    public string start;
    private Enemy enemyController;
    private SkeletonAnimation skeletonAnimation;
    private void Start() {
        enemyController = GetComponent<Enemy>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        enemyController.state = start;
        skeletonAnimation.state.Complete += delegate {
            if(enemyController.state == start){
                enemyController.state = enemyController.Move_anim;
            }
        };
    }
}