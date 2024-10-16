using System.Collections;
using System.Collections.Generic;
using ChineseChess;
using ChineseChess.Chesses;
using ChineseChess.Controller;
using ChineseChess.SO;
using ChineseChess.Tools;
using ChuuniExtension;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourMenu
{
    private const string MOVE_BUTTON_PATH = "Menu/MoveButton";
    private const string RAMPAGE_BUTTON_PATH = "Menu/RampageButton";
    private readonly Vector2 screenPositionOffset = new Vector2(140, 0);

    private IController controller;
    private GameObject view;
    private Button MoveButton;
    private Button RampageButton;

    public Button.ButtonClickedEvent MoveClicked => MoveButton.onClick;
    public Button.ButtonClickedEvent RampageClicked => RampageButton.onClick;

    public BehaviourMenu(HumanController controller){
        this.controller = controller;
        GameObject behaviourMenuPrefab = Resources.Load<PrefabsSettings>(ResourcesPath.PREFABS_SETTINGS)
                                                  .BehaviourMenu;
        view = Object.Instantiate(behaviourMenuPrefab, RootGroup.Object);
        MoveButton = view.transform.Find(MOVE_BUTTON_PATH).GetComponent<Button>();
        RampageButton = view.transform.Find(RAMPAGE_BUTTON_PATH).GetComponent<Button>();
        
        MoveButton.onClick.AddListener(controller.Enable);
        MoveButton.onClick.AddListener(CloseMenu);
        MoveButton.onClick.AddListener(controller.ToMoveMode);

        RampageButton.onClick.AddListener(controller.Enable);
        RampageButton.onClick.AddListener(CloseMenu);
        RampageButton.onClick.AddListener(controller.ToRampageMode);

        view.SetActive(false);
    }

    public void OpenMenu(Vector2 position){
        var menu = view.transform.Find("Menu");
        menu.position = position + screenPositionOffset;
        view.SetActive(true);
    }
    public void OpenMenu(ChessType type, Vector2 position){
        RampageButton.interactable = false;
        if(type == ChessType.砲 || type == ChessType.車){
            RampageButton.interactable = true;
        }
        OpenMenu(position);
    }
    public void CloseMenu() => view.SetActive(false);

}
