using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Pavantares.CCG.Services
{
    public class PictureService
    {
        public async UniTask<Texture> Load()
        {
            using var request = UnityWebRequestTexture.GetTexture("https://picsum.photos/200/300");
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                return DownloadHandlerTexture.GetContent(request);
            }

            return null;
        }
    }
}
