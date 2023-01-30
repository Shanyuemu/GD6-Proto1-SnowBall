using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public enum Types {None = 0, Snow = 1, Rock = 2, Wall = 3, Snowman = 4}; //, Horizontal = 4};

    int level_length = 100;
    int row_count = 5;
    int level_progress = 0;

    [SerializeField] GameObject snow;
    [SerializeField] GameObject rock;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject snowman;
//    [SerializeField] GameObject horizontal;

    [Space(15)]

    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] PlayerMove player;
    
    [Space(10)]

    public TextAsset CSVFile;
    public string[,] TextArray;
    public int [,] level_array;

    GameObject g_obj;
    ScrollingObject so;
    GameObject snowman_inst;

    float t_counter = 0;
    public static bool game_running = true;
    bool spawnedSnowman = false;

    Types[] prev_col = {Types.None, Types.None, Types.None, Types.None, Types.None};
    Types[] current_col = {Types.None, Types.None, Types.None, Types.None, Types.None};

    void Start()
    {
    }
    
    void Awake()
    {
        game_running = true;
        spawnedSnowman = false;
        level_progress = 0;
        readLevelFile();
    }

    public void restart()
    {
        game_running = true;
        spawnedSnowman = false;
        level_progress = 0;
        
        if(snowman_inst != null) Destroy(snowman_inst);

        player.gameObject.SetActive(true);
        player.enabled = true;
        player.restart();
    }

    void Update()
    {
        t_counter += Time.deltaTime;
        if(t_counter >= ScrollingObject.t_interval)
        {
            t_counter = 0;
            if(soundPlayer != null) soundPlayer.tick();
            spawn();
        }
    }

    
    public bool gameRunning()
    {
        return game_running;
    }

    public void gameOver(bool win = false)
    {
        game_running = false;

        if(soundPlayer != null) 
        {
            if(win)
                soundPlayer.victory();
            else
                soundPlayer.gameOver();
        }
        
        gameObject.SetActive(false);
    }

    void readLevelFile()
    {
        string[] strLines = CSVFile.text.Split(Environment.NewLine);    //row count
        string[] Row = strLines[0].Split(',');                          //level length
    
        TextArray = new string[strLines.Length, Row.Length];            //row count, level length

        level_length = Row.Length - 1;
        row_count = strLines.Length - 1;

        Debug.Log("level length: " + level_length);
        Debug.Log("row count: " + row_count);

        for(int i = 0; i < strLines.Length; i++)        //0-5
        {
            string[] TempRow = strLines[i].Split(',');
            
            for(int k = 0; k < TempRow.Length; k++)     //0- level_length
            {
                TextArray[i, k] = TempRow[k];           //(row x column) = value
            }
        }
    }


    void spawn()    //random
    {
        if(spawnedSnowman || !game_running) return;

        level_progress++;
        string[] col_s = {"", "", "", "", ""};
        
        //--------Snowman--------
            if(level_progress > level_length) 
            {
                if(!spawnedSnowman && (level_progress >= level_length + 8))
                {
                    if(snowman_inst != null) Destroy(snowman_inst);
                    snowman_inst = Instantiate(snowman);
                    
                    so = snowman_inst.GetComponent<ScrollingObject>();
                    if(so != null) so.spawn(2, player);

                    spawnedSnowman = true;
                }
                return;   
            }
        //----------------------
        
        Types t = Types.None;

        //row 0-4
        //column level_progress
        for(int i=0; i<5; i++)  //for all 5 rows...generate random objects
        {
            string s = TextArray[i+1, level_progress];
            col_s[i] = s;   //create column
            
            if(s == "0") t = Types.None;
            else if(s == "1") t = Types.Snow;
            else if(s == "2") t = Types.Rock;
            else if(s == "3") t = Types.Wall;
            else Debug.Log("s = " + s);

            switch (t)
            {
                case Types.None:
                break;
                case Types.Snow: //snow
                    g_obj = Instantiate(snow);
                break;
                case Types.Rock: //rock
                    g_obj = Instantiate(rock);
                break;
                case Types.Wall: //wall
                    g_obj = Instantiate(wall);
                break;
            }
        
            //spawn object
            if(t != Types.None && g_obj != null)
            {
                g_obj.transform.parent = this.transform;
                so = g_obj.GetComponent<ScrollingObject>();
                if(so != null) so.spawn(i, player);
            }
        }

        Debug.Log( "col[" + level_progress + "] = {" + col_s[0] + ", " + col_s[1] + ", " + col_s[2] + ", " + col_s[3] + ", " + col_s[4] + "}" );
    }
}


