using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour {
    public float nutrient;
    public float nutrientDecay = .001f;
    public int plantCount = 0;
    GameUIController ui;
    public bool startUnrandom;
    public int growCount = 0;
    public float size = 1;
    private bool shownText = false;
    private bool uiShown = false;

    private void Start() {
        if(!startUnrandom)
            nutrient = Random.Range(.7f, 1);
        ui = FindObjectOfType<GameUIController>();
    }
    
    private void Update() {
        if (plantCount > 0) {
            nutrient = Plant.LimitToRange(nutrient - (nutrientDecay * Time.deltaTime * plantCount * (growCount > 0 ? growCount * 1.5f : 1)/size),
                0f, 1f);
        }
        
        if(uiShown)
            ui.SetNutrient(nutrient);

        if (nutrient < .2f && !shownText) {
            ui.SetText("A bed has a low nutrient. Replant the flower in another bed, using the fork. (Press Enter to hide message)",
                KeyCode.Return);
            shownText = true;
        }

        if (nutrient > .2f) {
            shownText = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            
            ui.ShowFieldUI(true);
            uiShown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            ui.ShowFieldUI(false);
            uiShown = false;
        }

    }

    public void GetDung() {
        nutrient += 0.6f / size;
        if (nutrient > 1)
            nutrient = 1;
    }
}
