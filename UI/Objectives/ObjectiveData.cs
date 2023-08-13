using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveData
{
    public string[] Data =
    {
        //************** LEVEL 1 ********************
        //---- Main Objectives ----
        //0 - find old man - Page 1
        "First I will need to find a vehicle to get out of here, better off asking around the neighbourhood.",
        
        //1 - Find meds - Page 1
        "<s>First I will need to find a vehicle to get out of here, better off asking around the neighbourhood.</s>"
        + "\n\n" +
        "The old man who lives alone said I could take his car, provided I get him a box of meds for his asthama, he said" +
        " I should be looking out for carboard boxes in the Mall or supermarket.",

        //2 Got the meds - Page 2
        "I got the meds that the Old Man asked for, now I can trade this for his car keys and get outta here!",

        //3 Got the keys - Page 2
        "<s>I got the meds that the Old Man asked for, now I can trade this for his car keys and get outta here!</s>"
        + "\n\n" +
        "Trade Successfull! Time to get out of here.",


        //---- Side Objective ----
        // 4 - Try getting guns - Page 1
        "I know I have will have to deal with zombies along the way, maybe I should try searching something that could help make things easier.",
        
        // 5 - Talk to SuperMarketOwner - Page 1
        "<s>I know I have will have to deal with zombies along the way, maybe I should try searching something that could help make things easier.</s>"
        + "\n\n" +
            "The SuperMarket Owner didn't want to give me his vehicle, but he said he would give me something useful for my journey, if I deliver him " +
            "an important package located at the wharehouse of his supermarket. I should give it a look when I have time.",

        // 6 - Give the supplies back to Supermarket Owner - Page 2
        "Better give this supplies back to the supermarket owner so that I can get the key to the managers room.",

        // 7 - Visit manager room - Page 2
         "<s>Better give this supplies back to the supermarket owner so that I can get the key to the managers room.</s>"
            + "\n\n" +
        "Now let's see what awaits us in the Manager's room.",



        //************** LEVEL 2 ********************
        //---- Main Objectives ----
        // 8 - find food and water
        "I will need some food and drinking water in order to survive the night, hopefully there is some village or house nearby",

        // 9 - Find place to stay at the night
         "<s> I will need some food and drinking water in order to survive the night, hopefully there is some village or house nearby </s>" 
            + "\n\n" +
        "Its not safe to keep driving at the night, the odds are stacked against me. It would be nice to find a place to stay for the night.",

        // ---- Side Objectives
        // 10 - Get gun
        "As I try to find food and water, if I could get a gun along the way, that would be awesome.",

        // 11 - Got Gun
         "<s>As I try to find food and water, if I could get a gun along the way, that would be awesome.</s>"
         + "\n\n" +
           "With this shotgun, no zombie can stop me now!",


         //************* LEVEL 3 ********************
         //---- Main Objective ----
         // 12 - kill all enemies
         "The helicopter cannot land with all these zombies in the way, I will have to kill them all.",

         //13 Kill boss
         "<s>The helicopter cannot land with all these zombies in the way, I will have to kill them all.</s>"
            + "\n\n" +
         "The sound of the helicopter blades attracted this massive monster, I will have to do whatever it takes to kill this thing.",

          //14 Killed boss
         "<s>The helicopter cannot land with all these zombies in the way, I will have to kill them all.</s>"
            + "\n\n" +
         "<s>The sound of the helicopter blades attracted this massive monster, I will have to do whatever it takes to kill this thing.</s>"
            +"\n\n"+
            "Time to get on the helicopter and end this nightmare."

    };

   
}
