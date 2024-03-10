namespace SaYSpin.src
{
    public record struct Err(string Error)
    {
        public static Err New(string error) =>
            new Err(error);
        public override string ToString() => Error;
    }
}
