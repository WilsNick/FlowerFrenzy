using UnityEngine;

public class Despawner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Wind"))
        {
            Destroy(other.gameObject);
            return;
        }
        
        if(gameObject.name == "Top"){
            switch (other.tag)
            {
                case "Obstacle":
                    Destroy(other.transform.parent.gameObject);
                    break;
                case "Cloud":
                    Destroy(other.gameObject);
                    break;
                case "Flower":
                    Destroy(other.gameObject);
                    break;
                case "Coin":
                    Destroy(other.gameObject);
                    break;
            }
        }
        
        if(gameObject.name == "BorderDespawner" && other.CompareTag("Border")){
            Destroy(other.transform.parent.gameObject);
        }
    }
}
