using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class Game33 : Game
{

    public enum Game_states
    {
        No_game,
        Remember_toys,
        Restart_toys,
        Setting_toys,
        End_game
    }

    public List<GameObject> toys = new List<GameObject>();

    private List<GameObject> gameToys = new List<GameObject>();
    private List<int> randListNumbers = new List<int>();
    private List<GameObject> addToys = new List<GameObject>();
    private List<GameObject> startToys = new List<GameObject>();
    private List<GameObject> gameToysSort = new List<GameObject>();
    private Game_states game = Game_states.No_game;

    private const string NAME_NEXT_LEVEL = "Scenes/Game1/Game1_1.0";
    private const string NAME_RESTART_LEVEL = "Scenes/Game3/Game3_1.0";

    private readonly Vector3 START_POSITION_TOYS = new Vector3(3.5f, 0.9372351f, 5.0f);
    private readonly Vector3 RESTART_POSITION_TOYS_LINE_1 = new Vector3(3.25f, 0.9372351f, 5.0f); 
    private readonly Vector3 RESTART_POSITION_TOYS_LINE_2 = new Vector3(3.75f, 0.9372351f, 5.0f);
    private readonly Vector3 CHECK_TOYS_POSITION_1 = new Vector3(2.964076f, 0.9544753f, 5.737184f);
    private readonly Vector3 CHECK_TOYS_POSITION_2 = new Vector3(2.964076f, 0.9544753f, 5.476884f);
    private readonly Vector3 CHECK_TOYS_POSITION_3 = new Vector3(2.964076f, 0.9544753f, 5.222184f);


    public override int selectLevel()
    {
        float rotLaver = laver.transform.rotation.z;
        int level = 5;
        if (rotLaver <= 0.5f && rotLaver > 0.25f)
            level = 5;
        else if (rotLaver <= 0.25f && rotLaver >= 0)
            level = 6;
        else if (rotLaver <= 0 && rotLaver > -0.25f)
            level = 7;
        else if (rotLaver <= -0.25f && rotLaver >= -0.5f)
            level = 9;

        return level;
    }
    public override void infoLevel()
    {
        int level = selectLevel();
        if (level == 5)
        {
            infoLevelText.text = "?atwy";
        }
        else if (level == 6)
        {
            infoLevelText.text = "Normalny";
        }
        else if (level == 7)
        {
            infoLevelText.text = "Trudny";
        }
        else if (level == 9)
        {
            infoLevelText.text = "Ekstremalny";
        }
    }
    public void GameMenager()
    {
        if (game == Game_states.No_game)
        {
            starGame3();
        }
        else if (game == Game_states.Setting_toys)
        {
            checkToys();
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


    public void starGame3()
    {
        levelNumber = selectLevel();
        infoText.text = "Zapamietaj przedmioty\n na stole masz 10s";
        activationDeactivationHand();
        if (game == 0)
        {
            game = Game_states.Remember_toys;
            float Xpos = START_POSITION_TOYS.x;
            float Zpos = START_POSITION_TOYS.z;
            float Ypos = START_POSITION_TOYS.y;
            float step = 0.2f;
            GameObject newToy;
            int randNumber;
            for (int i = 0; i < levelNumber; i++)
            {
                do
                {
                    randNumber = Random.Range(0, toys.Count);
                } while (randListNumbers.Contains(randNumber));
                randListNumbers.Add(randNumber);
            }
            for (int i = 0; i < levelNumber; i++)
            {
                newToy = Instantiate(toys[randListNumbers[i]], new Vector3(Xpos, Ypos, Zpos + i * step), Quaternion.identity);
                gameToys.Add(newToy);
                startToys.Add(newToy);
            }
        }
    }

    public void restartToys()
    {
        game = Game_states.Restart_toys;
        activationDeactivationHand();
        GameObject newToy;
        int randNumber;
        for (int i = 0; i < levelNumber / 3; i++)
        {
            do
            {
                randNumber = Random.Range(0, toys.Count);
            } while (randListNumbers.Contains(randNumber));
            randListNumbers.Add(randNumber);
            newToy = Instantiate(toys[randListNumbers[randListNumbers.Count - 1]], new Vector3(0, 0, 0), Quaternion.identity);
            gameToys.Add(newToy);
            addToys.Add(newToy);
        }
        sortGameFigures();
        float Xpos = RESTART_POSITION_TOYS_LINE_1.x;
        float Zpos = RESTART_POSITION_TOYS_LINE_1.z;
        float Ypos = RESTART_POSITION_TOYS_LINE_1.y;
        float step = 0.25f;
        int counter = 0;
        int halfNumberToysSort = gameToysSort.Count / 2;
        for (int i = 0; i < halfNumberToysSort; i++)
        {
            gameToysSort[i].transform.position = new Vector3(Xpos, Ypos, Zpos + counter * step);
            counter++;
        }
        Xpos = RESTART_POSITION_TOYS_LINE_2.x;
        Zpos = RESTART_POSITION_TOYS_LINE_2.z;
        counter = 0;
        for (int i = halfNumberToysSort; i < gameToysSort.Count; i++)
        {
            gameToysSort[i].transform.position = new Vector3(Xpos, Ypos, Zpos + counter * step);
            counter++;
        }
    }

    public void sortGameFigures()
    {
        gameToysSort = gameToys.OrderBy(tile => tile.name).ToList();
    }

    public void settingToys()
    {
        game = Game_states.Setting_toys;
        infoText.text = "Znajd? dodane przedmioty\n i ustaw je w wyznaczonym miejscu masz 20s";
        buttonText.text = "Sprawd?";
    }

    public void checkToys()
    {
        game = Game_states.End_game;
        var win = win_sound.GetComponent<AudioSource>();
        var mistake = mistake_sound.GetComponent<AudioSource>();
        bool check = true;
        Vector3 AddToy1 = new Vector3(CHECK_TOYS_POSITION_1.x, CHECK_TOYS_POSITION_1.y, CHECK_TOYS_POSITION_1.z);
        Vector3 AddToy2 = new Vector3(CHECK_TOYS_POSITION_2.x, CHECK_TOYS_POSITION_2.y, CHECK_TOYS_POSITION_2.z);
        Vector3 AddToy3 = new Vector3(CHECK_TOYS_POSITION_3.x, CHECK_TOYS_POSITION_3.y, CHECK_TOYS_POSITION_3.z);
        for (int i = 0; i < addToys.Count; i++)
        {
            if (addToys[i].transform.position != AddToy1 && addToys[i].transform.position != AddToy2 && addToys[i].transform.position != AddToy3)
            {
                infoText.text = "Koniec gry\nZle ustawienie";
                mistake.Play();
                check = false;
                break;
            }
        }
        for (int i = 0; i < startToys.Count; i++)
        {
            if (startToys[i].transform.position == AddToy1 || startToys[i].transform.position == AddToy2 || startToys[i].transform.position == AddToy3)
            {
                infoText.text = "Koniec gry\nZle ustawienie";
                mistake.Play();
                check = false;
                break;
            }
        }
        if (check == true)
        {
            infoText.text = "Koniec gry\nPoprawne ustawienie";
            win.Play();
        }
        buttonText.text = "Restart";
    }

    void Update()
    {
        if (game != Game_states.No_game && game != Game_states.End_game)
        {
            updateTimer();
        }
        int seconds = (int)(timer % 60);
        if (seconds >= timeEndRemember && game == Game_states.Remember_toys)
        {
            restartToys();
            settingToys();
        }
        if (seconds >= timeEndGame && game == Game_states.Setting_toys)
        {
            checkToys();
        }
    }
}




