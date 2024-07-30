using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 
/// <summary>
/// 设计编写：常成功
/// 创建时间：2020/05/12 
/// 脚本功能：从A点(起始点), 到B点(目标点)的抛物线运动
/// 挂载位置：动态挂载, 即将运动的物体上
/// </summary>
/// 
/// ps. 关于箭的制作:
/// 1, 资源原点, 在箭头上(一般是箭头产生攻击力)
/// 2, 箭头朝向, 是z轴的增长方向
 
 
// 从A点(起始点), 到B点(目标点)的抛物线运动
public class Parabola_A_to_B : MonoBehaviour
{
    // 目标点Transform
    public Transform target_trans; 
    // 运动速度
    public float speed = 10;
    // 最小接近距离, 以停止运动
    public float min_distance = 0.5f;
    private float distanceToTarget;
    private bool move_flag = true;
    private Transform m_trans;
 
    void Start()
    {
        m_trans = this.transform; 
        distanceToTarget = Vector3.Distance(m_trans.position, target_trans.position);
        StartCoroutine(Parabola());
    }
 
    IEnumerator Parabola()
    {
        while (move_flag)
        {
            Vector3 targetPos = target_trans.position;
            // 朝向目标, 以计算运动
            m_trans.LookAt(targetPos);
            // 根据距离衰减 角度
            float angle = Mathf.Min(1, Vector3.Distance(m_trans.position, targetPos) / distanceToTarget) * 45;
            // 旋转对应的角度（线性插值一定角度，然后每帧绕X轴旋转）
            m_trans.rotation = m_trans.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            // 当前距离目标点
            float currentDist = Vector3.Distance(m_trans.position, target_trans.position);
            // 很接近目标了, 准备结束循环
            if (currentDist < min_distance)
            {
                move_flag = false; 
            }
            // 平移 (朝向Z轴移动)
            m_trans.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
            // 暂停执行, 等待下一帧再执行while
            yield return null;
        }
        if (move_flag == false)
        {
            // 使自己的位置, 跟[目标点]重合
            // [停止]当前协程任务,参数是协程方法名
            StopCoroutine(Parabola());
            Destroy(gameObject);
            // 销毁脚本
        }
    }
}