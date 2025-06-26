using UnityEngine;
using CustomSystem.Timer;
using CustomSystem.Debug;

public class ExampleTimer : MonoBehaviour
{
    // Commencer par créer une variable de la classe TimerSystem, un par différent timer souhaiter
    TimerSystem timer;

    void Start()
    {
        DebugSystem.EnableGlobalDebug(true);
        // Créer ensuite le timer en ajoutant le monobehaviour de ce script et les différentes fonction jouer au différent evenement
        timer = new TimerSystem((sec) => Debug.Log($"This is second {sec}"), () => Debug.Log("Done"));

        // Lancer le timer avec le temps souhaité, un seul a la fois 
        timer.StartTimer(10f, true);

        Invoke("Stop", 5f);
        Invoke("Continue", 8f);
    }

    void Update()
    {
        DebugSystem.Log("Super Test", Color.green);
    }

    private void Stop()
    {


        // Possibilité de stopper le timer quand on le souhaite, seulement si un timer est lancé
        timer.StopTimer();
    }

    private void Continue()
    {
        // Possibilité de continuer le timer quand on le souhaite, seulement si un timer est arreté
        timer.ContinueTimer();
    }
}
