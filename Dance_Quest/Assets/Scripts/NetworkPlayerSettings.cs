using UnityEngine;

namespace DanceQuest
{
    /// <summary>
    /// This static class generates a unique nickname for each networked player that joins the server
    /// </summary>
    public static class NetworkPlayerSettings
    {
        // Read-only property


        private static string _nickName = "CUBE #";
        public static string NickName
        {
            // TO DO: Consider assigning numbers in order of player count
            get
            {
                int number = Random.Range(1, 1000);     // Can have identical player numbers, so range is kept large
                return _nickName + number.ToString();
            }
        }
    }
}