using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class MusicManager : MonoBehaviour {
    public static MusicManager Instance;
    [SerializeField] private AudioClip[] _gameMusic;

    private void Start() {
        if (Instance == null)
            Instance = this;

        PlayMyMusic(0);
    }

    private void PlayMyMusic(int musicIndex) {
        MMSoundManagerSoundPlayEvent.Trigger(_gameMusic[musicIndex], MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, persistent: true);
    }

    private void StopMyMusic() {
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 0, 1f, 0, MMTweenType.DefaultEaseInCubic);
        //MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, 0);
    }

    public void ChangeMusic(int musicIndex) {
        StopMyMusic();
        PlayMyMusic(musicIndex);
    }
}