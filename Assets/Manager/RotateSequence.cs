using System.Collections.Generic;
using Holoville.HOTween;
using LitJson;
using UnityEngine;

/// <summary>
/// 旋转
/// </summary>
public class RotateSequence:MonoBehaviour
{
    //定义一个List表存放场景中的Item的信息
    public List<Transform> gameList = new List<Transform>();

    //存放位置信息
    private List<Vector3> vpos = new List<Vector3>();

    //存放相对偏移量
    private List<Sequence> mySequences = new List<Sequence>();

    //首先读取配置文件
    private void InitSequence()
    {
        InitSequencePos();
        RotateData();
    }

    /// <summary>
    /// 初始化位置
    /// </summary>
    private void InitSequencePos()
    {
        JsonData[] data = JsonMapper.ToObject<JsonData[]>(PreLoadConfig.text);
        vpos.Clear();

        //将数据取出放到vpos中
        int j = 0;
        foreach (JsonData jd in data)
        {
            Vector3 vec = Util.StrToVector3(jd["pos_" + j.ToString()].ToString(), ',');
            vpos.Add(vec);
            j++;
        }
    }

    private void RotateData()
    {
        for (int i = 0; i < gameList.Count; i++)
        {
            //每一个Item对应一个序列，此序列是个无限循环序列，从开始位置循环
            Sequence sequence = new Sequence(new SequenceParms().Loops( 1000,LoopType.Restart));
            mySequences.Add(sequence);
        }
         
        for (int i = 0; i < mySequences.Count; i++)
        {
            for (int j = 0; j < gameList.Count - i; j++)
            {
                mySequences[i].Append(HOTween.To(gameList[i], 1, new TweenParms().Prop("position", vpos[j + i])));
            }

            if (i > 0)
            {
                for (int j = 0; j < i; j++)
                {
                    mySequences[i].Append(HOTween.To(gameList[i], 1, new TweenParms().Prop("position", vpos[j])));                  
                }
            }
            mySequences[i].Append(HOTween.To(gameList[i], 1, new TweenParms().Prop("position", vpos[i])));

        }
        
    }

    /// <summary>
    /// 停止旋转
    /// </summary>
    private void PauseRotate()
    {
        for (int i = 0; i < mySequences.Count; i++)
        {
            mySequences[i].Pause();
        }
    }

    void Start()
    {
        InitSequence();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            PauseRotate();
        }
#else
        if (Input.touchCount > 0)
        {  
            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                PauseRotate();
            }
        }
#endif
    }

    /// <summary>
    /// 使之旋转
    /// </summary>
    void OnDrag(Vector2 data)
    {
        for (int i = 0; i < mySequences.Count; i++)
        {
            if (data.x > 0)
            {
                mySequences[i].PlayBackwards();
                mySequences[i].timeScale = 2.5f;
            }
            else if(data.x < 0)
            {
                mySequences[i].PlayForward();
                mySequences[i].timeScale = 2.5f;
            }
        }
    }
}