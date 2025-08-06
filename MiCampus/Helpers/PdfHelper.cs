using System.Text;
using UglyToad.PdfPig;

public static class PdfHelper
{
    public static string ExtractTextFromPdf(string path)
    {
        var sb = new StringBuilder();
        using var document = PdfDocument.Open(path);

        foreach (var page in document.GetPages())
        {
            sb.AppendLine(page.Text);
        }

        return sb.ToString();
    }

    public static string BuscarFragmentoRelacionado(string pdfText, string pregunta)
    {
        var palabrasClave = pregunta
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(p => p.Length > 4)
            .Select(p => p.ToLower())
            .ToList();

        var lineas = pdfText.Split('\n');
        var relevantes = lineas
            .Where(linea => palabrasClave.Any(palabra => linea.ToLower().Contains(palabra)))
            .Take(5);

        return string.Join('\n', relevantes);
    }

    public static string BuscarFragmentosEnMultiplesPDFs(string folderPath, string pregunta)
    {
        var fragmentos = new List<string>();
        var archivos = Directory.GetFiles(folderPath, "*.pdf");

        foreach (var archivo in archivos)
        {
            var texto = ExtractTextFromPdf(archivo);
            var fragmento = BuscarFragmentoRelacionado(texto, pregunta);

            if (!string.IsNullOrWhiteSpace(fragmento))
            {
                var nombre = Path.GetFileName(archivo);
                fragmentos.Add($"[Fuente: {nombre}]\n{fragmento}");
            }
        }

        return string.Join("\n\n", fragmentos);
    }
}
