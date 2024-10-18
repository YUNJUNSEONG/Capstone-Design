using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed = 10.0f;  // 투사체 속도
    public float range = 10.0f;  // 사거리

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 투사체를 전방으로 이동 (일직선 이동)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 사거리 초과 시 투사체 삭제
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 시 데미지 처리
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject); // 충돌 후 투사체 삭제
        }
        // 장애물과 충돌 시 투사체 삭제
        /*
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }*/
    }
}
