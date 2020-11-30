using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum MoveDirection
{
    Forward = 0,
    Left = 1,
    Right = 2
}

public enum MoveStatus
{
    Walk = 0,
    Run = 1
}

[RequireComponent(typeof(Collider))]
public class Snake : MonoBehaviour
{
    public event Action<MoveStatus> OnChangeMoveStatus;
    public event Action<float, float> OnChangeStamina;
    public UnityEvent OnEat;

    public MoveStatus MoveStatus { 
        get { return _moveStatus; } 
        set { if(_moveStatus != value)
            {
                _moveStatus = value; 
                OnChangeMoveStatus?.Invoke(value); 
            }  
        }
    }
    public float CurrentMoveSpeed { get; set; }

    [SerializeField] private List<SnakeTail> _snakeTail = new List<SnakeTail>();
    [SerializeField] private SnakeTail _tailPrefab;
    [Header("Stamina settings")]
    [SerializeField, Range(0, 50)] private float _stamina;
    [SerializeField, Range(0, 50)] private float _addStaminaOnEat;
    [SerializeField, Range(0, 50)] private float _minStaminaToRun;
    [SerializeField, Range(0, 50)] private float _staminaLessSpeed;
    [SerializeField, Range(0, 50)] private float _staminaRestoreSpeed;
    [Header("Move and rotate settings")]
    [SerializeField, Range(0, 50)] private float _walkSpeed;
    [SerializeField, Range(0, 50)] private float _runSpeed;
    [SerializeField, Range(0, 50)] private float _rotateSensity;
    [Header("Tail settings")]
    [SerializeField, Range(0, 10)] private float _tailBoneDistance;
    [SerializeField] private Transform _startTail;

    private MoveStatus _moveStatus = MoveStatus.Walk;
    private MoveDirection _moveDirection;
    private Transform _transform;
    private Transform _lastTailElement;
    private float _maxStamina;
    private bool _canRun = true;

    public void Run() => MoveStatus = MoveStatus.Run;
    public void Walk() => MoveStatus = MoveStatus.Walk;
    public void MoveRight() => _moveDirection = MoveDirection.Right;
    public void MoveLeft() => _moveDirection = MoveDirection.Left;
    public void MoveForward() => _moveDirection = MoveDirection.Forward;

    private void Start()
    {
        _transform = gameObject.transform;
        _lastTailElement = _transform;
        _maxStamina = _stamina;
    }

    private void OnValidate()
    {
        if (_runSpeed <= _walkSpeed)
            _runSpeed = _walkSpeed * 1.8f;
    }

    /// <summary>
    /// Returns moveSpeed
    /// </summary>
    /// <param name="isRuning"></param>
    /// <returns></returns>
    private void UpdateStamina(float value)
    {
        _stamina += value;
        OnChangeStamina?.Invoke(_stamina, _maxStamina);

        if (_stamina <= 0)
            _canRun = false;
        else if (_stamina > _maxStamina)
            _stamina = _maxStamina;
    }

    private void Move()
    {
        //Check can snake run.
        if (_canRun == false && _stamina >= _minStaminaToRun)
            _canRun = true;
        //Input.GetKey(KeyCode.LeftShift) == true

        bool _isRuning = (MoveStatus == MoveStatus.Run && _canRun == true) ? true : false;
        MoveStatus = _isRuning == false ? MoveStatus.Walk : MoveStatus.Run;

        UpdateStamina((_isRuning == true ? -_staminaLessSpeed : _staminaRestoreSpeed) * Time.deltaTime);
        CurrentMoveSpeed = _isRuning == true ? _runSpeed : _walkSpeed;
        _transform.position += _transform.forward * CurrentMoveSpeed * Time.deltaTime;

        //Rotate left or right.
        if(_moveDirection == MoveDirection.Left)
            _transform.Rotate(0, _rotateSensity * 10 * -Time.deltaTime, 0);
        else if (_moveDirection == MoveDirection.Right)
            _transform.Rotate(0, _rotateSensity * 10 * Time.deltaTime, 0);


        MoveStatus = Input.GetKey(KeyCode.LeftShift) == true? MoveStatus.Run : MoveStatus.Walk;

        if (Input.GetKey(KeyCode.Escape) == true && GameController.GameStatus == GameStatus.Game)
            GameController.GameStatus = GameStatus.Pause;

        if (Input.GetKey(KeyCode.A) == true)
            _transform.Rotate(0, _rotateSensity * 10 * -Time.deltaTime, 0);
        else if (Input.GetKey(KeyCode.D) == true)
            _transform.Rotate(0, _rotateSensity * 10 * Time.deltaTime, 0);

    }

    private void MoveTail()
    {
        if (_snakeTail.Count == 0)
            return;

        Transform _previousPosition = _startTail == null? transform : _startTail;
        float _sqrTailDistance = _tailBoneDistance * _tailBoneDistance;

        for (int i = 0; i < _snakeTail.Count; i++)
            if ((_snakeTail[i].transform.position - _previousPosition.position).sqrMagnitude > _sqrTailDistance)
                _previousPosition = MoveTail(_snakeTail[i].transform, _previousPosition);

        Transform MoveTail(Transform from, Transform to)
        {
            from.LookAt(to);
            from.position += (to.position - from.position).normalized * CurrentMoveSpeed * Time.deltaTime;
            return from;
        }
    }

    private void EatFood()
    {
        ClearListFromNull();
        _snakeTail.Add(Instantiate(_tailPrefab.gameObject, _lastTailElement.position, Quaternion.identity).GetComponent<SnakeTail>());
        _lastTailElement = _snakeTail[_snakeTail.Count - 1].transform;
        UpdateStamina(_addStaminaOnEat);
        Scope.AddScope(1);
        OnEat?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        SnakeTail _tail;
        if ((_tail = other.gameObject.GetComponent<SnakeTail>()) != null && _snakeTail.Count > 3)
        {
            for (int i = 0; i <= 3; i++)
                if (Equals(_tail, _snakeTail[i]) == false)
                    GameController.GameStatus = GameStatus.GameOver;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Food>() == true && _tailPrefab != null)
        {
            Destroy(collision.gameObject);
            EatFood();
        }
        else if(collision.gameObject.GetComponent<Building>() == true)
        {
            GameController.GameStatus = GameStatus.GameOver;
        }
        
    }

    private void ClearListFromNull()
    {
        for (int i = _snakeTail.Count - 1; i >= 0; i--)
            if (_snakeTail[i] == null)
                _snakeTail.RemoveAt(i);

        _lastTailElement = _snakeTail.Count > 0 ? _snakeTail[_snakeTail.Count - 1].transform : _lastTailElement = _transform;
    }

    private void Update()
    {
        Move();
        MoveTail();
    }


}
