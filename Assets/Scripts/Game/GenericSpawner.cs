using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpawner : MonoBehaviour {
    Dictionary<GameObject, float> enemies;
    Dictionary<GameObject, float> powerups;
    Vector2 timeBetweenSpawns;
    int levelPowerUps, levelEnemies;
    GameController control;

    void Start() {
        // Get config
        GameConfig config = GameInit.control.config;
        control = GameController.control;
        // Set dictionaries
        enemies = AddItemsToDictionary(config.enemyPrefabs, config.enemyWeights);
        powerups = AddItemsToDictionary(config.powerupPrefabs, config.powerUpWeights);
        // Get Amounts for level
        levelEnemies = config.enemies;
        levelPowerUps = config.powerUps;
        timeBetweenSpawns = config.timeBetweenSpawns;
    }

    public void StartSpawning() {
        StartCoroutine("Spawner");
    }

    IEnumerator Spawner() {
        bool cantSpawnEnemy = control.GetEnemiesSpawned() == levelEnemies;
        bool cantSpawnPowerUp = control.GetPowerUpsSpawned() == levelPowerUps;

        if (cantSpawnEnemy && cantSpawnPowerUp) yield break; //Nothing to do

        //Decide if we're spawning an enemy or a powerup
        bool isEnemy = Random.Range(0, 1) < .5f;

        if (isEnemy && !cantSpawnEnemy || !isEnemy && cantSpawnPowerUp) SpawnEnemy();
        else if (isEnemy && cantSpawnEnemy || !isEnemy && !cantSpawnPowerUp) SpawnPowerUp();

        yield return new WaitForSeconds(Random.Range(timeBetweenSpawns.x, timeBetweenSpawns.y));
        StartCoroutine("Spawner");
    }

    void Spawn(Dictionary<GameObject, float> dict, string tag) {
        GameObject gameObject = Instantiate(GetRandom(dict), transform.position, Quaternion.identity) as GameObject;
        gameObject.name = tag;
        gameObject.tag = tag;
        gameObject.transform.SetParent(this.gameObject.transform);
        if (tag == "Enemy") control.IncrementEnemiesSpawned();
        else control.IncrementPowerUpsSpawned();
    }

    // Helpers
    void SpawnPowerUp() {
        Spawn(powerups, "PowerUp");
    }

    void SpawnEnemy() {
        Spawn(enemies, "Enemy");
    }

    GameObject GetRandom(Dictionary<GameObject, float> dict) {
        return dict.RandomElementByWeight(e => e.Value).Key;
    }

    Dictionary<GameObject, float> AddItemsToDictionary(GameObject[] gameObjects, List<float> list) {
        Dictionary<GameObject, float> _dict = new Dictionary<GameObject, float>();
        int totalLength = gameObjects.Length;
        for (int i = 0; i < totalLength; i++) {
            _dict.Add(gameObjects[i], list[i]);
        }
        return _dict;
    }

    public bool HasChildren() {
        return transform.childCount >= 1;
    }
}
