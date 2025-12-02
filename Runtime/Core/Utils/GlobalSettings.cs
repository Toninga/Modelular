

using System.Diagnostics;
using static Codice.CM.Common.CmCallContext;

namespace Modelular.Runtime
{
    public static class GlobalSettings
    {

        public static int WarningVertexCount { get; set; } = 100000;
        public static int ErrorVertexCount { get; set; } = 300000;

        public static string MaximumVertexCountExceededException(int vertexCount)
        {
            throw new MaximumVertexCountExceededException( "[Modelular] The vertex count exceeded the maximum allowed vertex count : " + vertexCount + " > " + ErrorVertexCount
                + ". You can override the maximum vertex count by accessing GlobalSettings.ErrorVertexCount");
        }
        public static void MaximumVertexCountExceededWarning(int vertexCount)
        {
            UnityEngine.Debug.LogWarning( "[Modelular] The vertex count exceeded the maximum advised vertex count : " + vertexCount + " > " + WarningVertexCount
                + ". You can override the threshold for the vertex count warning by accessing GlobalSettings.WarningVertexCount");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexCount"></param>
        /// <returns>True if a warning is sent, false if no threshold is met, error otherwise</returns>
        public static bool DetectVertexCountLimitations(int vertexCount)
        {
            if (vertexCount > ErrorVertexCount)
                MaximumVertexCountExceededException(vertexCount);
            if (vertexCount >  WarningVertexCount)
            {
                MaximumVertexCountExceededWarning(vertexCount);
                return true;
            }
            return false;
        }
    }
}