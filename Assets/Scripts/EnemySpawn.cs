using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour {
    public float rate = 10f;
    public float increase = 0.01f;
    public GameObject enemyPrefab;
    public float radius;
    private float time = 0;
    private bool spawned;
    public GameUIController ui;

    private void Update() {
        time += Time.deltaTime;
        if (time > rate - rate / 3 && !spawned) {
            ui.SetText("The weed is starting to come. You can defeat it with the fork, so go and pick it up!", KeyCode.E);
            spawned = true;
        }
        if (time > rate) {
            for (int i = 0; i < Random.Range(1,4); i++) {
                float deg = Random.Range(0, 3.14f);
                Instantiate(enemyPrefab, radius * new Vector2(Mathf.Cos(deg), Mathf.Sin(deg)), 
                    Quaternion.identity, transform);
            }

            rate -= increase;
            if (rate < 15) {
                rate = 15;
            }
            time = 0;
        }
    }
}
