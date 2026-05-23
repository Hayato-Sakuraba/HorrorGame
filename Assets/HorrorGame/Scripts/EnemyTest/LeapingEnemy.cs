using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemy : MonoBehaviour
{
	public Transform player;
	private NavMeshAgent agent;

	[Header("Movement")]
	public float walkSpeed = 1.5f;
	public float chaseDistance = 6f;

	[Header("Leap Attack")]
	public float leapSpeed = 12f;
	public float leapDuration = 0.35f;
	public float leapCooldown = 3f;
	public float leapRange = 5f;

	[Header("Timing")]
	public float preLeapDelay = 0.15f;
	public float postLeapStun = 0.25f;

	private float leapTimer = 0f;
	private bool isLeaping = false;

	//飛びかかり方向を保存する
	private Vector2 leapDirection;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

		agent.speed = walkSpeed;

		agent.acceleration = 999f;
		agent.angularSpeed = 999f;
	}

	void Update()
	{
		leapTimer += Time.deltaTime;
		float dist = Vector2.Distance(transform.position, player.position);

		if (isLeaping)
		{
			//飛びかかり中はプレイヤーを追わず、固定方向へ直線移動
			Vector3 targetPos = (Vector2)transform.position + leapDirection * 10f;
			agent.SetDestination(targetPos);
			return;
		}

		// 通常追跡
		if (dist < chaseDistance)
		{
			agent.SetDestination(player.position);
		}

		// 飛びかかり条件
		if (dist < leapRange && leapTimer >= leapCooldown)
		{
			StartCoroutine(LeapAttack());
		}
	}

	private System.Collections.IEnumerator LeapAttack()
	{
		isLeaping = true;
		leapTimer = 0f;

		float originalSpeed = agent.speed;

		// -------------------------
		// 溜め（停止）
		// -------------------------
		agent.isStopped = true;
		yield return new WaitForSeconds(preLeapDelay);

		// -------------------------
		// 飛びかかり方向を確定
		// -------------------------
		leapDirection = (player.position - transform.position).normalized;

		// -------------------------
		// 飛びかかり開始（直線移動）
		// -------------------------
		agent.isStopped = false;
		agent.speed = leapSpeed;

		yield return new WaitForSeconds(leapDuration);

		// -------------------------
		// 着地後の硬直
		// -------------------------
		agent.isStopped = true;
		agent.speed = 0f;
		yield return new WaitForSeconds(postLeapStun);

		// -------------------------
		// 通常状態に戻る
		// -------------------------
		agent.isStopped = false;
		agent.speed = originalSpeed;

		isLeaping = false;
	}
}