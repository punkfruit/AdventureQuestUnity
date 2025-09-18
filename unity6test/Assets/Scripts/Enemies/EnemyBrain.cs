using UnityEngine;

public enum BehaviorType { Wander, Chase, Patrol }

public class EnemyBrain : MonoBehaviour, IMoveProvider
{
    public BehaviorType behavior = BehaviorType.Wander;

    [Header("Refs")]
    public EnemyMotor motor;
    public Transform target;          // For chase
    public Transform[] patrolPoints;  // For patrol

    [Header("Wander")]
    public float moveTimeMin = 1f, moveTimeMax = 3f;
    public float waitTimeMin = 0.5f, waitTimeMax = 2f;

    private Vector2 _wanderDir = Vector2.down;
    private float _timer;
    private int _patrolIndex;

    void Start()
    {
        if (!motor) motor = GetComponent<EnemyMotor>();
        motor.SetProvider(this);
        ResetWander();
    }

    public Vector2 GetDesiredVelocity()
    {
        switch (behavior)
        {
            case BehaviorType.Wander: return Wander();
            case BehaviorType.Chase:  return Chase();
            case BehaviorType.Patrol: return Patrol();
            default: return Vector2.zero;
        }
    }

    Vector2 Wander()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0f)
        {
            // Toggle between moving and waiting
            if (_wanderDir == Vector2.zero)
            {
                _wanderDir = RandomCardinal();
                _timer = Random.Range(moveTimeMin, moveTimeMax);
            }
            else
            {
                _wanderDir = Vector2.zero;
                _timer = Random.Range(waitTimeMin, waitTimeMax);
            }
        }
        return _wanderDir;
    }

    Vector2 Chase()
    {
        if (!target) return Vector2.zero;
        Vector2 to = (target.position - transform.position);
        return to.sqrMagnitude > 0.01f ? to.normalized : Vector2.zero;
    }

    Vector2 Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return Vector2.zero;
        var dest = (Vector2)patrolPoints[_patrolIndex].position;
        var to = dest - (Vector2)transform.position;

        if (to.magnitude < 0.1f)
            _patrolIndex = (_patrolIndex + 1) % patrolPoints.Length;

        return to.sqrMagnitude > 0.0001f ? to.normalized : Vector2.zero;
    }

    static Vector2 RandomCardinal()
    {
        switch (Random.Range(0, 4))
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            default: return Vector2.right;
        }
    }

    void ResetWander()
    {
        _wanderDir = Vector2.zero;
        _timer = Random.Range(waitTimeMin, waitTimeMax);
    }
}
