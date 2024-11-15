public static class CpfHelper
{
    public static string RemoveCpfFormat(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return string.Empty;

        // Removes all non-digit characters
        return Regex.Replace(cpf, @"\D", "");
    }
}