/*
void spawn()    //random
    {
        if(spawnedSnowman || !game_running) return;

        level_progress++;
        
        if(level_progress >= level_length) 
        {
            if(!spawnedSnowman && (level_progress >= level_length + 8))
            {
                if(snowman_inst != null) Destroy(snowman_inst);
                snowman_inst = Instantiate(snowman);
                
                so = snowman_inst.GetComponent<ScrollingObject>();
                if(so != null) so.spawn(2, player);

                spawnedSnowman = true;
            }

            //gameOver(true); //victory
            return;   
        }

        Types t = Types.None;
        float r = 0.0f;

        int snow_count = 0;
        int rock_count = 0;     //0-(3 - wall_count)
        int wall_count = 0;     //0-(4 - rock_count)
        int horizontal_count = 0;
 
        //player.unblockRows();

        for(int i=0; i<5; i++)  //for all 5 rows...generate random objects
        {
            float min = 0.0f;                   //probability (wall : rock : snow : nothing -> 2:3:5:10)

            //if (horizontal_count > 2 || i>4)    //no more than 3 horizontal platforms and do not spawn one on the 5th row
            //    min = 0.3f;
            if (wall_count + rock_count + horizontal_count > 3) 
                min = 0.5f;                     //no more than 4 rocks and walls combined
            else if (rock_count > 2) 
                min = 0.75f;                    //no more than 3 rocks total
            else if ((snow_count + wall_count + rock_count + horizontal_count == 4) || prev_col[i] != Types.None) 
                min = 1.0f;                     //at least one empty space, and an empty space after each object

            r = Random.Range(min, 2.0f);
            if(r < 0.2f)        //0.0f <= r < = 0.15f
                t = Types.Wall;
            else if(r < 0.5f)   //0.3f <= r < = 0.5f
                t = Types.Rock;
            else if(r < 1.0f)   //0.5f <= r < = 1.0f
                t = Types.Snow;
            else                //1.0f <= r < = 2.0f
                t = Types.None;
            
            current_col[i] = t;

            switch (t)
            {
                case Types.None:
                    //nothing
                break;
                case Types.Snow: //snow
                    snow_count++;
                    g_obj = Instantiate(snow);
                break;
                case Types.Rock: //rock
                    rock_count++;
                    g_obj = Instantiate(rock);
                break;
                case Types.Wall: //wall
                    wall_count++;
                    g_obj = Instantiate(wall);
                break;
                //case Types.Horizontal: //horizontal wall
                //    horizontal_count++;
                //    g_obj = Instantiate(horizontal);
                //break;
            }
            
            if(t != Types.None && g_obj != null)
            {
                g_obj.transform.parent = this.transform;
                so = g_obj.GetComponent<ScrollingObject>();
                if(so != null) so.spawn(i, player);
                else Debug.Log("scrolling object missing");
            }
            prev_col[i] = current_col[i];
        }

        //Debug.Log("Spawn Column: " + current_col[0] + ", " + current_col[1] + ", " + current_col[2] + ", " + current_col[3] + ", " + current_col[4]);
        //Debug.Log("Spawn! snow: " + snow_count + ", rock: " + rock_count + ", wall: " + wall_count + "\n;");
    }

*/