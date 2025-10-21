using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenUIManger : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button StartButton;
    [SerializeField] Button ExitButton;
    [SerializeField] string FirstLevelName = "Level1";

    [Header("Panels")]
    [SerializeField] GameObject ButtonsPanels;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
        addButtonsLiseners();
        AssignNamedActionTransition();

    }

    private void addButtonsLiseners()
    {
        ExitButton.onClick.AddListener(() => Application.Quit());
        StartButton.onClick.AddListener(() => SceneManager.LoadScene(FirstLevelName));
    }

    private void AssignNamedActionTransition()
    {
        var transitions = FindObjectsByType<NamedActionTransition>(FindObjectsSortMode.None);
        var buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        foreach (var transition in transitions)
        {
            var selectedButton = buttons.FirstOrDefault(x => x.name.Equals(transition.actionName));
            if (selectedButton != null)
            {
                selectedButton.onClick.AddListener(transition.DoAction);
            }
        }
    }
}
