using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine.UIElements;
public class Char : MonoBehaviour 
{
    public float hp = 50;
    public GameObject HpAnim;
    public GameObject HpBar;
    private float totalHp;
    private float hpScale = 1.0f;
    private float animScale = 1.0f;
    private SkeletonAnimation skeletonAnimation;
    public string Start_anim;
    public string Idle_anim;
    public string Attack_anim;
    public string Die_anim;
    [HideInInspector]
    public string state;
    public float damage = 5f;
    private GameObject attackObject;
    private string first = "getPos";
    [HideInInspector]
    public bool can_put = false;
    public enum charType
    {
        lowLand,
        highLand
    }
    public charType ct = charType.lowLand;
    public GameObject getDFrame;
    public GameObject getD;
    public GameObject showD;
    public GameObject attackRange;
    public GameObject dCircle;
    public GameObject targetCollider;
    private string di = "left";
    public GameObject createBtn;
    private int debugId;
    private bool b = false;
    public float def = 50;
    public float adef = 50;
    public BattleController.damageType dt;
    void Start() {
        debugId = Random.Range(10000,99999);
        totalHp = hp;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        getDFrame.SetActive(false);
        state = "Default";
    }
    public void TakeDamage(float damage,BattleController.damageType dt){
        float value = 0;
        if(dt == BattleController.damageType.Physics){
            value = Mathf.Max(0.05f*damage,damage-def);
        }else if(dt == BattleController.damageType.Abillity){
            value = Mathf.Max(0.05f*damage,damage*(1-adef*0.01f));
        }
        hp -= value;
        if(hp <= 0){
            hp = 0;
            state = Die_anim;
            skeletonAnimation.state.Complete += delegate{
                Destroy(gameObject);
            };
        }
        hpScale = hp/totalHp;
        animScale = hp/totalHp;
    }
    private void UpdateHpBar(){
        HpBar.transform.localScale = new Vector3(hpScale,1,1);
        if(HpAnim.transform.localScale.x > animScale){
            HpAnim.transform.localScale -= new Vector3(1,0,0)*Time.deltaTime;
        }else{
            HpAnim.transform.localScale = new Vector3(animScale,1,1);
        }
    }
    void Update(){
        UpdateHpBar();
        if(skeletonAnimation.AnimationName != state){
            if(state == Idle_anim || state == Attack_anim){
                skeletonAnimation.state.SetAnimation(0,state,true);
            }else{
                skeletonAnimation.state.SetAnimation(0,state,false);
            }
        }
        if(di == "right" && state != "Default" && (!b)){
            b = true;
            transform.eulerAngles = new Vector3(30,180,0);
            dCircle.transform.eulerAngles = new Vector3(0,0,0);
            attackRange.transform.localEulerAngles = new Vector3(-60,attackRange.transform.localEulerAngles.y,attackRange.transform.localEulerAngles.z-180);
            targetCollider.transform.localEulerAngles = new Vector3(-30,0,0);
        }
        if(state == "Default"){
            OnStart();
        }
        else if(state == Idle_anim){

        }else if(state == Attack_anim){
            if(attackObject.GetComponentInParent<Enemy>().hp <= 0){
                state = Idle_anim;
                attackObject = null;
            }
        }else if(state == Die_anim){

        }else if(state == Start_anim){

        }
    }
    private void OnStart(){
        if(first == "getPos"){
            if(!can_put){
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
                Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
                transform.position = new Vector3(wp.x,wp.y,0);
            }
            RaycastHit[] hitInfos;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitInfos = Physics.RaycastAll(ray,1000);
            if(hitInfos.Length==0){can_put = false;return;}
            foreach(RaycastHit hitInfo in hitInfos)
            {
                if(ct == charType.lowLand && hitInfo.collider.gameObject.tag == "lowland" && hitInfo.collider.gameObject.GetComponent<MapRender>().can_place){
                    can_put = true;
                    transform.position = hitInfo.collider.gameObject.transform.position;
                    return;
                }else if(ct == charType.highLand && hitInfo.collider.gameObject.tag == "highland" && hitInfo.collider.gameObject.GetComponent<MapRender>().can_place){
                    can_put = true;
                    transform.position = hitInfo.collider.gameObject.transform.position;
                    return;
                }else{
                    can_put = false;
                }
            }
        }else{
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
            Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
            if(Vector3.Distance(new Vector3(wp.x,wp.y,getD.transform.position.z),getDFrame.transform.position) < 1.1f){
                getD.transform.position = new Vector3(wp.x,wp.y,getD.transform.position.z);
            }
            GetD();
        }
    }
    void GetD(){
        Vector3 d = getD.transform.position;
        Vector3 o = getDFrame.transform.position;
        int angle = 0;
        if(d.y > o.y && Mathf.Abs(d.y-o.y)>0.8f){
            di = "left";
            angle = 90;
        }else if(d.y < o.y && Mathf.Abs(d.y-o.y)>0.8f){
            di = "left";
            angle = 270;
        }else if(d.x < o.x){
            di = "right";
            angle = 180;
        }else if(d.x > o.x){
            di = "left";
            angle = 0;
        }
        showD.transform.eulerAngles = new Vector3(0,0,angle);
        attackRange.transform.eulerAngles = new Vector3(0,0,angle);
    }
    private void FixedUpdate() {
        if(state == "Default"){
            //Debug.Log(debugId.ToString()+" - a");
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
            Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
            if(Input.GetMouseButton(0) && can_put && first == "getPos"){
                first = "getD";
                getDFrame.SetActive(true);
            }
            if(Input.GetMouseButton(1) && first == "getD" && Vector3.Distance(new Vector3(wp.x,wp.y,getD.transform.position.z),getDFrame.transform.position) > 0.5f){
                getDFrame.SetActive(false);
                state = Start_anim;
                BattleController.Instance.is_place = false;
                skeletonAnimation.state.Complete += delegate {
                    state = Idle_anim;
                };
            }
        }
    }
    public void Damage(){
        try{
            if(attackObject != null){
                attackObject.GetComponentInParent<Enemy>().TakeDamage(damage,dt);
            }
        }catch{}
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Enemy" && state != Die_anim && state != Start_anim && state != "Default"){
            if(other.gameObject.GetComponentInParent<Enemy>().hp > 0){
                if(ct == charType.lowLand && other.gameObject.GetComponentInParent<Enemy>().enemyType == Enemy.EnemyType.Fly){
                    return;
                }
                attackObject = other.gameObject;
                state = Attack_anim;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Enemy" && other.gameObject == attackObject && state != Die_anim && state != Start_anim){
            state = Idle_anim;
            attackObject = null;
        }    
    }
    private void OnDestroy() {
        if(createBtn != null){
            createBtn.GetComponent<CharCreator>().Respawn();
        }
    }
}