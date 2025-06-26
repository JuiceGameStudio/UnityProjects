using System;
using System.Collections;
using UnityEngine;
using static MonoBehaviourUtility;

namespace CustomSystem.Timer
{
    /// <summary>
    /// Classe pour créer des timer
    /// </summary>
    public class TimerSystem
    {
        // Référence de mon monobehaviour sur lequelle est exécuté le timer
        MonoBehaviour myMB;
        // Stock la couroutine actuellement exécuté
        IEnumerator currentTimer;
        // Définis si l'on as stopper ou non le timer
        bool haveStop = false;
        // Définis le temps par défaut au qu'elle commencer du timer actuelle
        float defaultTime;
        // Définis la durer du timer actuelle 
        float duration;
        // Definis si le timer a compte a rebourd
        bool negate;

        // Event du temps qui passe en seconde
        Action<int> TimerChanged;
        // Event quand le timer a finis
        Action TimerEnded;

        #region public 

        /// <summary>
        /// Créer la classe timer et lie les différents événement lié au timer
        /// </summary>
        /// <param name="TimerChanged">Event appelé quand le temps change (chaque seconde)</param>
        /// <param name="TimerEnded">Event appelé quand le timer est finis</param>
        public TimerSystem( Action<int> TimerChanged, Action TimerEnded)
        {
            this.myMB = GetCallingMonoBehaviour();
            this.TimerChanged = TimerChanged;
            this.TimerEnded = TimerEnded;
        }

        /// <summary>
        /// Démare un timer avec un temps infinie
        /// </summary>
        public void StartInfiniteTimer()
        {
            StartCouroutine(Mathf.Infinity);
        }

        /// <summary>
        /// Démare un timer avec un temps spécifique
        /// </summary>
        /// <param name="duration">Le temps souhaiter du timer (en seconde)</param>
        /// <param name="negate">Si vrais, lance le timer a compte a rebourd (faux par défaut)</param>
        public void StartTimer(float duration, bool negate = false)
        {
            StartCouroutine(duration, negate);
        }

        /// <summary>
        /// Arret le timer en cours
        /// </summary>
        public void StopTimer()
        {
            if (currentTimer != null)
            {
                haveStop = true;
                myMB.StopCoroutine(currentTimer);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"{typeof(TimerSystem).Name} : you can't stop a timer if no one was start");
            }
        }

        /// <summary>
        /// Remet a zéro le timer en cours
        /// </summary>
        public void ResetTimer()
        {
            if (currentTimer != null)
            {
                currentTimer?.Reset();
            }
            else
            {
                UnityEngine.Debug.LogWarning($"{typeof(TimerSystem).Name} : you can't reset a timer if no one was start");
            }
        }

        /// <summary>
        /// Continue le timer en cours
        /// </summary>
        public void ContinueTimer()
        {
            if (haveStop)
            {
                haveStop = false;
                currentTimer = TickTimer(duration, defaultTime, negate);
                myMB.StartCoroutine(currentTimer);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"{typeof(TimerSystem).Name} : you can't continue a timer if no one was stopped");
            }
        }

        /// <summary>
        /// Convertie les secondes en minutes
        /// </summary>
        /// <param name="currentTime">Le temps en seconde a convertir</param>
        /// <returns>Le nouveaux temps en minute</returns>
        public int ConvertSecondToMinute(int currentTime)
        {
            return Mathf.FloorToInt(currentTime / 60);
        }

        /// <summary>
        /// Convertie les secondes en heures
        /// </summary>
        /// <param name="currentTime">Le temps en seconde a convertir</param>
        /// <returns>Le nouveaux temps en heures</returns>
        public int ConvertSecondToHours(int currentTime)
        {
            return Mathf.FloorToInt(currentTime / 3600);
        }

        /// <summary>
        /// Convertie les minutes en heures
        /// </summary>
        /// <param name="currentTime">Le temps en minutes a convertir</param>
        /// <returns>Le nouveaux temps en heures</returns>
        public int ConvertMinuteToHours(int currentTime)
        {
            return Mathf.FloorToInt(currentTime / 60);
        }

        #endregion

        #region private

        /// <summary>
        /// Lance la couroutine du timer
        /// </summary>
        /// <param name="duration">Le temps de la couroutine du timer (en seconde)</param>
        /// <param name="negate">Si vrais, lance le timer a compte a rebourd (faux par défaut)</param>
        void StartCouroutine(float duration, bool negate = false)
        {
            this.duration = duration;
            this.negate = negate;
            if (currentTimer != null)
            {
                myMB.StopCoroutine(currentTimer);
            }
            currentTimer = TickTimer(duration, 0, negate);
            myMB.StartCoroutine(currentTimer);
        }

        /// <summary>s
        /// Lance le timer
        /// </summary>
        /// <param name="duration">Le temps du timer (en seconde)</param>
        /// <param name="defaultTime">Le temps auqu'elle commence le timer (en seconde)</param>
        /// <param name="negate">Si vrais, lance le timer a compte a rebourd (faux par défaut)</param>
        IEnumerator TickTimer(float duration, float defaultTime = 0, bool negate = false)
        {
            int previousIntPart = (int)defaultTime;
            TimerChanged?.Invoke(negate ? (int)duration - previousIntPart : previousIntPart);
            for (float t = defaultTime; t <= duration; t += Time.deltaTime)
            {
                int newIntPart = (int)t;
                if (newIntPart != previousIntPart)
                {
                    previousIntPart = newIntPart;
                    this.defaultTime = t;
                    TimerChanged?.Invoke(negate ? (int)duration - newIntPart : newIntPart);
                }
                yield return null;
            }

            TimerChanged?.Invoke(negate ? 0 : (int)duration);
            TimerEnded?.Invoke();
            this.defaultTime = 0;
        }

        #endregion
    }
}

