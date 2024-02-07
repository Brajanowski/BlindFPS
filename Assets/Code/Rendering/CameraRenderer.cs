using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Rendering
{
    public class CameraRenderer
    {
        private static readonly ShaderTagId ShaderTagFillGBuffer = new ShaderTagId("BlindFPS_GBufferFill");
        private static readonly ShaderTagId ShaderTagViewmodel = new ShaderTagId("BlindFPS_Viewmodel");

        private static readonly int EndFallOffDistanceShaderId = Shader.PropertyToID("_EndFallOffDistance");
        private static readonly int StartFallOffDistanceShaderId = Shader.PropertyToID("_StartFallOffDistance");
        private static readonly int BaseColorShaderId = Shader.PropertyToID("_BaseColor");

        // GBuffer
        private RenderTexture _albedoBuffer; // also contains depth buffer
        private RenderTexture _normalsBuffer;
        private RenderTexture _positionBuffer;

        private RenderTexture _visibilityBuffer;

        private Material _accumulateVisibilityBufferMaterial;
        private Material _finalFrameMaterial;

        private BlindFPSRenderPipelineAsset _pipelineAsset;

        public CameraRenderer(BlindFPSRenderPipelineAsset pipelineAsset)
        {
            _pipelineAsset = pipelineAsset;
            _finalFrameMaterial = new Material(Shader.Find("Hidden/BlindFPS/FinalFrame"));
            _accumulateVisibilityBufferMaterial = new Material(Shader.Find("Hidden/BlindFPS/VisibilityBuffer"));
        }

        private const int AlbedoAttachmentIndex = 0;
        private const int NormalsAttachmentIndex = 1;
        private const int WorldPositionAttachmentIndex = 2;
        private const int VisibilityBufferAttachmentIndex = 3;
        private const int DepthAttachmentIndex = 4;
        private const int OutputAttachmentIndex = 5;

        public void RenderCamera(ScriptableRenderContext context, BlindFPSCameraData cameraData)
        {
            if (_accumulateVisibilityBufferMaterial == null)
            {
                return;
            }

            Camera camera = cameraData.Camera;
            context.SetupCameraProperties(camera);

            if (cameraData.CameraType == BlindFPSCameraType.Main)
            {
                AttachmentDescriptor albedoAttachment = new AttachmentDescriptor(RenderTextureFormat.ARGB32);
                AttachmentDescriptor normalsAttachment = new AttachmentDescriptor(RenderTextureFormat.ARGBFloat);
                AttachmentDescriptor worldPositionAttachment = new AttachmentDescriptor(RenderTextureFormat.ARGBFloat);
                AttachmentDescriptor visibilityBufferAttachment = new AttachmentDescriptor(RenderTextureFormat.ARGB32);
                AttachmentDescriptor depthBufferAttachment = new AttachmentDescriptor(RenderTextureFormat.Depth);
                AttachmentDescriptor outputAttachment = new AttachmentDescriptor(RenderTextureFormat.ARGB32);

                albedoAttachment.ConfigureClear(Color.black);
                normalsAttachment.ConfigureClear(Color.black);
                worldPositionAttachment.ConfigureClear(Color.black);
                visibilityBufferAttachment.ConfigureClear(Color.black);
                depthBufferAttachment.ConfigureClear(Color.black, 1.0f, 0);

                outputAttachment.ConfigureTarget(BuiltinRenderTextureType.CameraTarget, false, true);

                NativeArray<AttachmentDescriptor> attachmentDescriptors = new NativeArray<AttachmentDescriptor>(6, Allocator.Temp);
                attachmentDescriptors[AlbedoAttachmentIndex] = albedoAttachment;
                attachmentDescriptors[NormalsAttachmentIndex] = normalsAttachment;
                attachmentDescriptors[WorldPositionAttachmentIndex] = worldPositionAttachment;
                attachmentDescriptors[VisibilityBufferAttachmentIndex] = visibilityBufferAttachment;
                attachmentDescriptors[DepthAttachmentIndex] = depthBufferAttachment;
                attachmentDescriptors[OutputAttachmentIndex] = outputAttachment;

                context.BeginRenderPass(camera.pixelWidth, camera.pixelHeight, 1, 1, attachmentDescriptors, DepthAttachmentIndex);

                GBufferPass(context, cameraData);
                VisibilityBufferPass(context);
                FinalFramePass(context);

                context.EndRenderPass();

                attachmentDescriptors.Dispose();
            }
            else if (cameraData.CameraType == BlindFPSCameraType.Viewmodel)
            {
                AttachmentDescriptor outputAttachment = new AttachmentDescriptor(RenderTextureFormat.ARGB32);
                AttachmentDescriptor depthBufferAttachment = new AttachmentDescriptor(RenderTextureFormat.Depth);

                outputAttachment.ConfigureTarget(BuiltinRenderTextureType.CameraTarget, true, true);
                depthBufferAttachment.ConfigureClear(Color.black, 1.0f, 0);

                NativeArray<AttachmentDescriptor> attachmentDescriptors = new NativeArray<AttachmentDescriptor>(2, Allocator.Temp);
                attachmentDescriptors[0] = outputAttachment;
                attachmentDescriptors[1] = depthBufferAttachment;

                context.BeginRenderPass(camera.pixelWidth, camera.pixelHeight, 1, 1, attachmentDescriptors, 1);
                ViewmodelPass(context, cameraData);
                context.EndRenderPass();
            }

            context.Submit();
        }

        private void GBufferPass(ScriptableRenderContext context, BlindFPSCameraData cameraData)
        {
            Camera camera = cameraData.Camera;

            NativeArray<int> gbufferColors = new NativeArray<int>(3, Allocator.Temp);
            gbufferColors[0] = AlbedoAttachmentIndex;
            gbufferColors[1] = NormalsAttachmentIndex;
            gbufferColors[2] = WorldPositionAttachmentIndex;

            context.BeginSubPass(gbufferColors);

            CullingResults cullingResults = Cull(context, camera);

            SortingSettings opaqueSortingSettings = new(camera);
            opaqueSortingSettings.criteria = SortingCriteria.CommonOpaque;

            DrawingSettings opaqueDrawingSettings = new(ShaderTagFillGBuffer, opaqueSortingSettings);
            opaqueDrawingSettings.enableDynamicBatching = false;
            opaqueDrawingSettings.enableInstancing = false;
            opaqueDrawingSettings.perObjectData = PerObjectData.None;

            FilteringSettings opaqueFilteringSettings = new(RenderQueueRange.opaque);
            context.DrawRenderers(cullingResults, ref opaqueDrawingSettings, ref opaqueFilteringSettings);

            context.EndSubPass();

            gbufferColors.Dispose();
        }

        private void VisibilityBufferPass(ScriptableRenderContext context)
        {
            NativeArray<int> visibilityColors = new NativeArray<int>(1, Allocator.Temp);
            visibilityColors[0] = VisibilityBufferAttachmentIndex;

            NativeArray<int> visibilityInputs = new NativeArray<int>(2, Allocator.Temp);
            visibilityInputs[0] = WorldPositionAttachmentIndex;
            visibilityInputs[1] = DepthAttachmentIndex;

            context.BeginSubPass(visibilityColors, visibilityInputs, true, true);

            CommandBuffer commandBuffer = CommandBufferPool.Get();

            List<AudioWave> waves;
            if (Application.isPlaying)
            {
                waves = AudioWave.Instances;
            }
            else
            {
                waves = new List<AudioWave>(Object.FindObjectsByType<AudioWave>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
            }

            _accumulateVisibilityBufferMaterial.SetFloat(StartFallOffDistanceShaderId, _pipelineAsset.StartFallOff);
            _accumulateVisibilityBufferMaterial.SetFloat(EndFallOffDistanceShaderId, _pipelineAsset.EndFallOff);

            foreach (AudioWave wave in waves)
            {
                commandBuffer.DrawMesh(RenderUtils.FullscreenQuad, Matrix4x4.identity, _accumulateVisibilityBufferMaterial, 0, 0, wave.MaterialPropertyBlock);
            }

            context.ExecuteCommandBuffer(commandBuffer);
            context.EndSubPass();

            visibilityColors.Dispose();
            visibilityInputs.Dispose();

            CommandBufferPool.Release(commandBuffer);
        }

        private void FinalFramePass(ScriptableRenderContext context)
        {
            NativeArray<int> finalColors = new NativeArray<int>(1, Allocator.Temp);
            finalColors[0] = OutputAttachmentIndex;

            NativeArray<int> finalInputs = new NativeArray<int>(5, Allocator.Temp);
            finalInputs[0] = AlbedoAttachmentIndex;
            finalInputs[1] = VisibilityBufferAttachmentIndex;
            finalInputs[2] = NormalsAttachmentIndex;
            finalInputs[3] = WorldPositionAttachmentIndex;
            finalInputs[4] = DepthAttachmentIndex;

            context.BeginSubPass(finalColors, finalInputs, true, true);

            CommandBuffer commandBuffer = CommandBufferPool.Get();
            commandBuffer.name = "FinalFramePass";

            _finalFrameMaterial.SetColor(BaseColorShaderId, _pipelineAsset.BaseColor);

            commandBuffer.DrawMesh(RenderUtils.FullscreenQuad, Matrix4x4.identity, _finalFrameMaterial, 0, 0);
            context.ExecuteCommandBuffer(commandBuffer);

            context.EndSubPass();

            finalColors.Dispose();
            finalInputs.Dispose();

            CommandBufferPool.Release(commandBuffer);
        }

        private void ViewmodelPass(ScriptableRenderContext context, BlindFPSCameraData cameraData)
        {
            Camera camera = cameraData.Camera;

            NativeArray<int> colors = new NativeArray<int>(1, Allocator.Temp);
            colors[0] = 0;

            context.BeginSubPass(colors);

            CullingResults cullingResults = Cull(context, camera);

            SortingSettings opaqueSortingSettings = new(camera);
            opaqueSortingSettings.criteria = SortingCriteria.CommonOpaque;

            DrawingSettings opaqueDrawingSettings = new(ShaderTagViewmodel, opaqueSortingSettings);
            opaqueDrawingSettings.enableDynamicBatching = false;
            opaqueDrawingSettings.enableInstancing = false;
            opaqueDrawingSettings.perObjectData = PerObjectData.None;

            FilteringSettings opaqueFilteringSettings = new(RenderQueueRange.opaque);
            context.DrawRenderers(cullingResults, ref opaqueDrawingSettings, ref opaqueFilteringSettings);

            context.EndSubPass();

            colors.Dispose();
        }

        private CullingResults Cull(ScriptableRenderContext context, Camera camera)
        {
            camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParameters);
            return context.Cull(ref cullingParameters);
        }
    }
}