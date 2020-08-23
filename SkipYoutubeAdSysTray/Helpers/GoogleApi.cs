using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipYoutubeAdSysTray.Helpers
{
    public class GoogleApi
    {
        private readonly ImageAnnotatorClient _apiClient;

        private const string GoogleApiCredentialsPath = "<full path to a json project secret. More on https://cloud.google.com/vision/docs/before-you-begin>";

        public GoogleApi()
        {
            _apiClient = new ImageAnnotatorClientBuilder
            {
                CredentialsPath = GoogleApiCredentialsPath
            }
          .Build();
        }

        public async Task<Vertex> SkipAdImageLocationAsync(string imageUri)
        {
            var image = await Image.FromFileAsync(imageUri);

            var req = new AnnotateImageRequest
            {
                Image = image,
                Features = { new Feature { Type = Feature.Types.Type.DocumentTextDetection } }
            };

            var resp = await _apiClient.AnnotateAsync(req);

            for (var i = 0; i < resp.TextAnnotations.Count - 1; ++i)
            {
                var annCurr = resp.TextAnnotations[i];
                var annNext = resp.TextAnnotations[i + 1];

                if (IsSkipAdSequence(annCurr.Description, annNext.Description))
                    return annCurr.BoundingPoly.Vertices.First();
            }
            return null;
        }
        private bool IsSkipAdSequence(string annCurr, string annNext)
            => annCurr == "Skip" && (annNext == "ad" || annNext == "Ads");
    }
}
