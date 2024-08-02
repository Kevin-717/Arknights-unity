using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Pathfinding;

public class Enemy : MonoBehaviour{
    private SkeletonAnimation spine_anim;
    private AIDestinationSetter aIDestinationSetter;
    private AIPath aIPath;
    public float speed = 3;
    private GameObject attackObject;
    [HideInInspector]
    public List<GameObject> move_line;
    public int move_index = 0;
    private float waitTime = 0;
    public float hp = 50f;
    public Image HpAnim;
    public Image HpBar;
    private float totalHp;
    public float damage = 5f;
    private float hpScale = 1.0f;
    private float animScale = 1.0f;
    public string Move_anim;
    public string Idle_anim;
    public string Attack_anim;
    public string Die_anim;
    [HideInInspector]
    public string state;
    [HideInInspector]
    public bool useSpecial = false;
    public float def = 50;
    public float adef = 50;
    public bool haveStart = false;
    public BattleController.damageType dt;
    // Start is called before the first frame update
    void Start()
    {
        spine_anim = GetComponent<SkeletonAnimation>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();
        aIPath.maxSpeed = 0;
        aIDestinationSetter.target = move_line[move_index].transform;
        totalHp = hp;
        if(!haveStart){
            state = Move_anim;
        }
    }
    private void Move(){
        if(move_index == move_line.Count){
            BattleController.Instance.life--;
            Destroy(gameObject);
            return;
        }
        if(Vector3.Distance(move_line[move_index].transform.position,transform.position) <= 0.7f){
            if(move_line[move_index].GetComponent<PointInfo>().isWait == PointType.wait){
                waitTime = move_line[move_index].GetComponent<PointInfo>().waitTime;
                state = Idle_anim;
                return;
            }
            move_index++;
        }else{
            aIDestinationSetter.target = move_line[move_index].transform;
            float x = transform.position.x;
            float dx = move_line[move_index].transform.position.x;
            float mx = (Mathf.Abs(x-dx)<=0.1f)?0:(x>dx?-1:1);
            if(mx < 0){
                transform.eulerAngles = new Vector3(30,180,0);
            }else{
                transform.eulerAngles = new Vector3(-30,0,0);
            }
            // Vector3 movement = new Vector3(mx,my,mz);
            // transform.Translate(movement*speed*Time.deltaTime);
        }
    }
    private void Delay(){
        waitTime -= Time.deltaTime;
        if(waitTime < 0){
            state = Move_anim;
            move_index ++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHpBar();
        if(spine_anim.AnimationName != state){
            if(state != Die_anim){
                spine_anim.state.SetAnimation(0,state,true);
            }else{
                spine_anim.state.SetAnimation(0,state,false);
            }
        }
        if(state == Move_anim){
            aIPath.maxSpeed = speed;
            Move();
        }else if(state == Idle_anim){
            aIPath.maxSpeed = 0;
            Delay();
        }else if(state == Attack_anim){
            aIPath.maxSpeed = 0;
            if(attackObject != null && attackObject.GetComponentInParent<Char>().hp <= 0){
                attackObject = null;
                state = Move_anim;
            }
        }else{
            
        }
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
            aIPath.maxSpeed = 0;
            spine_anim.state.Complete += delegate{
                Destroy(gameObject);
            };
        }
        hpScale = hp/totalHp;
        animScale = hp/totalHp;
    }
    private void OnDestroy() {
        if(EnemyController.Instance != null){
            EnemyController.Instance.enemyNum--;
            BattleController.Instance.enemyNum++;
        }
    }
    public void Damage(){
        try{
            if(attackObject != null && (!useSpecial)){
                attackObject.GetComponentInParent<Char>().TakeDamage(damage,dt);
            }
        }catch{}
    }
    private void UpdateHpBar(){
        HpBar.transform.localScale = new Vector3(hpScale,1,1);
        if(HpAnim.transform.localScale.x > animScale){
            HpAnim.transform.localScale -= new Vector3(1,0,0)*Time.deltaTime;
        }else{
            HpAnim.transform.localScale = new Vector3(animScale,1,1);
        }
    }
    private void OnTriggerStay(Collider other) {
        if(state != Die_anim && state != Attack_anim){
            if(other.gameObject.tag == "attackTarget"){
                if(other.gameObject.GetComponentInParent<Char>().hp > 0 && 
                other.gameObject.GetComponentInParent<Char>().state != "Default"){
                    state = Attack_anim;
                    attackObject = other.gameObject;
                }
            }
        }
    }
}