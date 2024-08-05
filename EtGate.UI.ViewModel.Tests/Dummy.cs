using Avalonia.Controls;
using Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;
using Moq;
using System.Reactive.Subjects;

namespace EtGate.UI.ViewModel.Tests
{
    internal class Dummy
    {
        public IQrReaderMgr Dummy_IQrReaderMgr { get; private set; }
        public IViewFactory Dummy_IViewFactory { get; private set; }
        public ContentControl Dummy_ContentControl { get; private set; } = new Mock<ContentControl>().Object;
        public Dummy()
        {
            Dummy_IQrReaderMgr = IQrReaderMgr();
            Dummy_IViewFactory = IViewFactory();
        }

        private static IQrReaderMgr IQrReaderMgr()
        {
            var res = new Mock<IQrReaderMgr>();
            res.Setup(x => x.StatusStream)
                .Returns(new Subject<QrReaderStatus>());

            return res.Object;
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
