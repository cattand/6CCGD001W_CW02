using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	public List <GameObject> tankOptions;
    public int initialTankNo;
    private GameObject currentTank;
    public float rotateSpeed;
    public Text tankNameText;
    public Button [] ButtonList;
    public Transform[] attributePanel;

	// Use this for initialization
	void Start () {
        //initialTankNo = PlayerPrefs.GetInt("TankSelected");
        initialTankNo = tankOptions.Count;
        nextTank(true);
        rotateSpeed = 60.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                nextTank(true);
            }
            else
                nextTank(false);
        }

        if (currentTank != null)
        {
            currentTank.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
    }

    public void ButtonPress()
    {
        var number = int.Parse(EventSystem.current.currentSelectedGameObject.name);

        SetCurrentTank(number);
        initialTankNo = number;
    }

    public void GameStartButton()
    {
        PlayerPrefs.SetInt("TankSelected", initialTankNo);

        SceneManager.LoadScene(4);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

	private void DeativateAllTanks()
	{
		foreach(GameObject tank in tankOptions)
		{
			tank.gameObject.SetActive (false);
		}
        foreach (Button tankButton in ButtonList)
        {
            tankButton.image.color= Color.grey;
        }
	}

    private void SetCurrentTank(int currentTankNo)
    {
        Quaternion tempRotation = new Quaternion();

        if (currentTank != null)
        {
           tempRotation = currentTank.transform.rotation;
        }
        DeativateAllTanks();
        ButtonList[currentTankNo].image.color = Color.white;
        tankOptions[currentTankNo].SetActive(true);
        currentTank = tankOptions[currentTankNo];
        UpdateAttributePanel(currentTankNo);
        if (currentTank != null)
        {
            currentTank.transform.rotation = tempRotation;
        }
        tankNameText.text = currentTank.name;
    }

    public void nextTank(bool increase)
    {
        if (increase)
        {
            initialTankNo++;

            if (initialTankNo >= tankOptions.Count)
            {
                initialTankNo = 0;
            }
        }
        else
        {
            initialTankNo--;

            if (initialTankNo < 0)
            {
                initialTankNo = tankOptions.Count - 1;
            }
        }

            switch (initialTankNo)
        {
            case 0:
                SetCurrentTank(initialTankNo);
                break;

            case 1:
                SetCurrentTank(initialTankNo);
                break;

            case 2:
                SetCurrentTank(initialTankNo);
                break;

            case 3:
                SetCurrentTank(initialTankNo);
                break;

            case 4:
                SetCurrentTank(initialTankNo);
                break;

            case 5:
                SetCurrentTank(initialTankNo);
                break;
        }
    }

    private void UpdateAttributePanel(int currentTankNumber)
    {
        var currentTankAttribute = tankOptions[currentTankNumber].GetComponent<Tank>();

        //int power = currentTankAttribute.

        for (int x = 0; x < attributePanel.Length; x++)
        {
            //int totalChildren = attributePanel[x].childCount;

            int currentAttNo = 0;

            switch (x)
            {
                case 0:
                    currentAttNo = currentTankAttribute.startingHP / 50;
                    break;
                case 1:
                    currentAttNo = currentTankAttribute.armor;
                    break;
                case 2:
                    currentAttNo = currentTankAttribute.power;
                    break;
                case 3:
                    currentAttNo = currentTankAttribute.FiringSpeed;
                    break;
                case 4:
                    currentAttNo = currentTankAttribute.speed;
                    break;
            }

            foreach (Transform attribute in attributePanel[x])
            {
                attribute.GetComponent<RawImage>().color = Color.grey;
            }


            for (int y = 0; y < currentAttNo; y++)
            {
                attributePanel[x].GetChild(y).GetComponent<RawImage>().color = Color.green;
            }



        }
    }




}
