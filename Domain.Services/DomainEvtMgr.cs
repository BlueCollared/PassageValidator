using EtGate.Domain.Services.Qr;

namespace Domain
{
    public class DomainEvtMgr
    {
        public DomainEvtMgr(IDomainEvtRepository domainEvtRepository,
            QrReaderMgr qrReaderMgr)
        {
            qrReaderMgr.StatusStream.Subscribe(x => { });
        }
    }
}