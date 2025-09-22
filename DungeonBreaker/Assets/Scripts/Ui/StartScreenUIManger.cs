using System;
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
        
    }

    private void addButtonsLiseners()
    {
        ExitButton.onClick.AddListener(() => Application.Quit());
        StartButton.onClick.AddListener(() => SceneManager.LoadScene(FirstLevelName));
    }

    void Update()
    {
        
    }
}
