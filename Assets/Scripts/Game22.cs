using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Game22 : Game
{
    public enum Knobs_types
    {
        BLUE,
        BROWN,
        GREEN,
        ORANGE,
        PINK,
        PURPLE,
        RED,
        YELLOW
    }
    public enum Knob_settings
    {
        UP,
        LEFT,
        RIGHT,
        DOWN,
        NON
    }
    public enum Game_states
    {
        Re_game,
        No_game,
        Remember_knobs,
        Restart_knobs,
        Setting_knobs,
        End_game
    }
    public List<GameObject> knobs = new List<GameObject>();
    public List<GameObject> knobsWithoutSnapzones = new List<GameObject>();

    private List<GameObject> createKnobs = new List<GameObject>();
    private List<GameObject> createKnobsWithoutSnapzones = new List<GameObject>();
    private List<GameObject> gameKnobs = new List<GameObject>();
    private List<GameObject> gameKnobsSort = new List<GameObject>();
    private Game_states game = Game_states.Re_game;
    private static int MAX_knobs = 8;
    private int[] correctKnobs = new int[MAX_knobs];
    private int[] setKnobs = new int[MAX_knobs];

    private const string NAME_NEXT_LEVEL = "Scenes/Game3/Game3_1.0";
    private const string NAME_RESTART_LEVEL = "Scenes/Game2/Game2_1.1";

    private readonly Vector3 POSITION_OUT_KNOBS = new Vector3(5.0f, 5.0f, 5.0f);
    private readonly Vector3 POSITION_KNOBS_LINE_1 = new Vector3(2.939571f, 0.697f, 6.145f);
    private readonly Vector3 POSITION_KNOBS_LINE_2 = new Vector3(3.163f, 0.697f, 6.341f);
    private readonly Vector3 POSITION_KNOBS_LINE_3 = new Vector3(3.3829f, 0.697f, 6.167f);


    void Start()
    {
        for (int i = 0; i < knobs.Count; i++)
        {
            correctKnobs[i] = (int)Knob_settings.NON;
            setKnobs[i] = (int)Knob_settings.NON;
            createKnobs.Add(GameObject.Find(knobs[i].name));
        }
        outKnobs();
    }
    public void outKnobs()
    {
        for (int i = 0; i < knobs.Count; i++)
        {
            createKnobs[i].transform.position = new Vector3(POSITION_OUT_KNOBS.x, POSITION_OUT_KNOBS.y, POSITION_OUT_KNOBS.y + i * 1f);
        }
    }
    public override int selectLevel()
    {
        float rotLaver = laver.transform.rotation.z;
        int level = 3;
        if (rotLaver <= 0.5f && rotLaver > 0.25f)
            level = 3;
        else if (rotLaver <= 0.25f && rotLaver >= 0)
            level = 4;
        else if (rotLaver <= 0 && rotLaver > -0.25f)
            level = 5;
        else if (rotLaver <= -0.25f && rotLaver >= -0.5f)
            level = 6;

        return level;
    }
    public override void infoLevel()
    {
        int level = selectLevel();
        if (level == 3)
        {
            infoLevelText.text = "£atwy";
        }
        else if (level == 4)
        {
            infoLevelText.text = "Normalny";
        }
        else if (level == 5)
        {
            infoLevelText.text = "Trudny";
        }
        else if (level == 6)
        {
            infoLevelText.text = "Ekstremalny";
        }
    }
    public void GameMenager()
    {
        if (game == Game_states.No_game)
        {
            starGame2();
        }
        else if (game == Game_states.Setting_knobs)
        {
            checkKnobs();
        }
        else if (game == Game_states.End_game)
        {
            outKnobs();
            SceneManager.LoadScene(NAME_RESTART_LEVEL);
        }
    }

    public override void nextGame()
    {
        SceneManager.LoadScene(NAME_NEXT_LEVEL);
    }

    void rotateKnobs(int orignalNumberknob, int counter)
    {
        GameObject childKnob;
        int numberKnobSettings = 4;
        int randNumber = Random.Range(0, numberKnobSettings);
        if (randNumber == (int)Knob_settings.RIGHT)
        {
            childKnob = createKnobsWithoutSnapzones[counter].transform.GetChild((int)Knob_settings.UP).gameObject;
            childKnob.transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
            correctKnobs[orignalNumberknob] = (int)Knob_settings.RIGHT;
        }
        else if (randNumber == (int)Knob_settings.DOWN)
        {
            childKnob = createKnobsWithoutSnapzones[counter].transform.GetChild((int)Knob_settings.UP).gameObject;
            childKnob.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
            correctKnobs[orignalNumberknob] = (int)Knob_settings.DOWN;
        }
        else if (randNumber == (int)Knob_settings.LEFT)
        {
            childKnob = createKnobsWithoutSnapzones[counter].transform.GetChild((int)Knob_settings.UP).gameObject;
            childKnob.transform.Rotate(0.0f, 270.0f, 0.0f, Space.World);
            correctKnobs[orignalNumberknob] = (int)Knob_settings.LEFT;
        }
        else
        {
            correctKnobs[orignalNumberknob] = (int)Knob_settings.UP;
        }
    }

    public void starGame2()
    {
        outKnobs();
        levelNumber = selectLevel();
        infoText.text = "Zapamietaj u≥oøenie ga≥ek\n masz 10s";
        activationDeactivationHand();
        if (game == Game_states.No_game)
        {
            game = Game_states.Remember_knobs;

            List<int> listNumbers = new List<int>();
            int randNumber;
            for (int i = 0; i < knobs.Count; i++)
            {
                do
                {
                    randNumber = Random.Range(0, knobs.Count);
                } while (listNumbers.Contains(randNumber));
                listNumbers.Add(randNumber);
            }
            float Xpos = POSITION_KNOBS_LINE_1.x;
            float Zpos = POSITION_KNOBS_LINE_1.z;
            float Ypos = POSITION_KNOBS_LINE_1.y;
            float step = 0.358f;
            int firstLine = 3;
            for (int i = 0; i < firstLine; i++)
            {
                createKnobsWithoutSnapzones.Add(Instantiate(knobsWithoutSnapzones[listNumbers[i]], new Vector3(Xpos, Ypos, Zpos - i * step), Quaternion.identity));
                rotateKnobs(listNumbers[i], i);
            }
            Xpos = POSITION_KNOBS_LINE_2.x;
            Zpos = POSITION_KNOBS_LINE_2.z;
            int counter = 0;
            for (int i = firstLine; i < levelNumber; i++)
            {
                createKnobsWithoutSnapzones.Add(Instantiate(knobsWithoutSnapzones[listNumbers[i]], new Vector3(Xpos, Ypos, Zpos - counter * step), Quaternion.identity));
                rotateKnobs(listNumbers[i], i);
                counter++;
            }

        }
    }

    public void restartKnobs()
    {
        game = Game_states.Restart_knobs;
        activationDeactivationHand();
        for (int i = 0; i < levelNumber; i++)
        {
            createKnobsWithoutSnapzones[i].SetActive(false);
        }
        float Xpos = POSITION_KNOBS_LINE_1.x;
        float Zpos = POSITION_KNOBS_LINE_1.z;
        float Ypos = POSITION_KNOBS_LINE_1.y;
        float step = 0.358f;
        int firstLine = 3;
        int secendLine = 3;

        int counter = 0;
        for (int i = 0; i < knobs.Count; i++)
        {

            if (i == firstLine)
            {
                Xpos = POSITION_KNOBS_LINE_2.x;
                Zpos = POSITION_KNOBS_LINE_2.z;
                counter = 0;
            }
            else if (i == firstLine + secendLine)
            {
                Xpos = POSITION_KNOBS_LINE_3.x;
                Zpos = POSITION_KNOBS_LINE_3.z;
                counter = 0;
            }
            if (correctKnobs[i] != (int)Knob_settings.NON)
            {
                createKnobs[i].transform.position = new Vector3(Xpos, Ypos, Zpos - counter * step);
            }
            counter++;
        }
    }

    public void sortGameFigures()
    {
        gameKnobsSort = gameKnobs.OrderBy(tile => tile.name).ToList();
    }

    public void settingKnobs()
    {
        game = Game_states.Setting_knobs;
        infoText.text = "Ustaw ga≥ki na poprzednie pozycje masz 20s";
        buttonText.text = "Sprawdü";
    }

    public void checkKnobs()
    {
        game = Game_states.End_game;
        var win = win_sound.GetComponent<AudioSource>();
        var mistake = mistake_sound.GetComponent<AudioSource>();
        bool check = true;
        for (int i = 0; i < knobs.Count; i++)
        {

            if (correctKnobs[i] != (int)Knob_settings.NON && setKnobs[i] != correctKnobs[i])
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
        if (seconds >= timeEndRemember && game == Game_states.Remember_knobs)
        {
            restartKnobs();
            settingKnobs();
        }
        if (seconds >= timeEndGame && game == Game_states.Setting_knobs)
        {
            checkKnobs();
        }
        if (game == Game_states.Re_game)
        {
            game = Game_states.No_game;
            outKnobs();
        }
    }


    // functions used by snapzones
    public void setKnobBlueUP()
    {
        setKnobs[(int)Knobs_types.BLUE] = (int)Knob_settings.UP;
    }
    public void setKnobBlueLeft()
    {
        setKnobs[(int)Knobs_types.BLUE] = (int)Knob_settings.LEFT;
    }
    public void setKnobBlueRight()
    {
        setKnobs[(int)Knobs_types.BLUE] = (int)Knob_settings.RIGHT;
    }
    public void setKnobBlueDown()
    {
        setKnobs[(int)Knobs_types.BLUE] = (int)Knob_settings.DOWN;
    }
    public void setKnobBrownUP()
    {
        setKnobs[(int)Knobs_types.BROWN] = (int)Knob_settings.UP;
    }
    public void setKnobBrownLeft()
    {
        setKnobs[(int)Knobs_types.BROWN] = (int)Knob_settings.LEFT;
    }
    public void setKnobBrownRight()
    {
        setKnobs[(int)Knobs_types.BROWN] = (int)Knob_settings.RIGHT;
    }
    public void setKnobBrownDown()
    {
        setKnobs[(int)Knobs_types.BROWN] = (int)Knob_settings.DOWN;
    }
    public void setKnobPurpleUP()
    {
        setKnobs[(int)Knobs_types.PURPLE] = (int)Knob_settings.UP;
    }
    public void setKnobPurpleLeft()
    {
        setKnobs[(int)Knobs_types.PURPLE] = (int)Knob_settings.LEFT;
    }
    public void setKnobPurpleRight()
    {
        setKnobs[(int)Knobs_types.PURPLE] = (int)Knob_settings.RIGHT;
    }
    public void setKnobPurpleDown()
    {
        setKnobs[(int)Knobs_types.PURPLE] = (int)Knob_settings.DOWN;
    }
    public void setKnobRedUP()
    {
        setKnobs[(int)Knobs_types.RED] = (int)Knob_settings.UP;
    }
    public void setKnobRedLeft()
    {
        setKnobs[(int)Knobs_types.RED] = (int)Knob_settings.LEFT;
    }
    public void setKnobRedRight()
    {
        setKnobs[(int)Knobs_types.RED] = (int)Knob_settings.RIGHT;
    }
    public void setKnobRedDown()
    {
        setKnobs[(int)Knobs_types.RED] = (int)Knob_settings.DOWN;
    }
    public void setKnobYellowUP()
    {
        setKnobs[(int)Knobs_types.YELLOW] = (int)Knob_settings.UP;
    }
    public void setKnobYellowLeft()
    {
        setKnobs[(int)Knobs_types.YELLOW] = (int)Knob_settings.LEFT;
    }
    public void setKnobYellowRight()
    {
        setKnobs[(int)Knobs_types.YELLOW] = (int)Knob_settings.RIGHT;
    }
    public void setKnobYellowDown()
    {
        setKnobs[(int)Knobs_types.YELLOW] = (int)Knob_settings.DOWN;
    }
    public void setKnobGreenUP()
    {
        setKnobs[(int)Knobs_types.GREEN] = (int)Knob_settings.UP;
    }
    public void setKnobGreenLeft()
    {
        setKnobs[(int)Knobs_types.GREEN] = (int)Knob_settings.LEFT;
    }
    public void setKnobGreenRight()
    {
        setKnobs[(int)Knobs_types.GREEN] = (int)Knob_settings.RIGHT;
    }
    public void setKnobGreenDown()
    {
        setKnobs[(int)Knobs_types.GREEN] = (int)Knob_settings.DOWN;
    }
    public void setKnobOrangeUP()
    {
        setKnobs[(int)Knobs_types.ORANGE] = (int)Knob_settings.UP;
    }
    public void setKnobOrangeLeft()
    {
        setKnobs[(int)Knobs_types.ORANGE] = (int)Knob_settings.LEFT;

    }
    public void setKnobOrangeRight()
    {
        setKnobs[(int)Knobs_types.ORANGE] = (int)Knob_settings.RIGHT;
    }
    public void setKnobOrangeDown()
    {
        setKnobs[(int)Knobs_types.ORANGE] = (int)Knob_settings.DOWN;
    }
    public void setKnobPinkUP()
    {
        setKnobs[(int)Knobs_types.PINK] = (int)Knob_settings.UP;
    }
    public void setKnobPinkLeft()
    {
        setKnobs[(int)Knobs_types.PINK] = (int)Knob_settings.LEFT;
    }
    public void setKnobPinkRight()
    {
        setKnobs[(int)Knobs_types.PINK] = (int)Knob_settings.RIGHT;
    }
    public void setKnobPinkDown()
    {
        setKnobs[(int)Knobs_types.PINK] = (int)Knob_settings.DOWN;
    }
}
