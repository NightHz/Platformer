#pragma warning disable 0649 // never assigned and always default value null
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    static DebugConsole Inst = null;
    DebugConsole()
    {
        Inst = this;
    }

    [SerializeField] Text text;
    [SerializeField] [Range(1, 100)] uint bufferN = 10;
    Queue<string> strs = new Queue<string>();

    private void Awake()
    {
        if (text != null)
            text.text = "";
    }

    void UpdateLog()
    {
        string s = "";
        foreach (string str in strs)
        {
            s += "\n" + str;
        }
        if (text != null)
            text.text = s;
    }

    static public void Log(string str)
    {
        Debug.Log(str);
        if (Inst == null)
            return;
        Inst.strs.Enqueue(str);
        while (Inst.strs.Count > Inst.bufferN)
            Inst.strs.Dequeue();
        Inst.UpdateLog();
    }
}
