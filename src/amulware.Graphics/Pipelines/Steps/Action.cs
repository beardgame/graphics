
namespace amulware.Graphics.Pipelines.Steps
{
    sealed class Action : IPipeline
    {
        private readonly System.Action action;

        public Action(System.Action action)
        {
            this.action = action;
        }

        public void Execute()
        {
            action();
        }
    }
}
