using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot_main : MonoBehaviour
{
    private RobotData thisRobot = new RobotData();

    private void Start()
    {
        thisRobot.myTransform = this.transform;
        thisRobot.myRigidbody = transform.GetComponent<Rigidbody2D>();
        thisRobot.myAnimator = transform.GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!thisRobot.GetHasBox())  //nemamo box, idi po box
        {
            if (thisRobot.selection == null) thisRobot.selection = RobotUtils.GetSelectionBox();                                     //nemamo selectirani box, odaberi/selectiraj box
            else
            {
                if (Vector2.Distance(thisRobot.selection.transform.position, transform.position) > 0.85f) RobotUtils.Walk(thisRobot);//hodaj prema selectionu (boxu)
                else RobotUtils.Box_Pickup(thisRobot);                                                                               //pokupi box
            }
        }
        else                    //imamo box, nosi ga u container
        {               
            if (thisRobot.selection == null) thisRobot.selection = RobotUtils.GetSelectionContainer(thisRobot);                      //nemamo selectirani container, odredi/selectiraj container
            else
            {
                if (Vector2.Distance(thisRobot.selection.transform.position, transform.position) > 2.2f) RobotUtils.Walk(thisRobot); //hodaj prema selectionu (containeru)
                else RobotUtils.Box_Store(thisRobot);                                                                                //pospremi box
            }
        }
    }
}
