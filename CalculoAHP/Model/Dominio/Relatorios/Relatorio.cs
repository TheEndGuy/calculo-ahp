using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CalculoAHP.Model.Dominio.Relatorios
{
    public class Relatorio
    {
        private Relatorio()
        {
        }

        private static Relatorio m_instance;

        public static Relatorio Instance
        {
            get { return m_instance ?? (m_instance = new Relatorio()); }
            set { m_instance = value; }
        }

        public void GerarRelatorio(Lancamento lancamentoGerar)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 40, 40, 40, 80);
            doc.AddCreationDate();

            Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"\Relatorios");

            string nomeRelatorio = "relatorio_" + lancamentoGerar.Id + "_" + lancamentoGerar.Id + DateTime.Now.Millisecond + ".pdf";
            string caminho = System.AppDomain.CurrentDomain.BaseDirectory + @"\Relatorios\" + nomeRelatorio;

            // exception interna porém é possível continuar
            PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.OpenOrCreate));

            doc.Open();

            // Título inicial
            Paragraph paragrafo = new Paragraph("", new Font(Font.NORMAL, 12))
            {
                Alignment = Element.ALIGN_LEFT,
                Font = new Font(Font.FontFamily.TIMES_ROMAN, 13, 1)
            };

            paragrafo.Add("Relatório de Decisão");
            doc.Add(paragrafo);

            paragrafo.Clear();

            // Data
            paragrafo.Font = new Font(Font.FontFamily.TIMES_ROMAN, 11, 1)
            {
                Color = new BaseColor(128, 128, 128)
            };

            paragrafo.Add("Data : " + DateTime.Now.ToString());
            doc.Add(paragrafo);

            paragrafo.Clear();

            doc.Add(new Paragraph(" "));
            LineSeparator line = new LineSeparator(1f, 100f, new BaseColor(0,0,0), Element.ALIGN_LEFT, 1);
            doc.Add(line);

            // Dados lançamento
            paragrafo.Font = new Font(Font.FontFamily.TIMES_ROMAN, 12, 1);

            // nome
            var firstFont = new Font(Font.FontFamily.TIMES_ROMAN, 12, 1);
            var secondFont = FontFactory.GetFont("Times New Roman", 12, new BaseColor(128, 128, 128));

            Chunk title;
            Chunk description;
            Phrase phrase;

            title = new Chunk("Nome da decisão : ", firstFont);
            description = new Chunk(lancamentoGerar.Nome, secondFont);

            phrase = new Phrase(title)
            {
                description
            };

            phrase.Add("\n");
            doc.Add(phrase);

            // descrição
            title = new Chunk("Descrição da decisão : ", firstFont);
            description = new Chunk(lancamentoGerar.Descricao, secondFont);

            phrase = new Phrase(title)
            {
                description
            };

            phrase.Add("\n");
            doc.Add(phrase);

            // data
            title = new Chunk("Data da decisão : ", firstFont);
            description = new Chunk(lancamentoGerar.Data, secondFont);

            phrase = new Phrase(title)
            {
                description
            };

            phrase.Add("\n");
            doc.Add(phrase);

            // modelo
            title = new Chunk("Modelo utilizado : ", firstFont);
            description = new Chunk(lancamentoGerar.Modelo.Nome, secondFont);

            phrase = new Phrase(title)
            {
                description
            };

            phrase.Add("\n");
            doc.Add(phrase);

            doc.Add(new Paragraph(" "));
            doc.Add(line);

            // Tabela 1
            paragrafo.Font = new Font(Font.FontFamily.TIMES_ROMAN, 12, 1);
            paragrafo.Alignment = Element.ALIGN_CENTER;

            paragrafo.Add("Tabela 1 - Informações sobre os elementos");
            doc.Add(paragrafo);
            doc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(1 + lancamentoGerar.Modelo.Criterios.Count);

            table.AddCell(new Phrase(new Chunk("Nome", new Font(Font.FontFamily.TIMES_ROMAN, 12, 1))));

            foreach (var criterio in lancamentoGerar.Modelo.Criterios)
                table.AddCell(new Phrase(new Chunk(criterio.Nome, new Font(Font.FontFamily.TIMES_ROMAN, 12, 1))));

            foreach(var element in lancamentoGerar.Elementos)
            {
                table.AddCell(element.Nome);

                foreach (var criterio in element.Criterios)
                    table.AddCell(Convert.ToString(criterio.Importancia));
            }

            doc.Add(table);
            
            doc.Add(new Paragraph(" "));
            doc.Add(line);
            paragrafo.Clear();

            // Tabela 2
            paragrafo.Font = new Font(Font.FontFamily.TIMES_ROMAN, 12, 1);
            paragrafo.Alignment = Element.ALIGN_CENTER;

            paragrafo.Add("Tabela 2 - Dados da tomada de decisão");
            doc.Add(paragrafo);
            doc.Add(new Paragraph(" "));

            table = new PdfPTable(2);

            table.AddCell(new Phrase(new Chunk("Nome", new Font(Font.FontFamily.TIMES_ROMAN, 12, 1))));
            table.AddCell(new Phrase(new Chunk("Peso", new Font(Font.FontFamily.TIMES_ROMAN, 12, 1))));

            foreach (var element in lancamentoGerar.ItensDecisao.OrderByDescending(entry=> entry.Peso))
            {
                table.AddCell(element.Nome);
                table.AddCell(Convert.ToString(element.Peso));
            }

            doc.Add(table);

            doc.Close();
            System.Diagnostics.Process.Start(caminho);
        }
    }
}
