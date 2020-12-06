using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace SonicBloom.Koreo.Demos
{
	[AddComponentMenu("Koreographer/Demos/Rhythm Game/Rhythm Game Controller")]
	public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] GameObject _platformPrefab;
        [SerializeField] GameObject _contnerPlatforms;

		#region Fields

		[Tooltip("The Event ID of the track to use for target generation.")]
		[EventID]
		public string eventID;

		[Tooltip("The number of milliseconds (both early and late) within which input will be detected as a Hit.")]
		[Range(8f, 150f)]
		public float hitWindowRangeInMS = 80;

		[Tooltip("The number of units traversed per second by Note Objects.")]
		public float noteSpeed = 1f;

		[Tooltip("The archetype (blueprints) to use for generating notes.  Can be a prefab.")]
		public NotePrefab noteObjectArchetype;

		[Tooltip("The list of Lane Controller objects that represent a lane for an event to travel down.")]
		public List<PositionNotesControl> _positionNotesControls = new List<PositionNotesControl>();

		[Tooltip("The amount of time in seconds to provide before playback of the audio begins.  Changes to this value are not immediately handled during the lead-in phase while playing in the Editor.")]
		public float leadInTime;

		[Tooltip("The Audio Source through which the Koreographed audio will be played.  Be sure to disable 'Auto Play On Awake' in the Music Player.")]
		public AudioSource audioCom;

		// La quantité de leadInTime restant avant que l'audio ne soit audible.
		float leadInTimeLeft;

		// Le temps restant avant de jouer l'audio (gère le délai d'événement).
		float timeLeftToPlay;

		// Cache local de la Koreography chargé dans le composant Koreographer.
		Koreography playingKoreo;

		// Koreographer travaille dans des échantillons. Convertissez les valeurs de l'utilisateur en temps d'échantillonnage. Cela simplifiera
		// calculs partout.
		int hitWindowRangeInSamples;    // La plage d'échantillons dans laquelle un événement viable peut être atteint.

		// Le pool pour contenir des objets de note pour réduire l'instanciation / destruction inutile.
		Stack<NotePrefab> noteObjectPool = new Stack<NotePrefab>();

		#endregion
		#region Properties

		// Accès public à la fenêtre de hit.
		public int HitWindowSampleWidth
		{
			get
			{
				return hitWindowRangeInSamples;
			}
		}

		// Accès à la taille actuelle de la fenêtre de hit en unités Unity.
		public float WindowSizeInUnits
		{
			get
			{
				return noteSpeed * (hitWindowRangeInMS * 0.001f);
			}
		}

		// Le taux d'échantillonnage spécifié par la Koreography.
		public int SampleRate
		{
			get
			{
				return playingKoreo.SampleRate;
			}
		}

		// L'heure d'échantillonnage actuelle, y compris les retards nécessaires.
		public int DelayedSampleTime
		{
			get
			{
				// Décalage de l'heure indiquée par Koreographer par un possible montant leadInTime.
				return playingKoreo.GetLatestSampleTime() - (int)(audioCom.pitch * leadInTimeLeft * SampleRate);
			}
		}

		#endregion
		#region Methods

		void Start()
		{
			InitializeLeadIn();

			// Initialise toutes les voies.
			for (int i = 0; i < _positionNotesControls.Count; ++i)
			{
				_positionNotesControls[i].Initialize(this);
			}

			// Initialise les événements.
			playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

			// Récupérez tous les événements de la Koreography.
			KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(eventID);
			List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

			for (int i = 0; i < rawEvents.Count; ++i)
			{
				KoreographyEvent evt = rawEvents[i];
				string payload = evt.GetTextValue();

				// Trouvez la bonne voie.
				for (int j = 0; j < _positionNotesControls.Count; ++j)
				{
					PositionNotesControl lane = _positionNotesControls[j];
					if (lane.DoesMatchPayload(payload))
					{
						// Ajout de l'objet pour le suivi des entrées.
						lane.AddEventToLane(evt);

						// Sortez de la boucle de recherche de voie.
						break;
					}
				}
			}
		}

		// Configure le lead-in-time. Démarre immédiatement la lecture audio si le délai spécifié est égal à zéro.
		void InitializeLeadIn()
		{
			// Initialize the lead-in-time only if one is specified.
			if (leadInTime > 0f)
			{
				// Initialise le délai d'entrée uniquement si un est spécifié.
				leadInTimeLeft = leadInTime;
				timeLeftToPlay = leadInTime - Koreographer.Instance.EventDelayInSeconds;
			}
			else
			{
				// Joue immédiatement et gère le décalage dans la chanson. Le zéro négatif est le même que
				// zéro donc ce n'est pas un problème.
				audioCom.time = -leadInTime;
				audioCom.Play();
			}
		}

		void Update()
		{
			// Cela devrait être fait dans Start (). Nous le faisons ici pour permettre des tests avec les modifications d'Inspector.
			UpdateInternalValues();

			// Compte à rebours certains de nos délais.
			if (leadInTimeLeft > 0f)
			{
				leadInTimeLeft = Mathf.Max(leadInTimeLeft - Time.unscaledDeltaTime, 0f);
			}

			// Compte à rebours le temps restant pour jouer, si nécessaire.
			if (timeLeftToPlay > 0f)
			{
				timeLeftToPlay -= Time.unscaledDeltaTime;

				// Vérifie s'il est temps de commencer la lecture.
				if (timeLeftToPlay <= 0f)
				{
					audioCom.time = -timeLeftToPlay;
					audioCom.Play();

					timeLeftToPlay = 0f;
				}
			}
		}

		// Mettre à jour toutes les valeurs internes qui dépendent de champs accessibles de l'extérieur (public ou piloté par l'inspecteur).
		void UpdateInternalValues()
		{
			hitWindowRangeInSamples = (int)(0.001f * hitWindowRangeInMS * SampleRate);
		}

		// Récupère un objet Note activé fréquemment à partir du pool.
		public NotePrefab GetFreshNoteObject()
		{
			NotePrefab retObj;

			if (noteObjectPool.Count > 0)
			{
				retObj = noteObjectPool.Pop();
			}
			else
			{
				retObj = GameObject.Instantiate<NotePrefab>(noteObjectArchetype);
			}

			retObj.gameObject.SetActive(true);
			retObj.enabled = true;

			return retObj;
		}

		// Désactive et renvoie un objet Note au pool.
		public void ReturnNoteObjectToPool(NotePrefab obj)
		{
			if (obj != null)
			{
				obj.enabled = false;
				obj.gameObject.SetActive(false);

				noteObjectPool.Push(obj);
			}
		}

		// Redémarre le jeu, provoquant la réinitialisation ou l'effacement de toutes les voies et de tous les objets de note actifs.
		public void Restart()
		{
			// Réinitialise l'audio.
			audioCom.Stop();
			audioCom.time = 0f;

			// Vide la file d'attente des mises à jour d'événements différées. Cela réinitialise efficacement la korégraphie et garantit que
			// Les événements retardés qui n'ont pas encore été envoyés ne continuent pas à être envoyés.
			Koreographer.Instance.FlushDelayQueue(playingKoreo);

			// Réinitialise l'heure de la coréographie. Ceci est généralement géré en chargeant la Koreography. Comme nous sommes simplement
			// redémarrage, nous devons gérer cela nous-mêmes.
			playingKoreo.ResetTimings();

			// Réinitialise toutes les voies pour que le suivi recommence.
			for (int i = 0; i < _positionNotesControls.Count; ++i)
			{
				_positionNotesControls[i].Restart();
			}

			// Réinitialise le lead-in-timing.
			InitializeLeadIn();
		}

		#endregion
	}
}

