using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfoPanel : MonoBehaviour {

    public GameObject goToShow;

    public void ShowCurrentPanel()
    {
        goToShow.SetActive(true);
    }
}
