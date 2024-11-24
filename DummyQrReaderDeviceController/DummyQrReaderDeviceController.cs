using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;

namespace DummyQrReaderDeviceController;

public class DummyQrReaderDeviceController : IQrReaderController
{
    Task tskStatusDetection;
    bool bStop = false;
    QrReaderStaticData qrStatic = new("1.2.3");
    private readonly IDeviceStatusPublisher<QrReaderStatus> publisher;
    private IMessagePublisher<QrReaderStaticData> pubStaticData;

    public DummyQrReaderDeviceController(IDeviceStatusPublisher<QrReaderStatus> publisher,
        IMessagePublisher<QrReaderStaticData> pubStaticData)
    {
        this.publisher = publisher;
        this.pubStaticData = pubStaticData;
        tskStatusDetection = Task.Run(() =>
        {
            Worker();
        });            
    }

    public bool Start()
    {
        pubStaticData.Publish(qrStatic);
        return true;
    }

    public bool StartDetecting()
    {
        bStop = false;
        return true;
    }

    private void Worker()
    {
        while (!bStop)
        {
            publisher.Publish(new QrReaderStatus(false, false));

            Thread.Sleep(1000);

            publisher.Publish(new QrReaderStatus(true, false));

            Thread.Sleep(1000);
        }
    }

    public void Stop()
    {
        bStop = true;            
    }

    public void StopDetecting()
    {
        bStop = true;
    }

    //public void Reboot()
    //{
    //    throw new NotImplementedException();
    //}
}
