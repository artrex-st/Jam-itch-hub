using Coimbra;
using Source;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JIH.ScreenService
{
    public class ScreenService : Actor, IScreenService
    {
        private ScreenReference _currentSceneReference;

        public List<ScreenReference> Levels { get; private set; }

        public void Initialize(List<ScreenReference> levelsList)
        {
            Levels = levelsList;
        }

        public AsyncOperation LoadSingleSceneAsync(ScreenReference sceneReference)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneReference.SceneName, LoadSceneMode.Single);
            return asyncOperation;
        }

        public void LoadSingleScene(ScreenReference sceneReference)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneReference.SceneName, LoadSceneMode.Single);
            _currentSceneReference = sceneReference;
            asyncOperation.completed += AsyncOperationOnCompleted;
        }

        public AsyncOperation LoadAdditiveSceneAsync(ScreenReference sceneReference)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneReference.SceneName, LoadSceneMode.Additive);
            return asyncOperation;
        }

        public AsyncOperation UnLoadSceneAsync(ScreenReference sceneReference)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneReference.SceneName);
            return asyncOperation;
        }

        private void AsyncOperationOnCompleted(AsyncOperation obj)
        {
            if (!string.IsNullOrEmpty(_currentSceneReference.LevelTitle))
            {
                new RequestLevelNameEvent(_currentSceneReference.LevelTitle).Invoke(this);
            }
        }
    }
}
