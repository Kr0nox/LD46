using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCan : Tool {
   private GameObject[] plants;
   public float distancePlant;
   public Transform[] fountains;
   public float distanceFountain;

   public float baseRate = 0.3f;
   public float rate;
   
   private bool hasWatered = false;
   private bool wasFilled = false;

   public AudioSource usingCan;

   private void Awake() {
      plants = GameObject.FindGameObjectsWithTag("Plant");
      hasFill = true;
      rate = baseRate;
   }

   public void Update() {
      if (fill < 1) {
         foreach (Transform fountain in fountains) {
            if (Vector2.Distance(transform.position, fountain.position) < distanceFountain) {
               fill += 0.5f * Time.deltaTime;
               ui.SetItemFill(fill);
               break;
            }
         }
      }
      if (hasWatered || !wasFilled) {
         return;
      }
      if (Vector2.Distance(transform.position, plants[0].transform.position) < distancePlant && equipted) {
         controller.GetComponent<GameController>().ui.SetText("Press the left mouse button to water the plant. But be careful: " +
                                                              "To much water is bad too!", KeyCode.Mouse0);
         hasWatered = true;
      }
   }

   public override void ActionWhileDown() {
      if (!equipted)
         return;
      if (fill > 0) {
         foreach (GameObject plant in GameObject.FindGameObjectsWithTag("Plant")) {
            if (Vector2.Distance(transform.position, plant.transform.position) < distancePlant) {
               plant.GetComponent<Plant>().AddWater(0.3f * rate * Time.deltaTime);
               fill -= 0.3f * Time.deltaTime;
               ui.SetItemFill(fill);
            }
         }
      }
   }

   protected override void OnPickUp() {
      if(wasFilled)
         return;

      ui.SetText("Your watering can is empty. Go and fill it at the fountain by standing next to it.", KeyCode.Mouse0);
      wasFilled = true;
   }

   public override void Upgrade(int number) {
      rate = (number + 1) * baseRate;
      ui.SetText("Your watering can received an upgrade. It holds more water now. (Press Enter to hide message)", KeyCode.Return);
   }

   public override void ActionOnPress() {
      if (!equipted)
         return;
      usingCan.Play();
   }

   public override void ActionRelease() {
      if(!equipted)
         return;
      usingCan.Stop();
   }
}
