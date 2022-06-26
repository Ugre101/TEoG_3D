namespace Static
{
    public static class GameTester
    {
        static bool firstCall = true;

        public static bool GetFirstCall()
        {
            bool orgValue = firstCall;
            firstCall = false;
            return orgValue;
        }
    }
}