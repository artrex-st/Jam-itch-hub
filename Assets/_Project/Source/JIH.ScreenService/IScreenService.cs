using Coimbra.Services;
using Source;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.ScreenService
{
    public interface IScreenService : IService
    {
        public List<ScreenReference> Levels { get; }

        public void Initialize(List<ScreenReference> levelsList);
        public AsyncOperation LoadSingleSceneAsync(ScreenReference sceneReference);
        public void LoadSingleScene(ScreenReference sceneReference);
        public AsyncOperation LoadAdditiveSceneAsync(ScreenReference sceneReference);
        public AsyncOperation UnLoadSceneAsync(ScreenReference sceneReference);
    }
}
