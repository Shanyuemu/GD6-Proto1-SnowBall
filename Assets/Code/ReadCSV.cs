using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    public TextAsset CSVFile;

    public string[,] TextArray;
    

    // Start is called before the first frame update
    void Start()
    {

        string[] strLines = CSVFile.text.Split(Environment.NewLine);
        //TextArray = CSVFile.text.Split(';');

        string[] Row = strLines[0].Split(';');

        TextArray = new string[strLines.Length,Row.Length];

        

        for(int i = 0; i < strLines.Length; i++)
        {
            string[] TempRow = strLines[i].Split(';');
            
            for(int k = 0; k < TempRow.Length; k++)
            {
                TextArray[i, k] = TempRow[k];
                Debug.Log("Row" + i.ToString() + " = " + TextArray[i, k]);
            }

           
        }


    }

    // Update is called once per frame
    void Update()
    {
 


    }


}
