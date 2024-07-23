namespace EtGate.Domain
{
    // TODO:  Let's see if we need to make a new type for result emananted by preleminary checks
    public record QrCodeValidationResult
    {
        public bool bGood;
        public bool bCallMade;

        public static readonly QrCodeValidationResult CallNotMade = new(); // TODO: SEE
    }
}