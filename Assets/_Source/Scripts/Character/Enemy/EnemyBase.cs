using UnityEngine;

public class EnemyBase : CharacterBase
{
    [SerializeField] private Transform[] _points;

    private Transform _target;
    private int _currentPath;

    private bool _isPatrol;
    private const float MoveAmount = 1;
    private const float IntervalPatrol = 2;
    private float _currentTime;

    protected override void Init()
    {
        _target = _points[0];

        Game.Action.OnEnter += () =>
        {
            _target = _points[0];
            _currentPath = 0;
            OnPause(false);
        };

        Game.Action.OnExit += () =>
        {
            Restart();
        };
    }

    protected override void GetInput()
    {
        if (Vector3.Distance(transform.position, _points[_currentPath].position) <= .5f)
        {
            _currentPath++;
            if (_currentPath >= _points.Length)
                _currentPath = 0;

            _isPatrol = false;
            _currentTime = IntervalPatrol;
            _target = _points[_currentPath];
        }

        if(!_isPatrol)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0) _isPatrol = true; 
        }
    }

    protected override void Movement()
    {
        if (_isPatrol)
        {
            _movement.MovementDirection = transform.TransformDirection(Vector3.forward);
            _movement.Move(MoveAmount);

            if (_target == null) return;

            Vector3 direction = (_target.position - transform.position).normalized;
            _movement.Rotate(direction);
        }

        _animator.MovementAnimations(_isPatrol ? MoveAmount : 0);
    }
}