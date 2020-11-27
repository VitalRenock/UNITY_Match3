using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandlerLevels : MonoBehaviour
{
    public void LoadLevel()
    {
        switch (gameObject.name)
        {
            case "Bruges": InfoManager._chooseALevel = 0; break;
            case "Bruxelles": InfoManager._chooseALevel = 1; break;
            case "Hasselt": InfoManager._chooseALevel = 2; break;
            case "Mons": InfoManager._chooseALevel = 3; break;
            case "Namur": InfoManager._chooseALevel = 4; break;
            case "Liège": InfoManager._chooseALevel = 5; break;
            case "Arlon": InfoManager._chooseALevel = 6; break;
            default:
                break;
        }

        SceneManager.LoadScene("3_Game");
    }
}
