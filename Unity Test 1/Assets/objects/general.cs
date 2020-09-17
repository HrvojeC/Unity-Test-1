using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RobotData
{
    public Transform myTransform;   //brzi link na Transform
    public Rigidbody2D myRigidbody; //brzi link na Rigidbody
    public Animator myAnimator;     //brzi link na Animator

    public Transform selection;    //ovdje spremamo gameobject kojeg robot fokusira - može biti box ili container (promjenjeno u Transform jer su nepotrebni ostali GameObject elementi)

    private bool hasBox = false;    //ako robot ima box u rukama ovo je true - odnesi ga u pripadajući container
    public float speed = 1.0f;

    public bool GetHasBox()
    {
        return hasBox;
    }

    public void SetHasBox(bool set)
    {
        hasBox = set;
        myAnimator.SetBool("HasBox", set);
    }
}

public static class RobotUtils
{ 
    public static void Walk(RobotData thisRobot)
    {
        //robot hoda prema selectionu, prvo odredi smjer kretanja i dodaj speed
        float movDirection = thisRobot.selection.transform.position.x - thisRobot.myTransform.position.x;
        movDirection = movDirection / Mathf.Abs(movDirection);
        thisRobot.myRigidbody.AddForce(new Vector2(movDirection, 0) * thisRobot.speed);
        thisRobot.myAnimator.SetBool("Walk", true);
    }

    public static Transform GetSelectionBox()
    {
        //od svih kutija biramo random kutiju koja nije robotu izvan dometa
        List<Transform> allAvailableObjects = new List<Transform>();
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.position.y < 2.0f) allAvailableObjects.Add(obj.transform);
        }
        if (allAvailableObjects.Count > 0)
            return allAvailableObjects[Random.Range(0, allAvailableObjects.Count)];
        //ako u sceni nema boxova u dohvatu robota
        Debug.Log("There is no reachable box in the scene");
        return null;
    }

    public static Transform GetSelectionContainer(RobotData thisRobot)
    {
        //hardcode-ano traženje ispravnog containera za selectani box - moglo bi ići napredno traženje ali nije bitno za ove dvije vrste box-ova
        if (thisRobot.myTransform.Find("positionBox").GetChild(0).name == "Box_Red") return GameObject.Find("Container_Red").transform;
        else if (thisRobot.myTransform.Find("positionBox").GetChild(0).name == "Box_Blue") return GameObject.Find("Container_Blue").transform;
        //ako je robot primio neispravni objekt
        Debug.Log("There is no container for selected object");
        return null;
    }

    public static void Box_Pickup(RobotData thisRobot)
    {
        //robot uzima selectirani box na sebe:
        //isključi od sada nepotrebne kompnente boxa i ispravno postavi box gameobjekt 
        Object.Destroy(thisRobot.selection.GetComponent<BoxCollider2D>());
        Object.Destroy(thisRobot.selection.GetComponent<Rigidbody2D>());
        Transform positionBox = thisRobot.myTransform.Find("positionBox");
        thisRobot.selection.SetParent(positionBox);
        thisRobot.selection.position = new Vector3(positionBox.position.x, positionBox.position.y, -1.1f);
        //stopiraj robota i postavi mu parametre
        thisRobot.myRigidbody.velocity = Vector2.zero;
        thisRobot.myAnimator.SetBool("Walk", false);
        thisRobot.SetHasBox(true);
        thisRobot.selection = null;
    }

    public static void Box_Store(RobotData thisRobot)
    {
        //robot sprema selectirani box u container:
        //uništi box
        Object.Destroy(thisRobot.myTransform.Find("positionBox").GetChild(0).gameObject);
        //stopiraj robota i postavi mu parametre
        thisRobot.myRigidbody.velocity = Vector2.zero;
        thisRobot.myAnimator.SetBool("Walk", false);
        thisRobot.SetHasBox(false);
        thisRobot.selection = null;
    }
}