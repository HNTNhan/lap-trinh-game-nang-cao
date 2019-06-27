﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AllEnemy : MonoBehaviour
{
    public GameObject[] allEnemy;
    public float volume;
    public string difficulty;

    private void OnEnable()
    {
        volume = PlayerPrefs.GetFloat("Volume");
        difficulty = PlayerPrefs.GetString("Difficulty");
        allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (PlayerPrefs.GetString("Load") == "true") LoadEnemy();
    }

    public void SaveEnemy()
    {
        if (PlayerPrefs.GetString("Save") == "fail")
        {
            PlayerPrefs.SetString("Save", "");
            return;
        }
        try
        {
            SaveSystem.SaveEnemy(this);
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Save Game", "Save Game Fail \n" + e.Message, "OK");
            return;
        }
        EditorUtility.DisplayDialog("Save Game", "Save Game Success", "OK");
    }
    public void LoadEnemy()
    {
        EnemyData data = SaveSystem.LoadEnemy();

        for (int j = 0; j < allEnemy.Length; j++)
        {
            for (int i = 0; i < data.name.Length; i++)
            {
                if (allEnemy[j].name == data.name[i])
                {
                    allEnemy[j].SetActive(true);
                    allEnemy[j].GetComponent<AudioSource>().volume = data.volume;
                    allEnemy[j].GetComponent<Health>().SetCurrentHealth(data.health[i]);
                    allEnemy[j].GetComponent<Health>().TakeDamage(0);
                    allEnemy[j].GetComponent<Enemy>().checkTime = 0;
                    allEnemy[j].GetComponentInChildren<Animator>().Rebind();
                    Vector3 position = new Vector3(data.position[i, 0], data.position[i, 1], data.position[i, 2]);
                    allEnemy[j].GetComponent<Transform>().position = position;
                    break;
                }
                if (i + 1 >= data.name.Length)
                {
                    allEnemy[j].SetActive(false);
                }
            }
        }
    }
}
