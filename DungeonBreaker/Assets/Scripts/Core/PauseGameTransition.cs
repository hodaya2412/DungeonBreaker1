//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//public class PauseGameTransition : TransitionBase
//{
//    InputActions inputActions;
//    bool menuPressed = false;
//    [SerializeField] string startSceneName = "StartScene";
//    protected override void Awake()
//    {
//        base.Awake();
//        inputActions = new();
//    }
//    private void OnEnable()
//    {
//        inputActions.Enable();
//        inputActions.Player.Menu.performed += OnMenuPressed;
//    }

//    private void OnMenuPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
//    {
//        menuPressed = true;
//    }

//    public override bool ShouldTransition()
//    {
//        if (startSceneName == SceneManager.GetActiveScene().name)
//        {
//            return false;
//        }
//        bool canTransition = menuPressed;
//        menuPressed = false;
//        return base.ShouldTransition() && canTransition;
//    }

//    private void OnDisable()
//    {
//        inputActions.Disable();
//        inputActions.Player.Menu.performed -= OnMenuPressed;
//    }
//}
