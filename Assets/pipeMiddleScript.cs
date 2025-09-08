using UnityEngine;

public class pipeMiddleScript : MonoBehaviour
{
    public LogicScript logic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        // Añadimos un mensaje para saber si encontró el objeto
        if (logic != null)
        {
            Debug.Log("Logic Script Encontrado!");
        }
        else
        {
            Debug.LogError("ERROR: No se pudo encontrar el Logic Script. Revisa la etiqueta del Logic Manager.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

private void OnTriggerEnter2D(Collider2D collision)
{
    // Mantenemos este primer debug para saber si CUALQUIER cosa activa el trigger
    Debug.Log("Trigger Activado por: " + collision.gameObject.name); 
    
    // Ahora comprobamos si lo que entró tiene la etiqueta "Player"
    if (collision.gameObject.tag == "Player")
    {
        // Este nuevo debug nos dirá si la etiqueta es correcta
        Debug.Log("¡Colisión con el Player detectada!"); 
        logic.addScore();
        
        // Llama a la función de partículas en el LogicScript, pasándole su propia posición
        if (LogicScript.Instance != null)
        {
            LogicScript.Instance.TriggerScoreParticles(transform.position);
        }
    }
}
}