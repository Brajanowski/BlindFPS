using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Rendering
{
    public class BlindFPSRenderPipeline : RenderPipeline
    {
        private BlindFPSRenderPipelineAsset _pipelineAsset;
        private CameraRenderer _cameraRenderer;

        public BlindFPSRenderPipeline(BlindFPSRenderPipelineAsset pipelineAsset)
        {
            _pipelineAsset = pipelineAsset;
            _cameraRenderer = new CameraRenderer(_pipelineAsset);
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (Camera camera in cameras)
            {
#if UNITY_EDITOR
                if (camera.cameraType == CameraType.SceneView)
                {
                    SceneCameraPass(context, camera);
                    return;
                }
#endif

                if (camera.cameraType == CameraType.Game)
                {
                    BlindFPSCameraData cameraData = camera.GetComponent<BlindFPSCameraData>();
                    if (cameraData == null)
                    {
                        cameraData = camera.gameObject.AddComponent<BlindFPSCameraData>();
                    }

                    _cameraRenderer.RenderCamera(context, cameraData);
                }
            }
        }

#if UNITY_EDITOR
        private void SceneCameraPass(ScriptableRenderContext context, Camera camera)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);

            context.SetupCameraProperties(camera);

            CommandBuffer cmd = CommandBufferPool.Get();
            cmd.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            cmd.ClearRenderTarget(true, true, camera.backgroundColor);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParameters);
            CullingResults cullingResults = context.Cull(ref cullingParameters);

            SortingSettings opaqueSortingSettings = new(camera);
            opaqueSortingSettings.criteria = SortingCriteria.CommonOpaque;

            DrawingSettings opaqueDrawingSettings = new DrawingSettings(new ShaderTagId("BlindFPS_SceneView"), opaqueSortingSettings);
            opaqueDrawingSettings.enableDynamicBatching = false;
            opaqueDrawingSettings.enableInstancing = false;
            opaqueDrawingSettings.perObjectData = PerObjectData.None;

            FilteringSettings opaqueFilteringSettings = new(RenderQueueRange.opaque);
            context.DrawRenderers(cullingResults, ref opaqueDrawingSettings, ref opaqueFilteringSettings);
            
            context.DrawSkybox(camera);

            SortingSettings transparentSortingSettings = new(camera);
            transparentSortingSettings.criteria = SortingCriteria.CommonTransparent;
            opaqueSortingSettings.criteria = SortingCriteria.CommonOpaque;

            DrawingSettings transparentDrawingSettings = new DrawingSettings(new ShaderTagId("SRPDefaultUnlit"), transparentSortingSettings);
            transparentDrawingSettings.enableDynamicBatching = false;
            transparentDrawingSettings.enableInstancing = false;
            transparentDrawingSettings.perObjectData = PerObjectData.None;
            
            FilteringSettings transparentFilteringSettings = new(RenderQueueRange.opaque);
            transparentFilteringSettings.renderQueueRange = RenderQueueRange.transparent;
    
            context.DrawRenderers(cullingResults, ref transparentDrawingSettings, ref transparentFilteringSettings);

            if (Handles.ShouldRenderGizmos())
            {
                context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
                context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
            }

            context.Submit();
        }
#endif
    }
}