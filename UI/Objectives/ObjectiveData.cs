using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveData
{
    public string[] Data =
    {
        //************** LEVEL 1 ********************
        //Main Objectives
        //0 - find old man - Page 1
        "First I will need to find a vehicle to get out of here, better off asking around the neighbourhood.",
        
        //1 - Find meds - Page 1
        "<s>First I will need to find a vehicle to get out of here, better off asking around the neighbourhood.</s>"
        + "\n\n" +
        "The old man who lives alone said I could take his car, provided I get him a box of meds for his asthama, he said" +
        " I should be looking out for carboard boxes in the Mall or supermarket.",

        //2 Got the meds - Page 2
        "I got the meds that the Old Man asked for, now I can trade this for this car and get outta here!",

        //3 Got the keys - Page 2
        "<s>I got the meds that the Old Man asked for, now I can trade this for this car and get outta here!</s>"
        + "\n\n" +
        "Trade Successfull! Time to get out of here.",


        //Side Objective
        // 4 - Try getting guns - Page 1
        "I know I have will have to deal with zombies along the way, maybe I should try searching something that could help make things easier.",
        // 5 - Talk to SuperMarketOwner - Page 1
        "<s>I know I have will have to deal with zombies along the way, maybe I should try searching something that could help make things easier.</s>"
        + "\n\n" +
            "The SuperMarket Owner didn't want to give me his vehicle, but he said he would give me something useful for my journey, if I deliver him " +
            "an important package located at the wharehouse of his supermarket. I should give it a look when I have time."
    };

    public void StrikeOutPreviousObjective(int index)
    {
        if (!Data[index - 1].Contains("<s>")) return; //check if string does not contain partially striked out strings

        string[] stringParts = Data[index - 1].Split('\n');
        string newString = "";

        for (int i = 0; i < stringParts.Length; i++)
        {
            if(!stringParts[i].Contains("<s>"))
            {
                stringParts[i] = "<s>" + stringParts + "</s>";
            }

            newString += stringParts[i];
            newString += '\n';
        }

        //Recovering string
        Data[index - 1] = newString;
    }
}
