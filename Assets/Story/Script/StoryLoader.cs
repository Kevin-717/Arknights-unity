using LitJson;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System;
using DG.Tweening;
public class StoryLoader : MonoBehaviour 
{
    public class StoryItem{
        public enum ActionType{
            Dialog,
            HideDialog,
            PlayMusic,
            Background,
            Delay,
            Character,
            Image,
            HideImage,
            ImageTween,
            ImageFade,
            Decision,
        }
        public ActionType Type;
        //params 1
        public string char_name;
        public string char_text;
        //params 2
        public string image_path;
        //params 3
        public List<string> char_images;
        public int focus;
        //params 4
        public string img_path;
        public Vector3 ip;
        public Vector3 isca;
        //params 5 
        public Vector3 pFrom;
        public Vector3 pTo;
        public Vector3 sFrom;
        public Vector3 sTo;
        public float durtation;
        //params 6
        public float fadeDurtation;
        //params 7
        public List<string> options;
        public List<List<int>> reactions;
        public List<List<int>> ranges;
        public int end_index;

    }
    [Header("剧情文件路径")]
    public string fn;
    private JsonData storyVars;
    private string storyText;
    public List<StoryItem> storyData;
    private int play_index = 0;
    private int text_index = 0;
    private string currentText = "";
    private string text_temp = "";
    private bool wait = false;
    private bool decision = false;
    private bool wait2 = false;
    private StoryItem current_decision;
    private int select_range;
    [Header("组件对象填入")]
    public Text char_name_text;
    public Text char_say_text;
    public Image bg_image;
    public Image char_0;
    public Image char_1;
    public Image char1_mask;
    public Image char0_mask;
    public GameObject img_image;
    public GameObject decision_frame;
    public List<GameObject> decision_buttons;
    [Header("调试模式")]
    public bool debug = false;
    private void Start() {
        if(!debug){
            fn = StoryJumper.Instance.storyPath;
        }
        storyVars = JsonMapper.ToObject(ReadData(Application.streamingAssetsPath + "\\story_variables.json"));
        storyText = ReadData(Application.streamingAssetsPath+"\\"+fn);
        storyData = loadStory(storyText);
        Debug.Log("load finish");
    }
    private void Update() {
        if((!wait) && (!wait2)){
            if(decision){
                if(play_index > select_range){
                    decision = false;
                    play_index = current_decision.end_index;
                }
            }
            PlayNext();
        }
        if(Input.GetMouseButtonDown(0)){
            wait = false;
            StopAllCoroutines();
        }
    }
    IEnumerator SayWordByWord(){
        while(text_index < currentText.Length){
            text_temp += currentText[text_index];
            text_index++;
            char_say_text.text = text_temp;
            yield return new WaitForSeconds(0.04f);
        }
        StopAllCoroutines();
    }
    private void PlayNext(){
        // play story
        if(play_index >= storyData.Count){
            StoryJumper.Instance.JumpToParent();
            return;
        }
        StoryItem storyItem = storyData[play_index];
        switch(storyItem.Type){
            case StoryItem.ActionType.Dialog:
                char_name_text.text = storyItem.char_name;
                text_index = 0;
                text_temp = "";
                currentText = storyItem.char_text;
                StartCoroutine("SayWordByWord");
                wait = true;
                Debug.Log(storyItem.char_name + " Say: "+storyItem.char_text);
                break;
            case StoryItem.ActionType.Background:
                Debug.Log("Show Background -> "+storyItem.image_path);
                Sprite bg = Resources.Load<Sprite>(storyItem.image_path);
                Debug.Log(bg);
                bg_image.enabled = true;
                bg_image.sprite = bg;
                break;
            case StoryItem.ActionType.Character:
                Debug.Log("Show Character -> " + storyItem.char_images.Count + " -> " + storyItem.focus);
                char0_mask.enabled = false;
                char1_mask.enabled = false;
                if(storyItem.char_images.Count == 1){
                    char_0.enabled = true;
                    char0_mask.sprite = Resources.Load<Sprite>(storyItem.char_images[0]);
                    char0_mask.enabled = false;
                    char_0.sprite = Resources.Load<Sprite>(storyItem.char_images[0]);
                    char_1.enabled = false;
                }else{
                    char_0.enabled = true;
                    char_1.enabled = true;
                    char0_mask.sprite = Resources.Load<Sprite>(storyItem.char_images[0]);
                    char1_mask.sprite = Resources.Load<Sprite>(storyItem.char_images[1]);
                    char_0.sprite = Resources.Load<Sprite>(storyItem.char_images[0]);
                    char_1.sprite = Resources.Load<Sprite>(storyItem.char_images[1]);
                    if(storyItem.focus == 0){
                        char1_mask.enabled = true;
                    }else{
                        char0_mask.enabled = true;
                    }
                }
                break;
            case StoryItem.ActionType.Image:
                Debug.Log("Show Image -> " + storyItem.img_path);
                img_image.GetComponent<Image>().enabled = true;
                img_image.GetComponent<Image>().sprite = Resources.Load<Sprite>(storyItem.img_path);
                img_image.GetComponent<RectTransform>().localPosition = storyItem.ip;
                img_image.GetComponent<RectTransform>().localScale = storyItem.isca;
                break;
            case StoryItem.ActionType.ImageTween:
                img_image.GetComponent<RectTransform>().localPosition = storyItem.pFrom;
                img_image.GetComponent<RectTransform>().localScale = storyItem.sFrom;
                img_image.GetComponent<RectTransform>().DOLocalMove(storyItem.pTo,storyItem.durtation);
                img_image.GetComponent<RectTransform>().DOScale(storyItem.sTo,storyItem.durtation);
                break;
            case StoryItem.ActionType.ImageFade:
                img_image.GetComponent<Image>()
                .DOFade(0,storyItem.fadeDurtation)
                .OnComplete(()=>{
                    img_image.GetComponent<Image>().enabled = false;
                    img_image.GetComponent<Image>().DOFade(1,0);
                });
                break;
            case StoryItem.ActionType.Decision:
                wait2 = true;
                decision = true;
                current_decision = storyItem;
                decision_frame.SetActive(true);
                int i = 0;
                foreach(GameObject b in decision_buttons){
                    b.SetActive(false);
                }
                foreach(GameObject btn in decision_buttons){
                    if(i < storyItem.options.Count){
                        btn.SetActive(true);
                        btn.GetComponentInChildren<Text>().text = storyItem.options[i];
                        i++;
                    }
                }
                break;
            case StoryItem.ActionType.HideImage:
                img_image.GetComponent<Image>().enabled = false;
                break;
        }
        play_index++;
    }
    public void play_decision(int val){
        decision_frame.SetActive(false);
        foreach(List<int> connect in current_decision.reactions){
            Debug.Log("match -> " + val + " <- " + connect[0] + " " + connect[1]);
            if(connect[0] == val){
                play_index = current_decision.ranges[connect[1]][0];
                select_range = current_decision.ranges[connect[1]][1];
                Debug.Log("Read ranges item " + connect[1] + "  val-> " + current_decision.ranges[connect[1]][0]);
                Debug.Log("set index to " + play_index);
                break;
            }
        }
        wait2 = false;
    }
    public List<StoryItem> loadStory(string text){
        List<StoryItem> stories = new List<StoryItem>();
        int count = 0;
        int te = 0;
        foreach(string line in text.Split("\n")){
            try{
                if(line.StartsWith("[name")){
                    string name = ReMatch(line,"name=(.*?)]");
                    name = name.Split("=\"")[1].Split("\"]")[0];
                    string story_text = line.Split("]")[1];
                    StoryItem storyItem = new StoryItem
                    {
                        char_name = name,
                        char_text = story_text,
                        Type = StoryItem.ActionType.Dialog
                    };
                    stories.Add(storyItem);
                }else if(line.StartsWith("[Background")){
                    string imagePath = ReMatch(line,"image=\"(.*?)\"");
                    imagePath = "story_src/Avg_bg_"+imagePath.Split("\"")[1];
                    StoryItem storyItem = new StoryItem
                    {
                        image_path = imagePath,
                        Type = StoryItem.ActionType.Background
                    };
                    stories.Add(storyItem);
                }else if(line.StartsWith("[Character")){
                    List<string> chars = new List<string>();
                    string name = ReMatch(line,"name=\"(.*?)\"");
                    name = name.Split("\"")[1];
                    chars.Add("story_src/Avg_"+name.Split("#")[0]);
                    if(line.IndexOf("name2") != -1){
                        string char2 = ReMatch(line,"name2=\"(.*?)\"");
                        char2 = char2.Split("\"")[1];
                        chars.Add("story_src/Avg_"+char2.Split("#")[0]);
                    }
                    int foc = 0;
                    if(line.IndexOf("focus") != -1){
                        string focus = ReMatch(line,"focus=[0-9]+");
                        focus = focus.Split("=")[1];
                        foc = int.Parse(focus);
                    }
                    StoryItem storyItem = new StoryItem
                    {
                        char_images = chars,
                        focus = foc,
                        Type = StoryItem.ActionType.Character
                    };
                    stories.Add(storyItem);
                }else if(line.IndexOf("[Image]") != -1){
                    StoryItem storyItem = new StoryItem
                    {
                        Type = StoryItem.ActionType.HideImage
                    };
                    stories.Add(storyItem);
                }else if(line.StartsWith("[Image(")){
                    if(line.IndexOf("(fadetime") == -1){
                        string image_patha = ReMatch(line,"image=\"(.*?)\"");
                        image_patha = "story_src/Avg_"+image_patha.Split("\"")[1];
                        float x = float.Parse(ReMatch(line,"x=[0-9.-]+").Split('=')[1]);
                        float y = float.Parse(ReMatch(line,"y=[0-9.-]+").Split('=')[1]);
                        float xScale = float.Parse(ReMatch(line,"xScale=[0-9.-]+").Split('=')[1]);
                        float yScale = float.Parse(ReMatch(line,"yScale=[0-9.-]+").Split('=')[1]);
                        StoryItem storyItem = new StoryItem
                        {
                            img_path = image_patha,
                            ip =  new Vector3(x,y,0),
                            isca = new Vector3(xScale,yScale,1),
                            Type = StoryItem.ActionType.Image
                        };
                        stories.Add(storyItem);
                    }else{
                        float fadetime = float.Parse(ReMatch(line,"fadetime=[0-9.-]+").Split('=')[1]);
                        StoryItem storyItem = new StoryItem
                        {
                            fadeDurtation = fadetime,
                            Type = StoryItem.ActionType.ImageFade
                        };
                        stories.Add(storyItem);
                    }
                }else if(line.StartsWith("[ImageTween(")){
                    float xFrom = float.Parse(ReMatch(line,"xFrom=[0-9.-]+").Split('=')[1]);
                    float xTo = float.Parse(ReMatch(line,"xTo=[0-9.-]+").Split('=')[1]);
                    float yFrom = float.Parse(ReMatch(line,"yFrom=[0-9.-]+").Split('=')[1]);
                    float yTo = float.Parse(ReMatch(line,"yTo=[0-9.-]+").Split('=')[1]);
                    float xScaleFrom = float.Parse(ReMatch(line,"xScaleFrom=[0-9.-]+").Split('=')[1]);
                    float yScaleFrom = float.Parse(ReMatch(line,"yScaleFrom=[0-9.-]+").Split('=')[1]);
                    float xScaleTo = float.Parse(ReMatch(line,"xScaleTo=[0-9.-]+").Split('=')[1]);
                    float yScaleTo = float.Parse(ReMatch(line,"yScaleTo=[0-9.-]+").Split('=')[1]);
                    float durtation = float.Parse(ReMatch(line,"duration=[0-9.-]+").Split('=')[1]);
                    StoryItem storyItem = new StoryItem
                    {
                        pFrom = new Vector3(xFrom,yFrom,0),
                        pTo = new Vector3(xTo,yTo,0),
                        sFrom = new Vector3(xScaleFrom,yScaleFrom,1),
                        sTo = new Vector3(xScaleTo,yScaleTo,1),
                        durtation = durtation,
                        Type = StoryItem.ActionType.ImageTween
                    };
                    stories.Add(storyItem);
                }else if(line.StartsWith("[Decision")){
                    Debug.Log("Find Decision");
                    List<string> options = new List<string>();
                    if(ReMatch(line,"options=\"(.*?)\"").IndexOf(";") != -1){
                        options = new List<string>(ReMatch(line,"options=\"(.*?)\"").Split("\"")[1].Split(";"));
                    }else{
                        options = new List<string>
                        {
                            ReMatch(line,"options=\"(.*?)\"").Split("\"")[1]
                        };
                    }
                    string values = ReMatch(line,"values=\"(.*?)\"").Split("\"")[1];
                    string[] lines = text.Split("\n");
                    int temp = 0;
                    int si = -1;
                    int ei = -1;
                    int st = 0;
                    int end = -1;
                    List<List<int>> reaction = new List<List<int>>();
                    List<List<int>> ranges = new List<List<int>>();
                    for(int i=te+1;i<lines.Length;i++){
                        Debug.Log("Read line -> " + lines[i]);
                        if(lines[i].StartsWith("[name") ||
                           lines[i].StartsWith("[Character") || 
                           lines[i].StartsWith("[Image(") || 
                           lines[i].StartsWith("[ImageTween(")){
                            Debug.Log("line is available ");
                            temp++;
                        }
                        if(lines[i].StartsWith("[Predicate")){
                            string value = ReMatch(lines[i],"references=\"(.*?)\"").Split("\"")[1];
                            Debug.Log("Predicate -> " + value + " " + values);
                            if(value != values){
                                List<int> r = new List<int>();
                                foreach(string rea in value.Split(";")){
                                    r.Add(int.Parse(rea));
                                    r.Add(st);
                                    Debug.Log("Connect choice " + rea + " -> " + st);
                                }
                                reaction.Add(r);
                            }
                            if(si == -1){
                                si = count + temp + 1;
                            }else{
                                ei = count + temp;
                                Debug.Log("Find Predicate Range -> " + si +' ' + ei);
                                List<int> range = new List<int>
                                {
                                    si,
                                    ei
                                };
                                ranges.Add(range);
                                si = ei+1;
                            }
                            if(value == values){
                                Debug.Log("Find end point ->" + (count+temp+1));
                                end = count+temp+1;
                                break;
                            }
                            st++;
                        }
                    }
                    StoryItem storyItem = new StoryItem
                    {
                        Type = StoryItem.ActionType.Decision,
                        options = options,
                        ranges = ranges,
                        reactions = reaction,
                        end_index = end
                    };
                    stories.Add(storyItem);
                }else{
                    count--;
                    Debug.Log("Cannot Read Line - "+line);
                }
                count++;
                te++;
            }catch(Exception e){
                Debug.Log("Parse line error -> "+ line + "\n" + e.ToString());
            }
        }
        return stories;
    }
    public string ReMatch(string text,string reg){
        Regex regex = new Regex(reg);
        MatchCollection mat = regex.Matches(text);
        try{
            return mat[0].ToString();
        }catch{
            Debug.Log("Match string error");
            return "err=1";
        }
    }
    public string ReadData(string path){
        string readData;
        string fileUrl = path;
        using (StreamReader sr = File.OpenText(fileUrl)){
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return readData;
    }
} 