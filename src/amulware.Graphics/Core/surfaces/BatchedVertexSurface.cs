using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace amulware.Graphics
{
    public sealed class BatchedVertexSurface<TVertexData> : Surface where TVertexData : struct, IVertexData
    {
        public class Batch
        {
            public VertexBuffer<TVertexData> VertexBuffer { get; private set; }

            private readonly List<SurfaceSetting> settingsSet = new List<SurfaceSetting>();
            private readonly List<SurfaceSetting> settingsUnSet = new List<SurfaceSetting>();

            public bool NeedsUploading { get; private set; }

            public Batch(VertexBuffer<TVertexData> buffer)
            {
                this.VertexBuffer = buffer ?? new VertexBuffer<TVertexData>();
            }

            public void MarkAsDirty()
            {
                this.NeedsUploading = true;
            }

            public void BufferData()
            {
                this.VertexBuffer.BufferData();
                this.NeedsUploading = false;
            }

            /// <summary>
            /// Adds <see cref="SurfaceSetting"/>s to this batch.
            /// </summary>
            /// <param name="settings">The settings.</param>
            public void AddSettings(IEnumerable<SurfaceSetting> settings)
            {
                foreach (SurfaceSetting setting in settings)
                    this.AddSetting(setting);
            }

            /// <summary>
            /// Adds <see cref="SurfaceSetting"/>s to this batch.
            /// </summary>
            /// <param name="settings">The settings.</param>
            public void AddSettings(params SurfaceSetting[] settings)
            {
                foreach (SurfaceSetting setting in settings)
                    this.AddSetting(setting);
            }

            /// <summary>
            /// Adds a <see cref="SurfaceSetting"/> to this batch.
            /// </summary>
            /// <param name="setting">The setting.</param>
            public void AddSetting(SurfaceSetting setting)
            {
                this.settingsSet.Add(setting);
                if (setting.NeedsUnsetting)
                    this.settingsUnSet.Add(setting);
            }

            /// <summary>
            /// Removes a <see cref="SurfaceSetting"/> from this batch.
            /// </summary>
            /// <param name="setting">The setting.</param>
            public void RemoveSetting(SurfaceSetting setting)
            {
                if (this.settingsSet.Remove(setting) && setting.NeedsUnsetting)
                    this.settingsUnSet.Remove(setting);
            }

            /// <summary>
            /// Removes all <see cref="SurfaceSetting"/>s from this batch.
            /// </summary>
            public void ClearSettings()
            {
                this.settingsSet.Clear();
                this.settingsUnSet.Clear();
            }

            public void SetAllSettings(ShaderProgram program)
            {
                foreach (SurfaceSetting setting in this.settingsSet)
                    setting.Set(program);
            }

            public void UnsetAllSettings(ShaderProgram program)
            {
                foreach (SurfaceSetting setting in this.settingsUnSet)
                    setting.UnSet(program);
            }
        }

        private struct BatchContainer
        {
            public readonly Batch Batch;
            public readonly VertexArray<TVertexData> VertexArray;

            private BatchContainer(Batch batch, VertexArray<TVertexData> vertexArray)
            {
                this.VertexArray = vertexArray;
                this.Batch = batch;
            }

            public static BatchContainer Make(VertexBuffer<TVertexData> buffer)
            {
                var batch = new Batch(buffer);
                return new BatchContainer(batch, new VertexArray<TVertexData>(batch.VertexBuffer));
            }

            public void Delete()
            {
                this.VertexArray.Dispose();
                this.Batch.ClearSettings();
                this.Batch.VertexBuffer.Clear();
                this.Batch.VertexBuffer.Dispose();
            }
        }

        private List<BatchContainer> inactiveBatches = new List<BatchContainer>();
        private List<BatchContainer> activeBatches = new List<BatchContainer>();
        private readonly Stack<BatchContainer> unusedBatches = new Stack<BatchContainer>();

        private readonly PrimitiveType primitiveType;

        public int ActiveBatches { get { return this.activeBatches.Count; } }

        public BatchedVertexSurface(PrimitiveType primitiveType = PrimitiveType.Triangles)
        {
            this.primitiveType = primitiveType;
        }

        protected override void onNewShaderProgram()
        {
            foreach (var container in this.activeBatches)
            {
                container.VertexArray.SetShaderProgram(this.program);
            }

            foreach (var container in this.inactiveBatches)
            {
                container.VertexArray.SetShaderProgram(this.program);
            }

            foreach (var container in this.unusedBatches)
            {
                container.VertexArray.SetShaderProgram(this.program);
            }
        }

        protected override void render()
        {
            if (this.activeBatches.Count == 0)
                return;

            foreach (var batch in this.activeBatches)
            {
                if (batch.Batch.VertexBuffer.Count == 0)
                    continue;

                batch.Batch.SetAllSettings(this.program);

                GL.BindBuffer(BufferTarget.ArrayBuffer, batch.Batch.VertexBuffer);

                batch.VertexArray.SetVertexData();
                if(batch.Batch.NeedsUploading)
                    batch.Batch.BufferData();

                GL.DrawArrays(this.primitiveType, 0, batch.Batch.VertexBuffer.Count);

                batch.VertexArray.UnSetVertexData();

                batch.Batch.UnsetAllSettings(this.program);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public Batch GetEmptyVertexBuffer(bool draw = true)
        {
            var batch = this.unusedBatches.Count > 0
                ? this.unusedBatches.Pop()
                : BatchContainer.Make(null);

            this.addBatch(batch, draw);

            return batch.Batch;
        }

        public Batch AdoptVertexBuffer(VertexBuffer<TVertexData> buffer, bool draw = true)
        {
            var batch = BatchContainer.Make(buffer);

            this.addBatch(batch, draw);

            return batch.Batch;
        }

        private void addBatch(BatchContainer batch, bool draw)
        {
            if (this.program != null)
                batch.VertexArray.SetShaderProgram(this.program);

            (draw ? this.activeBatches : this.inactiveBatches).Add(batch);
        }

        public void ActivateVertexBuffer(Batch batch)
        {
            this.moveBetweenLists(this.activeBatches, this.inactiveBatches, batch);
        }

        public void InactivateVertexBuffer(Batch batch)
        {
            this.moveBetweenLists(this.inactiveBatches, this.activeBatches, batch);
        }

        private void moveBetweenLists(List<BatchContainer> list, List<BatchContainer> list2, Batch batch)
        {
            var i = list.FindIndex(b => b.Batch == batch);
            var batchContainer = list[i];
            list.RemoveAt(i);

            list2.Add(batchContainer);
        }

        public void SwapActiveAndInactiveBuffers()
        {
            var temp = this.inactiveBatches;
            this.inactiveBatches = this.activeBatches;
            this.activeBatches = temp;
        }

        public void DeleteVertexBuffer(Batch batch, bool cacheForLater = true)
        {
            this.deleteFromList(this.activeBatches, batch, cacheForLater);
        }

        public void DeleteInactiveVertexBuffer(Batch batch, bool cacheForLater = true)
        {
            this.deleteFromList(this.inactiveBatches, batch, cacheForLater);
        }

        private void deleteFromList(List<BatchContainer> list, Batch batch, bool cacheForLater)
        {
            var i = list.FindIndex(b => b.Batch == batch);
            var batchContainer = list[i];
            list.RemoveAt(i);
            if (cacheForLater)
            {
                batchContainer.Batch.ClearSettings();
                batchContainer.Batch.VertexBuffer.Clear();
                this.unusedBatches.Push(batchContainer);
            }
            else
            {
                batchContainer.Delete();
            }
        }
    }
}
