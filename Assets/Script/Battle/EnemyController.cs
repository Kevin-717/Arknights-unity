using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;
    public GameObject previewLinePrefab;
    public List<LineInfo> pathList;
    public List<EnemyInfo> enemyInfos;
    private int index = 0;
    private float nowTime = 0;
    private bool flag = false;
    [HideInInspector]
    public int enemyNum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    private void CreateEnemy(){
        if(index == enemyInfos.Count){
            return;
        }
        if(enemyInfos[index].isWait && (!(enemyNum == 0))){
            return;
        }
        if(nowTime >= enemyInfos[index].time-2 && (!flag)){
            flag = true;
            CreatePreviewLine(enemyInfos[index].pathInd);
        }
        if(nowTime >= enemyInfos[index].time){
            List<GameObject> path = pathList[enemyInfos[index].pathInd].line;
            GameObject enemy = Instantiate(enemyInfos[index].enemyPrefab,path[0].transform.position,Quaternion.identity);
            enemy.transform.eulerAngles = new Vector3(-30,0,0);
            enemy.GetComponent<Enemy>().move_line = path;
            flag = false;
            index++;
            enemyNum++;
        }
    }
    private void CreatePreviewLine(int lineInd){
        List<GameObject> path = pathList[lineInd].line;
        GameObject line = Instantiate(previewLinePrefab,path[0].transform.position,Quaternion.identity);
        line.transform.eulerAngles = new Vector3(90,0,0);
        line.GetComponent<PreviewLine>().move_line = path;
    }
    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;
        CreateEnemy();
    }
}

[System.Serializable]
public class EnemyInfo
{
    public GameObject enemyPrefab;
    public int pathInd;
    public float time;
    public bool isWait;
}
[System.Serializable]
public class LineInfo
{
    public List<GameObject> line;
}