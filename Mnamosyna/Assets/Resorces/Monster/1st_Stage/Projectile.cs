using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed = 10.0f;
    public float range = 10.0f;
    public float dropSpeed = 2.0f; // ����ü�� �������� �ӵ�

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // �̵� ó��
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // �������� �������� ó��
        transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);

        // ��Ÿ� �ʰ� �� ����ü ����
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹 �� ������ ó��
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // �浹 �� ����ü ����
        }
    }
}
