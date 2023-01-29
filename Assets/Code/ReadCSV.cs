using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    public TextAsset CSVFile;


    // Start is called before the first frame update
    void Start()
    {
        string[] strLines = CSVFile.text.Split(Environment.NewLine);
        //TextArray = CSVFile.text.Split(';');

        string[] Row = strLines[0].Split(';');

        string[][] TextArray = new string[strLines.Length][Row.Length];

        

        for(int i = 0; i < strLines.Length; i++)
        {
            Debug.Log(strLines[i]);
        }


        //Debug.Log(CSVFile.text);
        //Debug.Log(TextArray[1]);





    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
