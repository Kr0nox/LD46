using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public Tool fork;
    public Tool can;
    public Tool scisor;
    public GameUIController ui;

    public float score = 1;
    public float  scoreGain = 2;
    public float scoreTime = 0.1f;
    public int plantNumber = 1;

    private int forkUpgrades = 1;
    private int forkUpgradeScore = 4;
    private int canUpgrades = 1;
    private int canUpgradeScore = 5;
    
    void Start() {
        Time.timeScale = 1;
        fork.AppearOnGround(new Vector2(4.9f,-2));
        can.AppearOnGround(new Vector2(7.95f,-1.95f));
        scisor.AppearOnGround(new Vector2(7, -2));
        ui.SetText("You have to keep your holy purple flower alive. You need to water it, so go and get the watering can. " +
                   "(Press Enter to hide Message)", KeyCode.Return);
    }

    private void FixedUpdate() {
        score += Mathf.Pow(scoreGain, plantNumber) * Time.fixedDeltaTime * scoreTime;
        ui.SetScore((int)score);
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ui.Pause();
        }

        if (score > forkUpgrades * forkUpgradeScore) {
            fork.Upgrade(forkUpgrades);
            forkUpgrades++;
        }

        if (score > canUpgrades * canUpgradeScore) {
            can.Upgrade(canUpgrades);
            canUpgrades++;
        }
    }

    public void GameOver(string reason) {
        Time.timeScale = 0;
        ui.GameOver((int)score, reason);
    }
}
