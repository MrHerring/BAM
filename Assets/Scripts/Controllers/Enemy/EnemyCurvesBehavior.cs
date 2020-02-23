using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCurvesBehavior : MonoBehaviour, IPoolableComponent
{
    public AnimationCurve verticalMovement;

    private Keyframe[] _vmKeys;

    public AnimationCurve verticalStretch;

    public AnimationCurve horizontalStretch;

    private float _yPosition;
    private float _xScale;
    private float _yScale;

    private float _currentTime;

    [SerializeField]
    private int _enemyScoreValue;

    [SerializeField]
    private float _minEnemyVerticalPosition;
    [SerializeField]
    private float _maxEnemyVerticalPosition;

    private Animator _animator;

    private static bool _inputEnabled = true;

    [SerializeField]
    private bool _isFriendly;

    [SerializeField]
    private float _timePenalty;

    void Awake()
    {
        _animator = this.transform.GetComponentInChildren<Animator>();
        _vmKeys = verticalMovement.keys;
    }

    /* 
     * Method of IPoolableComponent
     * Called When Enemy is Spawned
     */
    
    public void Spawned()
    {
       
    }

    /* 
     * Call : When Object is Despawned in PrefabPool Class
     */
    public void Despawned()
    {
        _currentTime = 0;

        _vmKeys[1].value = Random.Range(_minEnemyVerticalPosition, _maxEnemyVerticalPosition);

        verticalMovement.keys = _vmKeys;
    }

    /*
     * Enemy movement and Animation
     * It Can Be big problem in feature
     */
    void Update()
    {
        if (_currentTime < 2.26)
        {
            _yPosition = verticalMovement.Evaluate(_currentTime);
            _xScale = horizontalStretch.Evaluate(_currentTime);
            _yScale = verticalStretch.Evaluate(_currentTime);

            this.transform.position = new Vector3(transform.position.x, _yPosition, 0);
            this.transform.localScale = new Vector3(_xScale, _yScale,1);
        }
        else
        {
            OutOfScreenDestroy();
        }

        _currentTime += Time.deltaTime;
    }


    public static void EnableOnTap()
    {
        _inputEnabled = true;
    }

    public static void DisableOnTap()
    {
        _inputEnabled = false;
    }

    // Destroy Object When Tapped
    private void OnMouseDown()
    {
        if (_inputEnabled)
        {
            OnTapDestroy();
        }
        
    }

    //Needs to reset bonus in gameController
    //Needs to reset extra second counter in gameController
    private void OutOfScreenDestroy()
    {
        if (!_isFriendly)
        {
            GameController.Instance.ResetBonus();
        }

        PrefabPoolingSystem.Despawn(this.gameObject);
    }

    
    private void OnTapDestroy()
    {
        if (!System.Object.ReferenceEquals(_animator, null)) 
        {
            _animator.Play("Destroy");
            Invoke("DelayDestory", _animator.runtimeAnimatorController.animationClips[1].length);
        }
        else
        {
            if (_isFriendly)
            {
                GameController.Instance.TimePenalty(5f);
                GameController.Instance.ResetBonus();
            }

            PrefabPoolingSystem.Despawn(this.gameObject);
        }

        GameController.Instance.UpdateScore(_enemyScoreValue);
    }

    private void DelayDestory()
    {
        if (_isFriendly)
        {
            GameController.Instance.TimePenalty(_timePenalty);
            GameController.Instance.ResetBonus();
        }

        PrefabPoolingSystem.Despawn(this.gameObject);
    }

    
}
