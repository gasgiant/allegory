using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public enum GameState{ Normal, InMenu }
    public GameState state;

    public RectTransform menuPanel;
    public RectTransform howtoPanel;
    public RectTransform thanksPanel;
    public Text scoreText;
    Vector3 menuPanelV;
    Vector3 howtoPanelV;
    Vector3 ingPanelV;
    bool showHowTo;
    public int score;

    public bool finished;


    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        MenuUpdate();
        scoreText.text = score + "/9";
    }

    public void SwitchState()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (state == GameState.Normal)
            state = GameState.InMenu;
        else
            state = GameState.Normal;
    }

    public void HowTo()
    {
        showHowTo = !showHowTo;
    }

    void MenuUpdate()
    {
        if (state == GameState.Normal)
        {
            menuPanel.localPosition = Vector3.SmoothDamp(menuPanel.localPosition, Vector3.up * 2080, ref menuPanelV, 0.3f);
            thanksPanel.localPosition = Vector3.SmoothDamp(thanksPanel.localPosition, Vector3.zero * 2080, ref ingPanelV, 0.3f);
            howtoPanel.localPosition = Vector3.SmoothDamp(howtoPanel.localPosition, Vector3.up * 2080 + Vector3.right * 1920, ref howtoPanelV, 0.3f);
        }
        else
        {

            if (showHowTo)
            {
                menuPanel.localPosition = Vector3.SmoothDamp(menuPanel.localPosition, Vector3.left * 1920, ref menuPanelV, 0.3f);
                howtoPanel.localPosition = Vector3.SmoothDamp(howtoPanel.localPosition, Vector3.zero, ref howtoPanelV, 0.3f);

            }
            else
            {
                menuPanel.localPosition = Vector3.SmoothDamp(menuPanel.localPosition, Vector3.zero, ref menuPanelV, 0.3f);
                howtoPanel.localPosition = Vector3.SmoothDamp(howtoPanel.localPosition, Vector3.right * 1920, ref howtoPanelV, 0.3f);

            }

        }

        if (finished)
            thanksPanel.localPosition = Vector3.SmoothDamp(thanksPanel.localPosition, Vector3.zero, ref ingPanelV, 0.3f);
        else
            thanksPanel.localPosition = Vector3.SmoothDamp(thanksPanel.localPosition, Vector3.up * 2080, ref ingPanelV, 0.3f);



    }

}
