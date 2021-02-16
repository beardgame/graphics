
namespace Bearded.Graphics.Pipelines.Steps
{
    sealed class Action<TState> : IPipeline<TState>
    {
        private readonly System.Action<TState> action;

        public Action(System.Action<TState> action)
        {
            this.action = action;
        }

        public void Execute(TState state)
        {
            action(state);
        }
    }
}
