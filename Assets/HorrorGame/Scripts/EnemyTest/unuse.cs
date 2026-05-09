using UnityEngine;

public class EnemyPatrolRoute3D : MonoBehaviour
{
	public Transform[] waypoints; // 巡回ポイント
	public Transform player;

	public float patrolSpeed = 2f; // 巡回中速度
	public float chaseSpeed = 4f;// 追跡中速度
	public float waitTime = 1f;   // 各ポイントで止まる時間

	public float detectionRange = 5f; // 発見距離

	private int currentIndex = 0;
	private float waitCounter = 0f;
	private bool isWaiting = false;

	private enum State { Patrol, Chase }
	private State currentState = State.Patrol;

	// プレイヤー視認
	bool CanSeePlayer()
	{
		Vector3 direction = (player.position - transform.position).normalized;

		RaycastHit hit;

		if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
		{
			// 最初に当たったものがプレイヤーならOK
			if (hit.transform == player)
			{
				return true;
			}
		}
		Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
		return false;
	}
	void Update()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer < detectionRange && CanSeePlayer())
		{
			currentState = State.Chase;
		}
		else
		{
			currentState = State.Patrol;
		}

		if (currentState == State.Patrol)
		{
			Patrol();
		}
		else if (currentState == State.Chase)
		{
			Chase();
		}
	}

	void Patrol() // 巡回
	{
		// 待機状態制御
		if (waypoints.Length == 0) return;

		if (isWaiting)
		{
			waitCounter -= Time.deltaTime;
			if (waitCounter <= 0f)
			{
				isWaiting = false;
				currentIndex = (currentIndex + 1) % waypoints.Length;
			}
			return;
		}
		// 巡回ポイント決定
		Vector3 targetPos = waypoints[currentIndex].position;
		targetPos.y = transform.position.y;

		Move(targetPos, patrolSpeed);
		// 到達判定
		if (Vector3.Distance(transform.position, targetPos) < 0.1f)
		{
			isWaiting = true;
			waitCounter = waitTime;
		}
	}

	void Chase() // 追跡
	{
		Vector3 targetPos = player.position;
		targetPos.y = transform.position.y;

		Move(targetPos, chaseSpeed);
	}

	void Move(Vector3 target, float speed) // 移動処理
	{
		transform.position = Vector3.MoveTowards(
			transform.position,
			target,
			speed * Time.deltaTime
		);

		Vector3 dir = target - transform.position;
		dir.y = 0;

		if (dir.magnitude > 0.2f)
		{
			Quaternion targetRotation = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				targetRotation,
				5f * Time.deltaTime
			);
		}
	}
}