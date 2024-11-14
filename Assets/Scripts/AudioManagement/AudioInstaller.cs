using UnityEngine;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    [SerializeField] private AudioController audioController;
    
    public override void InstallBindings()
    {
        Container.Bind<AudioController>().FromInstance(audioController).AsSingle();
    }
}