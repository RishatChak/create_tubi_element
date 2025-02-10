using Aveva.ApplicationFramework.Presentation;
using Aveva.ApplicationFramework;


namespace Aveva.C.Sharp.Training
{
    public class AddCollectAddin : IAddin
    {
        public string Name => "CollFormAddin";
  
        public string Description => "Coll Form addin";

        [System.Obsolete]
        public void Start(ServiceManager serviceManager)
        {
            var wndManager = DependencyResolver.GetImplementationOf<IWindowManager>();
            var cmdManager = DependencyResolver.GetImplementationOf<ICommandManager>();

            var collCmd = new AddCollectCmd(wndManager);

            cmdManager.Commands.Add(collCmd);
        }

        public void Stop() { }
    }
}
