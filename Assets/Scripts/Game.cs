using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Game : MonoBehaviour
{
    public int levelNumber;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI infoLevelText;
    public GameObject laver;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject win_sound;
    public GameObject mistake_sound;
    public TextMeshProUGUI buttonText;

    protected float timer = 0;
    protected float rot;
    protected int timeEndRemember = 10;
    protected int timeStartPlay = 11;
    protected int timeEndGame = 30;

    public void exitGame()
    {
        Application.Quit();
    }

    public virtual void nextGame()
    {

    }

    public void activationDeactivationHand()
    {
        var left = leftHand.GetComponent<Collider>();
        var right = rightHand.GetComponent<Collider>();
        left.enabled = !left.enabled;
        right.enabled = !right.enabled;
    }

    public void updateTimer()
    {
        timer += Time.deltaTime;
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        Debug.Log(timer);
    }

    public virtual int selectLevel()
    {
        rot = laver.transform.rotation.z;
        int level = 3;

        return level;
    }
    public virtual void infoLevel()
    {
        int level = selectLevel();
    }
    public virtual void GameMenager()
    {

    }
}
