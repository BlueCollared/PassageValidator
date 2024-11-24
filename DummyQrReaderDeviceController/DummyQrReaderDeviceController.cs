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

    public async Task<QrCodeInfo?> StartDetecting(CancellationToken cancellationToken)
    {        
        try
        {
            await Task.Delay(5000, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            return null;
        }

        QrCodeInfo qr = new();
        qr.TicketId = 3;
        qr.ValidTill = DateTimeOffset.Now.AddMinutes(30);
        return qr;
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
}
