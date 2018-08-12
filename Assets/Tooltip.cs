using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Hide();
	}

    public bool isShowing = false;

	// Update is called once per frame
	void Update () {
        if(PauseManager.isPaused)
        {
            Hide();
            return;
        }

        RectTransform rt = GetComponent<RectTransform>();
        Vector3 globalMousePos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponentInParent<Canvas>().GetComponent<RectTransform>(),
            Input.mousePosition, Camera.main, out globalMousePos))
        {
            Vector3 offset = new Vector3(15, 5, 0);

            if (globalMousePos.x  > Screen.width * 0.50)
            {
                offset.x = -rt.sizeDelta.x - 15;
            }
            if (globalMousePos.y < Screen.height * 0.50)
            {
                offset.y = rt.sizeDelta.y;
            }
            

            rt.position = globalMousePos +
                (GetComponentInParent<Canvas>().scaleFactor * offset);
        }
    }

    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        isShowing = true;
    }

    public void Hide()
    {
        isShowing = false;
        GetComponent<CanvasGroup>().alpha = 0;
    }
}
