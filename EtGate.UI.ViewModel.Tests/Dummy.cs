using Avalonia.Controls;
using Moq;

namespace EtGate.UI.ViewModel.Tests
{
    internal class Dummy
    {
        public IViewFactory Dummy_IViewFactory { get; private set; }
        public ContentControl Dummy_ContentControl { get; private set; } = new Mock<ContentControl>().Object;
        public Dummy()
        {            
            Dummy_IViewFactory = IViewFactory();
        }


        private static IViewFactory IViewFactory()
        {
            var res = new Mock<IViewFactory>();
            res.Setup(m => m.Create(It.IsAny<Type>()))
                .Returns(() => new UserControl());
            return res.Object;
        }
    }
}
