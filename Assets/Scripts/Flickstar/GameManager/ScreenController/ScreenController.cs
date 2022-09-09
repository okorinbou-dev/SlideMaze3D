using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{

    List<ScreenController> listConnectScreen = new List<ScreenController>();
    private int changeScreen = 0;
    public bool changeScreenFlag = false;

    public ScreenController() { 
        changeScreenFlag = false;
    }

    public virtual void Initialize() {
        changeScreenFlag = false;
	}

    public virtual void Update() {
	}

    public virtual bool isChangeScreen() {
		return changeScreenFlag;
	}

    public virtual void addChangeScreen( ScreenController scene ) {
		listConnectScreen.Add(scene);
	}

    public virtual void setChangeScreen( int no ) {
		changeScreen = no;
		changeScreenFlag = true;
	}

    public virtual ScreenController getChangeScreen() {
        changeScreenFlag = false;
		return listConnectScreen [changeScreen];
	}

}
