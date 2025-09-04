using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private TMPro.TMP_Text text;
    private Vector3 respawnPosPlayer;
    private Vector3 respawnPosEnemy;
    private GameObject player;
    private GameObject enemy;

    void Start()
    {
        respawnPosPlayer = transform.Find("Start").transform.position;
        respawnPosEnemy = transform.Find("RespawnEnemy").transform.position;
    }

    void Update()
    {
        
    }

    public void GameClear()
    {
        text.SetText("Clear");
        InitGame();
    }

    public void GameOver()
    {
        text.SetText("Game Over");
        InitGame();
    }

    public void GameStart()
    {
        player = Instantiate(prefabPlayer, respawnPosPlayer, Quaternion.identity);
        //enemy = Instantiate(prefabEnemy, respawnPosEnemy, Quaternion.identity);
        text.SetText("");
        buttonStart.SetActive(false);
    }

    private void InitGame()
    {
        Destroy(player);
        Destroy(enemy);
        buttonStart.SetActive(true);
    }
}
