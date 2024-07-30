using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PaoDan : MonoBehaviour 
{
    public float damage = 0;
    public BattleController.damageType damageType = BattleController.damageType.Physics;
    public GameObject target;
    private void OnDestroy() {
        target.GetComponent<Char>().TakeDamage(damage,damageType);
    }
}