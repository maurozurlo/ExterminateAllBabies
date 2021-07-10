using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpawner : MonoBehaviour {
    Dictionary<GameObject, float> enemies = new Dictionary<GameObject, float>();
    Dictionary<GameObject, float> powerups = new Dictionary<GameObject, float>();
    Vector2 timeBetweenSpawns;
    int levelPowerUps, levelEnemies;
    int spawnedPowerUps = 0, spawnedEnemies = 0;

    void Start() {
        // Get config
        GameConfig config = GameInit.control.config;
        // Set dictionaries
        AddItemsToDictionary(ref enemies, config.enemyPrefabs, config.enemyWeights, "enemies");
        AddItemsToDictionary(ref powerups, config.powerupPrefabs, config.powerUpWeights, "powerups");
        // Get Amounts for level
        levelEnemies = config.enemies;
        levelPowerUps = config.powerUps;
        timeBetweenSpawns = config.timeBetweenSpawns;
    }

    public void StartSpawning() {
        StartCoroutine("Spawner");
    }

    IEnumerator Spawner() {
        bool cantSpawnEnemy = spawnedEnemies == levelEnemies;
        bool cantSpawnPowerUp = spawnedPowerUps == levelPowerUps;

        if (cantSpawnEnemy && cantSpawnPowerUp) yield break; //Nothing to do

        //Decide if we're spawning an enemy or a powerup
        bool isEnemy = Random.Range(0, 1) < .5f;

        if (isEnemy && !cantSpawnEnemy || !isEnemy && cantSpawnPowerUp) SpawnEnemy();
        else if (isEnemy && cantSpawnEnemy || !isEnemy && !cantSpawnPowerUp) SpawnPowerUp();

        yield return new WaitForSeconds(Random.Range(timeBetweenSpawns.x, timeBetweenSpawns.y));
        StartCoroutine("Spawner");
    }

    void Spawn(ref Dictionary<GameObject, float> dict, string tag, ref int counter) {
        GameObject spawneable = GetRandom(ref dict);
        GameObject gameObject = Instantiate(spawneable, transform.position, Quaternion.identity) as GameObject;
        gameObject.name = tag;
        gameObject.tag = tag;
        gameObject.transform.SetParent(this.gameObject.transform);
        counter++;
    }

    // Helpers
    void SpawnPowerUp() {
        Spawn(ref powerups, "PowerUp", ref spawnedPowerUps);
    }

    void SpawnEnemy() {
        Spawn(ref enemies, "Enemy", ref spawnedEnemies);
    }

    GameObject GetRandom(ref Dictionary<GameObject, float> dict) {
        return dict.RandomElementByWeight(e => e.Value).Key;
    }

    void AddItemsToDictionary(ref Dictionary<GameObject, float> dict, GameObject[] gameObjects, List<float> list, string name) {
        int totalLength = gameObjects.Length;
        for (int i = 0; i < totalLength; i++) {
            dict.Add(gameObjects[i], list[i]);
        }
    }
}
