using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {
    public Slider waterSlider, healthSlider, nutrientSlider;
    public GameObject plantUI;
    public GameObject fieldUI;
    public Image itemImage;
    public GameObject itemFill;
    public Slider itemFillSlider;
    public TextMeshProUGUI helpText;
    public GameObject helpUI;
    public TextMeshProUGUI score;

    public TextMeshProUGUI reasonText, scoreText;
    public GameObject gameOverHolder;
    public Slider audioSlider;
    public AudioMixer mixer;
    public static float volume;
    public GameObject pauseMenu;
    

    private void Start() {
        SetItemFill(1,false);
        helpUI.SetActive(false);
        ShowPlantUI(false);
        ShowFieldUI(false);
        ChangeItem(null);
        gameOverHolder.SetActive(false);
        SetVolume();
        pauseMenu.SetActive(false);
    }

    public void SetWater(float value) {
        waterSlider.value = value;
    }

    public void SetHealth(float value) {
        healthSlider.value = value;
    }

    public void SetNutrient(float value) {
        nutrientSlider.value = value;
    }

    public void ShowPlantUI(bool show) {
        plantUI.SetActive(show);
    }

    public void ShowFieldUI(bool show) {
        fieldUI.SetActive(show);
    }

    public void ChangeItem(Sprite sprite) {
        SetItemFill(0,false);
        if (sprite == null) {
            itemImage.gameObject.SetActive(false);
        } else {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
        }
    }

    public void SetItemFill(float fill, bool show = true) {
        itemFill.SetActive(show);
        if (show) {
            itemFillSlider.value = fill;
        }
    }
    
    public void SetText(string text, KeyCode exitKey, Func<int> f = null) {
        helpUI.SetActive(true);
        helpText.SetText(text);
        StartCoroutine(WaitForPress(exitKey, f));
    }

    public void SetScore(int value) {
        score.text = "Your Score: " + value.ToString();
    }
    
    IEnumerator WaitForPress(KeyCode key, Func<int> f) {
        while (!Input.GetKeyDown(key)) {
            yield return null;
        }
        helpUI.SetActive(false);
        if (f != null) 
            f();
    }

    public void GameOver(int score, string reason) {
        // string scoreReward = score > 5
        //     ? "That was really good. But maybe you can beat it?"
        //     : "That was good already. But maybe you can be better";
        gameOverHolder.SetActive(true);
        reasonText.text += reason;
        scoreText.text += score.ToString();
    }

    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToMenu() {
        SceneManager.LoadScene(0);
    }

    public void SetVolume() {
        mixer.SetFloat("Volume", Mathf.Log10(audioSlider.value) * 20);
        volume = audioSlider.value;
    }

    public void Pause() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Continue() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    
}
