using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungSpawner : MonoBehaviour
{
    public float rate = 120f;
    public GameObject dungPrefab;
    public float radius;
    private float time = 0;
    private bool spawned;
    public GameUIController ui;
    public Vector2 offset;

    [Header("For Dung Item")] 
    public Transform controller;
    public Collider2D[] fields;
    public AudioSource packSound;
    public PlayerController player;
    
    
    
    private void Update() {
        time += Time.deltaTime;
        
        if (time > rate) {
            if (!spawned) {
                ui.SetText("The compost produced some fertilizer. Use it to put nutrient in used beds. (Press Enter to hide message)",
                    KeyCode.Return);
                spawned = true;
            }

            Vector2 pos = transform.position;
            GameObject dung = Instantiate(dungPrefab, pos + offset, 
                Quaternion.identity, transform);
            ToolDung d = dung.GetComponent<ToolDung>();
            // d.controller = controller;
            // d.fields = fields;
            // d.packSound = packSound;
            // d.player = player;
            time = 0;
        }
    }
}
