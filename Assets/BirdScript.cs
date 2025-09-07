using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrenght;
    public LogicScript logic;
    public bool birdIsAlive = true;


    // Awake se llama justo al iniciar el objeto, antes que Start.
    // Es el lugar ideal para buscar componentes.
    void Awake()
    {
        // Esta línea busca el componente Rigidbody2D en el mismo GameObject
        // y lo asigna a tu variable 'myRigidbody'.
        myRigidbody = GetComponent<Rigidbody2D>();
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            // Pone la velocidad vertical en 0 antes de saltar
            myRigidbody.linearVelocity = Vector2.zero;

            // Aplica la nueva fuerza hacia arriba
            myRigidbody.linearVelocity = Vector2.up * flapStrenght;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birdIsAlive = false;
    }
}
