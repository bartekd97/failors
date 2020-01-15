using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void CloseApp()
    {
        Application.Quit();
    }

    public void SetAnimBoolShowTrue(Animator anim)
    {
        anim.SetBool("Show", true);
    }

    public void SetAnimBoolShowFalse(Animator anim)
    {
        anim.SetBool("Show", false);
    }

    public void CheckIfMainMenuShouldBeActivated(Animator mainMenu)
    {
        if (!GameManager.instance.GameplayActive)
            mainMenu.SetBool("Show", true);
    }

    public void CheckIfGameOverShouldBeDeactivated(Animator gameOverMenu)
    {
        if (GameManager.instance.GameOverMenuActive)
            gameOverMenu.SetBool("Show", true);
    }
}
