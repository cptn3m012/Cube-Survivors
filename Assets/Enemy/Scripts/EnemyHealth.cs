using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject DamageTextPrefab;
    [SerializeField]
    private float health; // Zdrowie moba
    private Animator animator; // Animator moba
    public bool isDead = false; // Dodajemy now� zmienn� boolowsk�
    private GameObject currentDamageText; // Aktualny tekst obra�e�
    private float damageTextOffset = 0.5f; // Przesuni�cie tekstu obra�e�
    [SerializeField]
    private float baseTextHeight;
    private float damageTextHeight; // Pocz�tkowa wysoko�� tekstu obra�e�
    int AddHeight = 0;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Pobierz komponent Animator
        damageTextHeight = baseTextHeight;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null && !isDead) // Sprawdzamy, czy mob nie jest martwy
        {
            TakeDamage(bullet.damage);
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage; // Zmniejsz zdrowie o warto�� obra�e�
        ShowDamageText();

        if (health <= 0 && !isDead) // Sprawdzamy, czy mob nie jest martwy
        {
            Die(); // Je�li zdrowie <= 0, mob umiera
        }
    }

    void ShowDamageText()
    {
        Vector3 textPosition = new Vector3(transform.position.x, transform.position.y + damageTextHeight, transform.position.z); // Pozycja tekstu nad g�ow�
        currentDamageText = Instantiate(DamageTextPrefab, textPosition, Quaternion.identity, transform);
        currentDamageText.transform.LookAt(Camera.main.transform); // Skieruj tekst w stron� kamery
        currentDamageText.transform.rotation = Quaternion.Euler(60, 0, 0); // Obr�� tekst, aby by� r�wnoleg�y do kamery
        currentDamageText.GetComponent<TextMeshPro>().text = health < 0 ? "0" : health.ToString(); // Ustaw tekst obra�e�
        currentDamageText.GetComponent<TextMeshPro>().color = Color.red; // Ustaw kolor na czerwony
        if (AddHeight == 3)
        {
            damageTextHeight = baseTextHeight;
            AddHeight = 0;
        }
        else
        {
            damageTextHeight += damageTextOffset;
            AddHeight++;
        }
        Destroy(currentDamageText, 1f); // Zniszcz tekst obra�e� po 1 sekundzie
    }

    private void Die()
    {
        isDead = true; // Ustawiamy isDead na true, gdy mob umiera
        if (animator != null)
        {
            animator.Play("Death"); // Uruchom animacj� �mierci
        }
        Invoke("DestroyObject", 1f); // Zniszcz obiekt po 2 sekundach
        SpawnerScript.currentEnemies--; // Dekrementuj liczb� wrog�w
    }

    private void DestroyObject()
    {
        Destroy(gameObject); // Zniszcz obiekt
    }
}