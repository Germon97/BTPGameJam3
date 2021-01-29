using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UnityEvent WipeLevel;

    [SerializeField]
    private RoomGenerationScript generator;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject exit;

    [SerializeField]
    private Text levelText;

    public static int currentLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (player == null)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
        currentLevel = 0;
        CreateNewLevel();
    }

    private void CreateNewLevel()
    {
        if (WipeLevel != null)
            WipeLevel.Invoke();

        generator.GenerateMap();

        currentLevel ++;
        levelText.text = "Level: " + currentLevel.ToString();

        player.transform.position = generator.GetStartPosition();
        exit.transform.position = generator.GetExitPosition();
        exit.GetComponent<Exit>().touchedExit.AddListener(CreateNewLevel);
    }
}
