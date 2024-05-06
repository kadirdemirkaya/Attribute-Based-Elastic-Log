namespace Es_Log.Extensions
{
    public static class CharacterExtension
    {
        public static string ReplaceControllerTag(this string controllerName, string replacement = "")
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentNullException(nameof(controllerName), "Controller name cannot be null or empty.");
            }

            return controllerName.Replace("Controller", replacement);
        }
    }
}
