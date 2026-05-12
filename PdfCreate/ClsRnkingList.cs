using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PdfCreate
{
    public class ClsRankingList : IDocument
    {
        private List<Kakunintest> _results;

        public ClsRankingList(List< Kakunintest> seiseki)
        {
            _results = seiseki;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            List<Kakunintest> query;
            query = _results.OrderByDescending( x => x.Goukei).ToList();

            foreach (var chunk in query.Chunk(30))
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);

                    page.MarginTop(50);
                    page.MarginBottom(10);
                    page.MarginHorizontal(35);  // 左右

                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content()
                        .Column(column => 
                        {
                            column.Item().Text("○○○テスト　成績順位表").FontFamily("MS PGothic").FontSize(16).AlignCenter();

                            column.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(60);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(60);
                                    columns.ConstantColumn(60);
                                });

                                // ヘッダ
                                table.Header(header =>
                                {
                                    header.Cell().BorderBottom(1).Text("No").Bold();
                                    header.Cell().BorderBottom(1).Text("氏名").Bold();
                                    header.Cell().BorderBottom(1).Text("点数").Bold();
                                    header.Cell().BorderBottom(1).Text("順位").Bold();
                                });

                                int i = 0;
                                foreach (var r in chunk)
                                {
                                    int rowCount = chunk.Count();
                                    int colCount = 4;

                                    //for ( int i = 0; i < rowCount; i++)
                                    //{
                                    //var r = chunk[i];

                                    // 各列まとめて処理
                                    var cells = new[]
                                    {
                                        r.Jno?.ToString() ?? "",
                                        r.Name ?? "",
                                        r.Goukei?.ToString() ?? "",
                                        r.Junni?.ToString() ?? ""
                                    };

                                    for (int col = 0; col < colCount; col++)
                                    {
                                        IContainer cell = table.Cell().Padding(2);

                                        // 外枠
                                        if (i == 0)
                                            cell = cell.BorderTop(1);

                                        if (i == rowCount - 1)
                                            cell = cell.BorderBottom(1);
                                        else
                                            cell = cell.BorderBottom(0.5f); // 明細行の細線

                                        if (col == 0)
                                            cell = cell.BorderLeft(1);

                                        if (col == colCount - 1)
                                            cell = cell.BorderRight(1);

                                        // 余白
                                        cell = cell.Padding(2);

                                        // 文字位置は最後の方
                                        if (col == 2 || col == 3)
                                            cell = cell.AlignCenter();

                                        cell.Text(cells[col]);
                                    }

                                    i = i + 1;

                                };
                            });
                        });
                });
            }
        }
    }
}

