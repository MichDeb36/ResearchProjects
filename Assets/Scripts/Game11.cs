using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class Game11 : Game
{
    public enum Game_states
    {
        No_game,
        Remember_tower,
        Destruction_tower,
        Build_tower,
        End_game
    }

    public List<GameObject> figures = new List<GameObject>();
    public GameObject snapZones;

    private List<GameObject> gameFigures = new List<GameObject>();
    private List<GameObject> gameFiguresSort = new List<GameObject>();
    private GameObject snapZonesCopies;
    private Game_states game = Game_states.No_game;

    private const string NAME_NEXT_LEVEL = "Scenes/Game2/Game2_1.1";
    private const string NAME_RESTART_LEVEL = "Scenes/Game1/Game1_1.0";
    private readonly Vector3 SNAP_ZONES_POSITION = new Vector3(15.436f, -0.818f, 5.981f);
    private readonly Vector3 START_POSITION_TOWER = new Vector3(3.021f, 0.9372351f, 5.23f);
    private readonly Vector3 TOWER_POSITION_BULDING = new Vector3(3.3f, 0.9372351f, 4.6f);


    void Start()
    {
        levelNumber = 5;
        snapZonesCopies = Instantiate(snapZones, SNAP_ZONES_POSITION, Quaternion.identity);
        snapZonesCopies.SetActive(false);
    }
    public override int selectLevel()
    {
        rot = laver.transform.rotation.z;
        int level = 3;
        if (rot <= 0.5f && rot > 0.25f)
            level = 3;
        else if (rot <= 0.25f && rot >= 0)
            level = 5;
        else if (rot <= 0 && rot > -0.25f)
            level = 6;
        else if (rot <= -0.25f && rot >= -0.5f)
            level = 7;

        return level;
    }
    public override void infoLevel()
    {
        int level = selectLevel();
        if (level == 3)
        {
            infoLevelText.text = "£atwy";
        }
        else if (level == 5)
        {
            infoLevelText.text = "Normalny";
        }
        else if (level == 6)
        {
            infoLevelText.text = "Trudny";
        }
        else if (level == 7)
        {
            infoLevelText.text = "Ekstremalny";
        }
    }
    public override void GameMenager()
    {
        if (game == Game_states.No_game)
        {
            starGame1();
        }
        else if (game == Game_states.Build_tower)
        {
            checkTower();
        }
        else if (game == Game_states.End_game)
        {
            SceneManager.LoadScene(NAME_RESTART_LEVEL);
        }
    }

    public override void nextGame()
    {
        SceneManager.LoadScene(NAME_NEXT_LEVEL);
    }

    public void starGame1()
    {
        levelNumber = selectLevel();
        infoText.text = "Zapamietaj u≥oøenie wieøy\n masz 10s";
        activationDeactivationHand();
        if (game == 0)
        {
            game = Game_states.Remember_tower;
            float step = 0.1428f;
            List<int> listNumbers = new List<int>();
            int randNumber;
            for (int i = 0; i < levelNumber; i++)
            {
                do
                {
                    randNumber = Random.Range(0, figures.Count);
                } while (listNumbers.Contains(randNumber));
                listNumbers.Add(randNumber);
            }
            for (int i = 0; i < levelNumber; i++)
            {
                gameFigures.Add(Instantiate(figures[listNumbers[i]], new Vector3(START_POSITION_TOWER.x, START_POSITION_TOWER.y + i * step, START_POSITION_TOWER.z), Quaternion.identity));
            }
        }
    }

    public void destructionTower()
    {
        game = Game_states.Destruction_tower;
        activationDeactivationHand();
        sortGameFigures();
        snapZones.SetActive(false);
        float step = 0.15f;
        for (int i = 0; i < levelNumber; i++)
        {
            gameFiguresSort[i].transform.position = new Vector3(TOWER_POSITION_BULDING.x, TOWER_POSITION_BULDING.y, TOWER_POSITION_BULDING.z + i * step);
        }
    }

    public void sortGameFigures()
    {
        gameFiguresSort = gameFigures.OrderBy(tile => tile.name).ToList();
    }

    public void buildTower()
    {
        game = Game_states.Build_tower;
        infoText.text = "Zbuduj poprzedniπ wieøe\n masz 20s";
        buttonText.text = "Sprawdü";
        snapZonesCopies.SetActive(true);
    }

    public void checkTower()
    {
        game = Game_states.End_game;
        var win = win_sound.GetComponent<AudioSource>();
        var mistake = mistake_sound.GetComponent<AudioSource>();
        bool check = true;
        for (int i = 1; i < levelNumber; i++)
        {
            if (gameFigures[i - 1].transform.position.y >= gameFigures[i].transform.position.y)
            {
                infoText.text = "Koniec gry\nZla wieøa";
                mistake.Play();
                check = false;
                break;
            }
        }
        if (check == true)
        {
            infoText.text = "Koniec gry\nPoprawna wieøa";
            win.Play();
        }
        buttonText.text = "Restart";
    }

    // Update is called once per frame
    void Update()
    {
        
        if (game != Game_states.No_game && game != Game_states.End_game)
        {
            Debug.Log("jol");
            updateTimer();
            
        }
        int seconds = (int)(timer % 60);
        
        if (seconds >= timeEndRemember && game == Game_states.Remember_tower)
        {
            destructionTower();
        }
        if (seconds >= timeStartPlay && game == Game_states.Destruction_tower)
        {
            buildTower();
        }
        if (seconds >= timeEndGame && game == Game_states.Build_tower)
        {
            checkTower();
        }
    }
}
