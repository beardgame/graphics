using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Graphics.RenderSettings;
using Bearded.Graphics.Shading;

namespace Bearded.Graphics.Rendering
{
    public sealed class BatchedRenderer : IRenderer
    {
        private readonly IBatchedRenderable renderable;
        private readonly ImmutableArray<IRenderSetting> settings;

        private readonly LinkedList<DrawCall> activeDrawCallsInOrder = new();
        private readonly Dictionary<IRenderable, LinkedListNode<DrawCall>> activeDrawCalls = new();
        private readonly Dictionary<IRenderable, LinkedListNode<DrawCall>> inactiveDrawCalls = new();
        private readonly List<IRenderable> batchesWaitingForActivation = new();

        private ShaderProgram? shaderProgram;
        private ImmutableArray<IProgramRenderSetting> settingsForProgram;

        public static BatchedRenderer From(IBatchedRenderable renderable)
        {
            return From(renderable, Enumerable.Empty<IRenderSetting>());
        }

        public static BatchedRenderer From(IBatchedRenderable renderable, params IRenderSetting[] settings)
        {
            return From(renderable, settings.AsEnumerable());
        }

        public static BatchedRenderer From(IBatchedRenderable renderable, IEnumerable<IRenderSetting> settings)
        {
            return new BatchedRenderer(renderable, null, settings);
        }

        public static BatchedRenderer From(IBatchedRenderable renderable, ShaderProgram shaderProgram)
        {
            return From(renderable, shaderProgram, Enumerable.Empty<IRenderSetting>());
        }

        public static BatchedRenderer From(
            IBatchedRenderable renderable, ShaderProgram shaderProgram, params IRenderSetting[] settings)
        {
            return From(renderable, shaderProgram, settings.AsEnumerable());
        }

        public static BatchedRenderer From(
            IBatchedRenderable renderable, ShaderProgram shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            return new BatchedRenderer(renderable, shaderProgram, settings);
        }

        private BatchedRenderer(
            IBatchedRenderable renderable, ShaderProgram? shaderProgram, IEnumerable<IRenderSetting> settings)
        {
            this.renderable = renderable;
            this.settings = settings.ToImmutableArray();

            renderable.BatchActivated += onBatchActivated;
            renderable.BatchDeactivated += onBatchDeactivated;

            if (shaderProgram != null)
                SetShaderProgram(shaderProgram);
        }

        private void onBatchActivated(IRenderable batch)
        {
            batchesWaitingForActivation.Add(batch);
        }

        private void onBatchDeactivated(IRenderable batch)
        {
            if (activeDrawCalls.TryGetValue(batch, out var drawCall))
            {
                activeDrawCallsInOrder.Remove(drawCall);
                inactiveDrawCalls[batch] = drawCall;
                activeDrawCalls.Remove(batch);
            }
            else
            {
                batchesWaitingForActivation.Remove(batch);
            }
        }

        public void SetShaderProgram(ShaderProgram program)
        {
            shaderProgram = program;

            activeDrawCallsInOrder.Clear();
            disposeAndClear(activeDrawCalls);
            disposeAndClear(inactiveDrawCalls);

            batchesWaitingForActivation.Clear();
            batchesWaitingForActivation.AddRange(renderable.GetActiveBatches());

            settingsForProgram = settings.Select(s => s.ForProgram(program)).ToImmutableArray();
        }

        public void Render()
        {
            if (shaderProgram == null)
                throw new InvalidOperationException("Must set renderer shader program before rendering.");

            activateQueuedDrawCallsFor(shaderProgram);

            using (shaderProgram.Use())
            {
                foreach (var setting in settingsForProgram)
                {
                    setting.Set();
                }

                foreach (var drawCall in activeDrawCallsInOrder)
                {
                    drawCall.Invoke();
                }
            }
        }

        private void activateQueuedDrawCallsFor(ShaderProgram program)
        {
            foreach (var batch in batchesWaitingForActivation)
            {
                if (inactiveDrawCalls.TryGetValue(batch, out var node))
                {
                    inactiveDrawCalls.Remove(batch);
                }
                else
                {
                    var drawCall = batch.MakeDrawCallFor(program);
                    node = new LinkedListNode<DrawCall>(drawCall);
                }
                activeDrawCalls[batch] = node;
                activeDrawCallsInOrder.AddLast(node);
            }
            batchesWaitingForActivation.Clear();
        }

        public void Dispose()
        {
            disposeAndClear(activeDrawCalls);
            disposeAndClear(inactiveDrawCalls);
        }

        private static void disposeAndClear(Dictionary<IRenderable, LinkedListNode<DrawCall>> drawCalls)
        {
            foreach (var (_, drawCall) in drawCalls)
            {
                drawCall.Value.Dispose();
            }

            drawCalls.Clear();
        }
    }
}
