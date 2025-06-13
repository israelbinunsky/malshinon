public static class local
{
    public static string ganarateCode(int len)
    {
        string code = "";
        Random random = new Random();
        for (int i = 0; i < len; i++)
        {
            code += random.Next(9);
        }
        return code;
    }

   public static string[] getTargetName(string txt)
    {
        string[] names = new string[2];
        string[] reportWards = txt.Split(' ');
        for (int i = 0; i < reportWards.Length - 1; i++)
        {
            if (char.IsUpper(reportWards[i][0]) && char.IsUpper(reportWards[i + 1][0]))
            {
                names[0] = reportWards[i];
                names[1] = reportWards[i + 1];
            }
        }
        return names;
    }
}