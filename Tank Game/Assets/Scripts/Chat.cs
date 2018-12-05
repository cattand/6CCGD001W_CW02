using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
	[SerializeField]
	private Text container;
	[SerializeField]
	private ScrollRect rect;

    public GameObject textObject;
    public GameObject contentForText;

	internal void AddMessage(string message, Color playerColor)
	{
        //if (container.text == "")
        //{
        //    container.text += message;
        //    container.color = playerColor;
        //}
        //else
        //{
        //    container.text += "\n" + message;
        //    container.color = playerColor;
        //}

        GameObject textGO = Instantiate(textObject, contentForText.transform);
        textGO.SetActive(true);
        textGO.GetComponentInChildren<Text>().text = message;
        textGO.GetComponentInChildren<Text>().color = playerColor;

        //just a hack to jump a frame and scrolldown the chat
        Invoke("ScrollDown", .1f);
	}

	public virtual void SendMessage(InputField input)	{}

	private void ScrollDown()
	{
		if (rect != null)
			rect.verticalScrollbar.value = 0;
	}
}