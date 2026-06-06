using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy2D : MonoBehaviour
{
	[Header("References")]
	public Transform[] patrolPoints;
	private Transform target;

	[Header("Movement")]
	public float walkSpeed = 1.5f;
	public float runSpeed = 3.5f;

	[Header("Vision Settings")]
	public float viewDistance = 6f;
	public float viewAngle = 60f;
	public LayerMask obstacleMask;

	[Header("Chase Settings")]
	public float chaseKeepTime = 2f;
	public float lostWaitTime = 1.5f;
	private float chaseTimer = 0f;
	private float waitTimer = 0f;

	[Header("Visual")]
	[SerializeField] public Transform spriteRoot;

	public LaserPointer laser;

	public bool IsMovementStopped = false;
	private Coroutine stopRoutine;

	private NavMeshAgent agent;
	private int currentPatrolIndex = 0;

	private enum State { Patrol, Chase, Wait }
	private State state = State.Patrol;
	private Vector2 lastMoveDir = Vector2.right;

	public Transform Target => target;
	public bool IsExternalControl { get; set; } = false;

	private void OnEnable()
	{
		TrapEvent.OtonarashiTrapActivated += GetPlayerPosition;
	}

	private void OnDisable()
	{
		TrapEvent.OtonarashiTrapActivated -= GetPlayerPosition;
	}

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

		target = GameObject.FindWithTag("Player").transform;

		agent.speed = walkSpeed;
		GoToNextPatrolPoint();
	}

	private bool canSeePlayerCached;

	void Update()
	{
		if (IsMovementStopped)
		{
			agent.velocity = Vector3.zero;
			agent.ResetPath();
			return;
		}

		if (IsExternalControl)
		{
			UpdateSpriteRotation();
			return;
		}

		bool canSeePlayer = CanSeePlayer();
		canSeePlayerCached = canSeePlayer;

		switch (state)
		{
			case State.Patrol:
				PatrolUpdate(canSeePlayer);
				break;
			case State.Chase:
				ChaseUpdate(canSeePlayer);
				break;
			case State.Wait:
				WaitUpdate();
				break;
		}

		UpdateSpriteRotation();
	}

	public bool IsChasingPlayer => state == State.Chase && canSeePlayerCached;
	public bool IsChasingPlayerStable => state == State.Chase;

	void PatrolUpdate(bool canSeePlayer)
	{
		laser.SetActive(false);

		if (canSeePlayer)
		{
			state = State.Chase;
			agent.speed = runSpeed;
			chaseTimer = chaseKeepTime;
			return;
		}

		if (!agent.pathPending && agent.remainingDistance < 0.2f)
		{
			GoToNextPatrolPoint();
		}
	}

	void ChaseUpdate(bool canSeePlayer)
	{
		if (IsMovementStopped)
		{
			agent.velocity = Vector3.zero;
			agent.ResetPath();
			return;
		}

		laser.SetActive(canSeePlayer);

		if (canSeePlayer)
			chaseTimer = chaseKeepTime;
		else
			chaseTimer -= Time.deltaTime;

		if (chaseTimer <= 0f)
		{
			state = State.Wait;
			waitTimer = lostWaitTime;
			agent.speed = 0f;
			agent.ResetPath();
			return;
		}

		agent.isStopped = false;
		agent.speed = runSpeed;
		agent.SetDestination(target.position);

		if (agent.velocity.sqrMagnitude > 0.01f)
			lastMoveDir = agent.velocity.normalized;
	}

	private void GetPlayerPosition(GameObject player)
	{
		target = player.transform;
		state = State.Chase;
		agent.SetDestination(target.position);
	}

	void GoToNearestPatrolPoint()
	{
		if (patrolPoints.Length == 0) return;

		float bestDist = Mathf.Infinity;
		int bestIndex = 0;

		Vector2 pos = transform.position;

		for (int i = 0; i < patrolPoints.Length; i++)
		{
			float d = Vector2.Distance(pos, patrolPoints[i].position);
			if (d < bestDist)
			{
				bestDist = d;
				bestIndex = i;
			}
		}

		currentPatrolIndex = bestIndex;
		agent.SetDestination(patrolPoints[currentPatrolIndex].position);
	}

	void WaitUpdate()
	{
		laser.SetActive(false);
		waitTimer -= Time.deltaTime;

		if (waitTimer <= 0f)
		{
			state = State.Patrol;
			agent.speed = walkSpeed;
			GoToNearestPatrolPoint();
		}
	}

	void UpdateSpriteRotation()
	{
		if (spriteRoot == null) return;

		Vector2 moveDir = agent.velocity.sqrMagnitude > 0.01f
			? agent.velocity.normalized
			: lastMoveDir;

		if (moveDir.sqrMagnitude < 0.001f)
			return;

		float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
		Quaternion targetRot = Quaternion.Euler(0, 0, angle - 90f);

		// ★ ここを追加：スムーズに回転
		spriteRoot.rotation = Quaternion.Slerp(
			spriteRoot.rotation,
			targetRot,
			Time.deltaTime * 10f   // ← 回転速度（調整可）
		);
	}

	bool CanSeePlayer()
	{
		if (target == null)
			return false;

		Vector2 pos = transform.position;
		Vector2 playerPos = target.position;
		Vector2 dirToPlayer = (playerPos - pos).normalized;

		float distanceToPlayer = Vector2.Distance(pos, playerPos);
		if (distanceToPlayer > viewDistance)
			return false;

		Vector2 forward = spriteRoot.up;
		float angle = Vector2.Angle(forward, dirToPlayer);
		if (angle > viewAngle * 0.5f)
			return false;

		RaycastHit2D hit = Physics2D.Raycast(pos, dirToPlayer, distanceToPlayer, obstacleMask);
		if (hit && hit.collider.transform != target)
			return false;

		return true;
	}

	void GoToNextPatrolPoint()
	{
		if (patrolPoints.Length == 0) return;

		agent.SetDestination(patrolPoints[currentPatrolIndex].position);
		currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
	}

	public void StopMovementFor(float duration)
	{
		if (stopRoutine != null)
			StopCoroutine(stopRoutine);

		stopRoutine = StartCoroutine(StopMovementRoutine(duration));
	}

	private IEnumerator StopMovementRoutine(float duration)
	{
		IsMovementStopped = true;

		agent.velocity = Vector3.zero;
		agent.ResetPath();

		yield return new WaitForSeconds(duration);

		IsMovementStopped = false;
	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;

		Vector2 moveDir = agent.velocity.sqrMagnitude > 0.01f
			? agent.velocity.normalized
			: lastMoveDir;

		Gizmos.color = new Color(1f, 1f, 0f, 0.25f);

		int segments = 30;
		float halfAngle = viewAngle * 0.5f;

		Vector3 origin = transform.position;

		for (int i = 0; i < segments; i++)
		{
			float angle1 = -halfAngle + (viewAngle / segments) * i;
			float angle2 = -halfAngle + (viewAngle / segments) * (i + 1);

			Vector3 dir1 = Quaternion.Euler(0, 0, angle1) * moveDir;
			Vector3 dir2 = Quaternion.Euler(0, 0, angle2) * moveDir;

			Vector3 p1 = origin + dir1 * viewDistance;
			Vector3 p2 = origin + dir2 * viewDistance;

			Gizmos.DrawLine(origin, p1);
			Gizmos.DrawLine(p1, p2);
		}

		Gizmos.color = Color.red;
		Gizmos.DrawLine(origin, origin + (Vector3)moveDir * viewDistance);
	}
}
