namespace ZestyBiteWebAppSolution.Helpers
{
    public static class VerificationCodeGenerator
    {
        public static string GetSixDigitCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}