// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformerGameKit
{
    /// <summary>Loads the next example scene when <see cref="OnTriggerEnter2D"/> is caused by the player.</summary>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/NextExampleTrigger
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Next Example Trigger")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(NextExampleTrigger))]
    public sealed class NextExampleTrigger : MonoBehaviour
    {
        /************************************************************************************************************************/

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                var scene = SceneManager.GetActiveScene();
#if UNITY_EDITOR
                // Unity uses forward slashes but Windows uses back slashes.
                // Then after we load a scene using back slashes, Unity will be using that exact path.
                // So we just convert all slashes to forward slashes to avoid inconsistency.

                var path = scene.path.Replace('\\', '/');
                var directory = Path.GetDirectoryName(path);

                // Figure out which file is the current scene.
                var files = Directory.GetFiles(directory, "*.unity");
                var index = -1;
                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i].Replace('\\', '/');
                    if (file == path)
                    {
                        index = i;
                        break;
                    }
                }

                // Move to the next file.
                index++;
                if (index >= files.Length)
                    index = 0;

                UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(files[index], default);
#else
                SceneManager.LoadScene(scene.buildIndex + 1);
#endif
            }
        }

        /************************************************************************************************************************/
    }
}