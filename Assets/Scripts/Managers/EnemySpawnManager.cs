using System.Collections;
using UnityEngine;

/*
 * Uses PoolingSystem class for Enemy Spawn
 * Listener to OnEnemyDestory Event in Observer Pattern (Observer: GameController class)
 */

public class EnemySpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _mobileSpawnPoints;

    [SerializeField]
    private SO_GameSkin _gameSkin;

    private GameObject[] _prefabs;

    [SerializeField]
    private float _minSpawnTimePeriod;
    private float _resetMinSpawnTimePeriod;

    [SerializeField]
    private float _maxSpawnTimePeriod;
    private float _resetMaxSpawnTimePeriod;

    [SerializeField]
    private float _minSpawnTimePeriodLimit;

    [SerializeField]
    private float _timeDecrement;

    [SerializeField]
    private int _updateTimeScoreAmount;
    [SerializeField]
    private int _updateTimeCnt;

    void Awake()
    {
        _prefabs = _gameSkin.enemyPrefabs;

        _resetMinSpawnTimePeriod = _minSpawnTimePeriod;
        _resetMaxSpawnTimePeriod = _maxSpawnTimePeriod;

        PrefabPoolingSystem.Prespawn(_prefabs, _mobileSpawnPoints[0].transform.position, _mobileSpawnPoints[0].transform.rotation, 3);
    }

    void Start()
    {
        GameController.OnEnemyDestroy += UpdateScore;
        StartCoroutine("SpawnEnemy");
    }


    private void UpdateScore(int currentScore)
    {
        if ((_updateTimeCnt * _updateTimeCnt * _updateTimeScoreAmount) < currentScore)
        {
           UpdateTimes();
           _updateTimeCnt++;
        }
    }

    public void ReplayGame()
    {
        _updateTimeCnt = 1;

        _minSpawnTimePeriod = _resetMinSpawnTimePeriod;
        _maxSpawnTimePeriod = _resetMaxSpawnTimePeriod;
    }

    private void UpdateTimes()
    {
        if (_minSpawnTimePeriod > _minSpawnTimePeriodLimit)
        {
            _minSpawnTimePeriod -= _timeDecrement;
            _maxSpawnTimePeriod -= _timeDecrement;
        }
        else if (_maxSpawnTimePeriod > _minSpawnTimePeriodLimit)
        {
            _maxSpawnTimePeriod -= _timeDecrement;
        }

        GameController.Instance.Bonus();
    }

    //Async Enemy Spawn on predefined time delay
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            float bombProbability = Random.Range(0f, 1f);
            float prevRandomPoint = 0f;

            int enemyPrefabIndex;
            int prevEnemyPrefabIndex = 0;

            if (bombProbability < .93f)
            {
                enemyPrefabIndex = Random.Range(0, _prefabs.Length - 1);
            }
            else
            {
                enemyPrefabIndex = _prefabs.Length - 1;
            }


            float _spawnTimePeriod = Random.Range(_minSpawnTimePeriod, _maxSpawnTimePeriod);

            float randomPoint = Random.Range(-2f, 2f);

            if (Mathf.Abs(randomPoint - prevRandomPoint) < 0.5f && ((enemyPrefabIndex == _prefabs.Length - 1) || (prevEnemyPrefabIndex == _prefabs.Length - 1)))
            {
                if (randomPoint < 0)
                {
                    randomPoint += 1f;
                }
                else
                {
                    randomPoint -= 1f;
                }
            }

            prevRandomPoint = randomPoint;
            prevEnemyPrefabIndex = enemyPrefabIndex;


            PrefabPoolingSystem.Spawn(_prefabs[enemyPrefabIndex], new Vector3(randomPoint, -6f, 0f), new Quaternion(0f, 0f, 0f, 0f));

            yield return new WaitForSeconds(_spawnTimePeriod);
        }
    }

    //Bug Fix
    public void OnDestroy()
    {
        GameController.OnEnemyDestroy -= UpdateScore;
    }
}
